using System;
using System.Net.Sockets;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// TCP 内部服务端套接字
    /// </summary>
    public sealed unsafe class ServerSocket : TcpServer.ServerSocket<ServerAttribute, Server, ServerSocket, ServerSocketSender>
    {
        /// <summary>
        /// 自定义数据字节长度
        /// </summary>
        private int customDataSize;
        /// <summary>
        /// TCP 内部服务套接字数据发送
        /// </summary>
        internal ServerSocketSender Sender;
        /// <summary>
        /// TCP 内部服务端套接字
        /// </summary>
        /// <param name="server">TCP调用服务端</param>
        internal ServerSocket(Server server) : base(server) { }
        /// <summary>
        /// 释放接收数据缓冲区与异步事件对象
        /// </summary>
        private void close()
        {
            try
            {
#if DOTNET2
                DisposeSocket();
#else
                if (receiveAsyncEventArgs == null) DisposeSocket();
                else
                {
                    receiveAsyncEventArgs.Completed -= onReceive;
                    DisposeSocket();
                    SocketAsyncEventArgsPool.PushNotNull(ref receiveAsyncEventArgs);
                }
#endif
            }
            catch (Exception error)
            {
                Server.AddLog(error);
            }
            CloseFree();
            if (Sender != null) Sender.Close();
        }
        /// <summary>
        /// TCP 内部服务端套接字任务处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerSocket RunTask()
        {
            ServerSocket value = NextTask;
            Start();
            NextTask = null;
            return value;
        }
        /// <summary>
        /// TCP 内部服务端套接字任务错误处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerSocket ErrorTask()
        {
            ServerSocket value = NextTask;
            close();
            NextTask = null;
            return value;
        }
        /// <summary>
        /// TCP 内部服务端套接字任务处理
        /// </summary>
        internal void Start()
        {
            receiveBufferSize = Server.ReceiveBufferPool.Size;
#if !MONO
            Socket.ReceiveBufferSize = receiveBufferSize;
            Socket.SendBufferSize = Server.SendBufferPool.Size;
#endif
#if !DOTNET2
            receiveAsyncEventArgs = SocketAsyncEventArgsPool.Get();
#endif
            Server.ReceiveBufferPool.Get(ref ReceiveBuffer);
            Sender = new ServerSocketSender(this);
#if DOTNET2
            onReceiveAsyncCallback = onReceive;
#else
            receiveAsyncEventArgs.Completed += onReceive;
            receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
#endif
            if (Server.VerifyCommandIdentity == 0)
            {
                IsVerifyMethod = true;
                if (isReceiveCommand()) return;
            }
            else
            {
                IsVerifyMethod = false;
                if (isReceiveVerifyCommand()) return;
            }
            close();
        }
#if DOTNET2
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private void onReceive(IAsyncResult async)
#else
        /// <summary>
        /// 数据接收完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private void onReceive(object sender, SocketAsyncEventArgs async)
#endif
        {
#if DOTNET2
            receiveAsyncEventArgs = async;
#endif
            switch (ReceiveType)
            {
                case TcpServer.ServerSocketReceiveType.VerifyCommand: verifyCommandAsync(); return;
                case TcpServer.ServerSocketReceiveType.VerifyData: verifyDataAsync(); return;
                case TcpServer.ServerSocketReceiveType.Command: commandAsync(); return;
                case TcpServer.ServerSocketReceiveType.Data: dataAsync(); return;
                case TcpServer.ServerSocketReceiveType.BigData: bigDataAsync(); return;
            }
        }
        /// <summary>
        /// 接收验证命令
        /// </summary>
        /// <returns></returns>
        private bool isReceiveVerifyCommand()
        {
            Socket socket = Socket;
            if (socket != null)
            {
                ReceiveType = TcpServer.ServerSocketReceiveType.VerifyCommand;
                receiveTimeout = Date.NowTime.Now.AddSeconds(Server.Attribute.ReceiveVerifyCommandSeconds + 1);
#if DOTNET2
                IAsyncResult async = socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success)
                {
                    if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, socket);
                    return true;
                }
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex, receiveBufferSize);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
                    Server.PushReceiveVerifyCommandTimeout(this, socket);
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
#if !DotNetStandard
                Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                return isVerifyCommand();
#endif
            }
            return false;
        }
        /// <summary>
        /// 接收验证命令
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void verifyCommandAsync()
        {
#if DOTNET2
            if (!receiveAsyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
#else
            Server.CancelReceiveVerifyCommandTimeout(this);
#endif
            try
            {
                if (isVerifyCommand()) return;
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            close();
        }
        /// <summary>
        /// 接收验证命令
        /// </summary>
        /// <returns></returns>
        private bool isVerifyCommand()
        {
#if DOTNET2
            Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
            if (socket == Socket)
            {
                receiveCount = socket.EndReceive(receiveAsyncEventArgs, out socketError);
                if (socketError == SocketError.Success)
                {
#else
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                receiveCount = receiveAsyncEventArgs.BytesTransferred;
#endif
                    if (receiveCount >= (sizeof(int) * 2 + sizeof(uint)))
                    {
                        fixed (byte* receiveDataFixed = ReceiveBuffer.Buffer)
                        {
                            if (*(int*)(receiveDataStart = receiveDataFixed + ReceiveBuffer.StartIndex) == Server.VerifyCommandIdentity)
                            {
                                if ((compressionDataSize = *(int*)(receiveDataStart + (sizeof(uint) + sizeof(int)))) > 0)
                                {
                                    CommandIndex = *(uint*)(receiveDataStart + sizeof(int));
                                    dataSize = compressionDataSize;
                                    receiveIndex = sizeof(int) * 2 + sizeof(uint);
                                    return checkVerifyCommandFixed();
                                }
                                else if (compressionDataSize < 0 && receiveCount >= (sizeof(int) * 3 + sizeof(uint)))
                                {
                                    if ((dataSize = *(int*)(receiveDataStart + (sizeof(uint) + sizeof(int) * 2))) > (compressionDataSize = -compressionDataSize))
                                    {
                                        CommandIndex = *(uint*)(receiveDataStart + sizeof(int));
                                        receiveIndex = sizeof(int) * 3 + sizeof(uint);
                                        return checkVerifyCommandFixed();
                                    }
                                }
                            }
                            else if (Server.Log.IsAnyType(AutoCSer.Log.LogType.Info))
                            {
#if !DOTNET2
                            Socket socket = Socket;
#endif
                                Server.Log.Add(AutoCSer.Log.LogType.Info, socket == null ? "TCP 验证函数命令错误" : ("TCP 验证函数命令错误 " + socket.RemoteEndPoint.ToString()));
                            }
                        }
                    }
#if DOTNET2
                }
#endif
            }
#if !DOTNET2
            else socketError = receiveAsyncEventArgs.SocketError;
#endif
            return false;
        }
        /// <summary>
        /// 检查验证命令
        /// </summary>
        /// <returns></returns>
        private bool checkVerifyCommandFixed()
        {
            int maxVerifyDataSize = Math.Min(Server.Attribute.MaxVerifyDataSize, receiveBufferSize - receiveIndex);
            if (dataSize <= maxVerifyDataSize)
            {
                int nextSize = compressionDataSize - (receiveCount - receiveIndex);
                if (nextSize == 0)
                {
                    if (doVerifyCommand()) return true;
                }
                else if (nextSize > 0 && isReceiveVerifyData()) return true;
                if (!IsVerifyMethod && Server.Log.IsAnyType(AutoCSer.Log.LogType.Info))
                {
                    Socket socket = Socket;
                    Server.Log.Add(AutoCSer.Log.LogType.Info, socket == null ? "TCP 验证函数调用失败" : ("TCP 验证函数调用失败 " + socket.RemoteEndPoint.ToString()));
                }
            }
            else if (Server.Log.IsAnyType(AutoCSer.Log.LogType.Debug)) Server.Log.Add(AutoCSer.Log.LogType.Debug, "TCP 验证函数接收数据长度超限 " + dataSize.toString() + " > " + maxVerifyDataSize.toString());
            return false;
        }
        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <returns></returns>
        private bool isReceiveVerifyData()
        {
            Socket socket = Socket;
            if (socket != null)
            {
                ReceiveType = TcpServer.ServerSocketReceiveType.VerifyData;
#if DOTNET2
                IAsyncResult async = socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success)
                {
                    if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, socket);
                    return true;
                }
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
                    Server.PushReceiveVerifyCommandTimeout(this, socket);
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
#if !DotNetStandard
                Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                return isVerifyData();
#endif
            }
            return false;
        }
        /// <summary>
        /// 获取验证数据
        /// </summary>
        private void verifyDataAsync()
        {
#if DOTNET2
            if (!receiveAsyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
#else
            Server.CancelReceiveVerifyCommandTimeout(this);
#endif
            try
            {
                if (isVerifyData()) return;
                if (!IsVerifyMethod && Server.Log.IsAnyType(AutoCSer.Log.LogType.Info))
                {
                    Socket socket = Socket;
                    Server.Log.Add(AutoCSer.Log.LogType.Info, socket == null ? "TCP 验证函数调用失败" : ("TCP 验证函数调用失败 " + socket.RemoteEndPoint.ToString()));
                }
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            close();
        }
        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <returns></returns>
        private bool isVerifyData()
        {
            if (Date.NowTime.Now < receiveTimeout)
            {
#if DOTNET2
                Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
                if (socket == Socket)
                {
                    int count = socket.EndReceive(receiveAsyncEventArgs, out socketError);
                    if (socketError == SocketError.Success)
                    {
#else
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = receiveAsyncEventArgs.BytesTransferred;
#endif
                        int nextSize = compressionDataSize - ((receiveCount += count) - receiveIndex);
                        if (nextSize == 0)
                        {
                            ReceiveSizeLessCount = 0;
                            return doVerifyCommand();
                        }
                        return nextSize > 0 && (count >= TcpServer.Server.MinSocketSize || (count > 0 && ReceiveSizeLessCount++ == 0)) && isReceiveVerifyData();
#if DOTNET2
                    }
#endif
                }
#if !DOTNET2
                socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            else if (Server.Log.IsAnyType(AutoCSer.Log.LogType.Info))
            {
                Socket socket = Socket;
                Server.Log.Add(AutoCSer.Log.LogType.Info, socket == null ? "TCP 验证数据接收超时" : ("TCP 验证数据接收超时 " + socket.RemoteEndPoint.ToString()));
            }
            return false;
        }
        /// <summary>
        /// 执行函数验证
        /// </summary>
        /// <returns></returns>
        private bool doVerifyCommand()
        {
            if (compressionDataSize == dataSize)
            {
                SubArray<byte> data = new SubArray<byte> { Array = ReceiveBuffer.Buffer, Start = ReceiveBuffer.StartIndex + receiveIndex, Length = compressionDataSize };
                if (MarkData != 0) TcpServer.CommandBuffer.Mark(ref data, MarkData);
                Server.DoCommand(Server.VerifyCommandIdentity, Sender, ref data);
                if (IsVerifyMethod) return isReceiveCommand();
                if (--verifyMethodCount != 0) return isReceiveVerifyCommand();
            }
            else
            {
                if (MarkData != 0) TcpServer.CommandBuffer.Mark(ReceiveBuffer.Buffer, MarkData, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize);
                SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
                AutoCSer.IO.Compression.DeflateDeCompressor.Get(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize, ref buffer);
                if (buffer.Buffer != null)
                {
                    Server.DoCommand(Server.VerifyCommandIdentity, Sender, ref buffer, dataSize);
                    if (IsVerifyMethod) return isReceiveCommand();
                    if (--verifyMethodCount != 0) return isReceiveVerifyCommand();
                }
            }
            return false;
        }
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool isReceiveCommand()
        {
            Socket socket = Socket;
            if (socket != null)
            {
                receiveCount = receiveIndex = 0;
                ReceiveType = TcpServer.ServerSocketReceiveType.Command;
#if DOTNET2
                socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex, receiveBufferSize);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
#if !DotNetStandard
                Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                return isCommand();
#endif
            }
            return false;
        }
        /// <summary>
        /// 获取命令
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void commandAsync()
        {
            try
            {
                if (isCommand()) return;
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            close();
        }
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool isCommand()
        {
#if DOTNET2
            Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
            if (socket == Socket)
            {
                receiveCount += socket.EndReceive(receiveAsyncEventArgs, out socketError);
                if (socketError == SocketError.Success)
                {
#else
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                receiveCount += receiveAsyncEventArgs.BytesTransferred;
#endif
                    fixed (byte* receiveDataFixed = ReceiveBuffer.Buffer)
                    {
                        receiveDataStart = receiveDataFixed + ReceiveBuffer.StartIndex;
                        return loop(true);
                    }
#if DOTNET2
                }
#endif
            }
#if !DOTNET2
            else socketError = receiveAsyncEventArgs.SocketError;
#endif
            return false;
        }
        /// <summary>
        /// 循环处理命令
        /// </summary>
        /// <param name="isCommand"></param>
        /// <returns></returns>
        private bool loop(bool isCommand)
        {
            START:
            int receiveSize = receiveCount - receiveIndex;
            if (receiveSize == 0)
            {
                receiveCount = receiveIndex = 0;
                if (isCommand) return false;
            }
            else
            {
                if (receiveSize >= (sizeof(int) + sizeof(uint)))
                {
                    byte* start = receiveDataStart + receiveIndex;
                    if (Server.IsCommand(command = *(int*)start) && IsCommand(command))
                    {
                        switch (command - TcpServer.Server.MinCommandIndex)
                        {
                            case TcpServer.Server.MergeCommandIndex - TcpServer.Server.MinCommandIndex:
                                if ((compressionDataSize = *(int*)(start + sizeof(uint))) > 0)
                                {
                                    dataSize = compressionDataSize;
                                    receiveIndex += sizeof(int) + sizeof(uint);
                                    break;
                                }
                                if (receiveSize >= (sizeof(int) * 2 + sizeof(uint)))
                                {
                                    if ((dataSize = *(int*)(start + (sizeof(int) + sizeof(uint)))) <= (compressionDataSize = -compressionDataSize) || compressionDataSize == 0) return false;
                                    receiveIndex += sizeof(int) * 2 + sizeof(uint);
                                    break;
                                }
                                if (isCommand) return false;
                                goto COPY;
                            case TcpServer.Server.CancelKeepCommandIndex - TcpServer.Server.MinCommandIndex:
                                if (receiveSize >= (sizeof(int) * 2 + sizeof(uint)))
                                {
                                    if (*(int*)(start + (sizeof(int) + sizeof(uint))) != 0) return false;
                                    Sender.CancelKeepCallback(*(int*)(start + sizeof(int)));
                                    receiveIndex += sizeof(int) * 2 + sizeof(uint);
                                    isCommand = false;
                                    goto START;
                                }
                                if (isCommand) return false;
                                goto COPY;
                            case TcpServer.Server.CustomDataCommandIndex - TcpServer.Server.MinCommandIndex:
                                if (receiveSize >= (sizeof(int) * 2 + sizeof(uint)))
                                {
                                    if ((compressionDataSize = *(int*)(start + (sizeof(int) + sizeof(uint)))) > 0)
                                    {
                                        dataSize = compressionDataSize;
                                        if ((customDataSize = *(int*)(start + sizeof(int))) < 0 || (uint)(dataSize - customDataSize) >= 4)
                                        {
                                            Server.Log.Add(Log.LogType.Error, "客户端自定义数据解析错误");
                                            return false;
                                        }
                                        receiveIndex += sizeof(int) * 2 + sizeof(uint);
                                        break;
                                    }
                                    if (receiveSize >= (sizeof(int) * 3 + sizeof(uint)))
                                    {
                                        if ((dataSize = *(int*)(start + (sizeof(int) * 2 + sizeof(uint)))) <= (compressionDataSize = -compressionDataSize) || compressionDataSize == 0) return false;
                                        if ((customDataSize = *(int*)(start + sizeof(int))) < 0 || (uint)(dataSize - customDataSize) >= 4)
                                        {
                                            Server.Log.Add(Log.LogType.Error, "客户端自定义数据解析错误");
                                            return false;
                                        }
                                        receiveIndex += sizeof(int) * 3 + sizeof(uint);
                                        break;
                                    }
                                }
                                if (isCommand) return false;
                                goto COPY;
                            default:
                                if (((CommandIndex = *(uint*)(start + sizeof(int))) & (uint)TcpServer.CommandFlags.NullData) == 0)
                                {
                                    if (receiveSize >= (sizeof(int) * 2 + sizeof(uint)))
                                    {
                                        if ((compressionDataSize = *(int*)(start + (sizeof(uint) + sizeof(int)))) > 0)
                                        {
                                            dataSize = compressionDataSize;
                                            receiveIndex += (sizeof(int) * 2 + sizeof(uint));
                                            break;
                                        }
                                        if (receiveSize >= (sizeof(int) * 3 + sizeof(uint)))
                                        {
                                            if ((dataSize = *(int*)(start + (sizeof(uint) + sizeof(int) * 2))) > (compressionDataSize = -compressionDataSize) && compressionDataSize != 0)
                                            {
                                                receiveIndex += (sizeof(int) * 3 + sizeof(uint));
                                                break;
                                            }
                                            return false;
                                        }
                                    }
                                    if (isCommand) return false;
                                    goto COPY;
                                }
                                if (command != TcpServer.Server.RemoteExpressionCommandIndex && command != TcpServer.Server.RemoteExpressionNodeIdCommandIndex)
                                {
                                    receiveIndex += (sizeof(int) + sizeof(uint));
                                    Server.DoCommand(command, Sender, ref SubArray<byte>.Null);
                                    isCommand = false;
                                    goto START;
                                }
                                Server.Log.Add(Log.LogType.Error, "远程表达式服务端节点类型数据解析错误 " + command.toString());
                                return false;
                        }
                        if (compressionDataSize <= receiveCount - receiveIndex)
                        {
                            if (isDoCommandLoop())
                            {
                                isCommand = false;
                                goto START;
                            }
                            return false;
                        }
                        bool isDoCommand = false;
                        if (receiveCommandData(ref isDoCommand))
                        {
                            if (isDoCommand)
                            {
                                isCommand = false;
                                goto START;
                            }
                            return true;
                        }
                    }
                    return false;
                }
                else if (isCommand) return false;
                COPY:
                Memory.SimpleCopyNotNull64(receiveDataStart + receiveIndex, receiveDataStart, receiveCount = receiveSize);
                receiveIndex = 0;
            }
            Socket socket = Socket;
            if (socket != null)
            {
                ReceiveType = TcpServer.ServerSocketReceiveType.Command;
#if DOTNET2
                socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    receiveCount += receiveAsyncEventArgs.BytesTransferred;
                    isCommand = true;
                    goto START;
                }
                socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            return false;
        }
        /// <summary>
        /// 检查命令数据
        /// </summary>
        /// <param name="isDoCommand">是否执行了命令</param>
        /// <returns></returns>
        private bool receiveCommandData(ref bool isDoCommand)
        {
            if (compressionDataSize <= receiveBufferSize)
            {
                if (receiveIndex + compressionDataSize > receiveBufferSize)
                {
                    Memory.CopyNotNull(receiveDataStart + receiveIndex, receiveDataStart, receiveCount -= receiveIndex);
                    receiveIndex = 0;
                }
                ReceiveType = TcpServer.ServerSocketReceiveType.Data;
#if !DOTNET2
            RECEIVE:
#endif
                Socket socket = Socket;
                if (socket != null)
                {
#if DOTNET2
                    socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        return true;
                    }
                    if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        if (compressionDataSize <= (receiveCount += receiveAsyncEventArgs.BytesTransferred) - receiveIndex) return isDoCommand = isDoCommandLoop();
                        goto RECEIVE;
                    }
                    socketError = receiveAsyncEventArgs.SocketError;
#endif
                }
            }
            else
            {
                SubBuffer.Pool.GetBuffer(ref ReceiveBigBuffer, compressionDataSize);
                if (ReceiveBigBuffer.PoolBuffer.Pool == null) ++Server.ReceiveNewBufferCount;
                receiveBigBufferCount = receiveCount - receiveIndex;
                ReceiveType = TcpServer.ServerSocketReceiveType.BigData;
#if !DOTNET2
            BIGRECEIVE:
#endif
                Socket socket = Socket;
                if (socket != null)
                {
#if DOTNET2
                    socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, compressionDataSize - receiveBigBufferCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, compressionDataSize - receiveBigBufferCount);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        return true;
                    }
                    if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        if (compressionDataSize == (receiveBigBufferCount += receiveAsyncEventArgs.BytesTransferred)) return isDoCommand = isDoCommandBig();
                        goto BIGRECEIVE;
                    }
                    socketError = receiveAsyncEventArgs.SocketError;
#endif
                }
            }
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private void dataAsync()
        {
            try
            {
#if DOTNET2
                Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
                if (socket == Socket)
                {
                    receiveCount += socket.EndReceive(receiveAsyncEventArgs, out socketError);
                    if (socketError == SocketError.Success)
                    {
#else
            CHECK:
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    receiveCount += receiveAsyncEventArgs.BytesTransferred;
#endif
                        if (compressionDataSize <= receiveCount - receiveIndex)
                        {
                            fixed (byte* receiveDataFixed = ReceiveBuffer.Buffer)
                            {
                                receiveDataStart = receiveDataFixed + ReceiveBuffer.StartIndex;
                                if (isDoCommandLoop() && loop(false)) return;
                            }
                        }
                        else
                        {
#if DOTNET2
                            if (socket == Socket)
                            {
                                socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                                if (socketError == SocketError.Success) return;
                            }
#else
                        Socket socket = Socket;
                        if (socket != null)
                        {
#if !DotNetStandard
                            while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                            receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount);
                            if (socket.ReceiveAsync(receiveAsyncEventArgs))
                            {
#if !DotNetStandard
                                Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                                return;
                            }
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            goto CHECK;

                        }
#endif
                        }
#if DOTNET2
                    }
#endif
                }
#if !DOTNET2
                else socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            close();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private void bigDataAsync()
        {
            try
            {
#if DOTNET2
                Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
                if (socket == Socket)
                {
                    receiveBigBufferCount += socket.EndReceive(receiveAsyncEventArgs, out socketError);
                    if (socketError == SocketError.Success)
                    {
#else
            CHECK:
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    receiveBigBufferCount += receiveAsyncEventArgs.BytesTransferred;
#endif
                        int nextSize = compressionDataSize - receiveBigBufferCount;
                        if (nextSize == 0)
                        {
                            if (isDoCommandBig() && isReceiveCommand()) return;
                        }
                        else
                        {
#if DOTNET2
                            if (socket == Socket)
                            {
                                socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                                if (socketError == SocketError.Success) return;
                            }
#else
                        Socket socket = Socket;
                        if (socket != null)
                        {
#if !DotNetStandard
                            while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                            receiveAsyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize);
                            if (socket.ReceiveAsync(receiveAsyncEventArgs))
                            {
#if !DotNetStandard
                                Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                                return;
                            }
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            goto CHECK;
                        }
#endif
                        }
#if DOTNET2
                    }
#endif
                }
#if !DOTNET2
                else socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            close();
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <returns></returns>
        private bool isDoCommandBig()
        {
#if !DOTNET2
            receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
#endif
            Buffer.BlockCopy(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, receiveCount - receiveIndex);
            if (compressionDataSize == dataSize)
            {
                SubArray<byte> data = new SubArray<byte> { Array = ReceiveBigBuffer.Buffer, Start = ReceiveBigBuffer.StartIndex, Length = dataSize };
                doCommandMark(ref data);
                ReceiveBigBuffer.Free();
            }
            else
            {
                if (MarkData != 0) TcpServer.CommandBuffer.Mark(ReceiveBigBuffer.Buffer, MarkData, ReceiveBigBuffer.StartIndex, compressionDataSize);
                SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
                try
                {
                    AutoCSer.IO.Compression.DeflateDeCompressor.Get(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, compressionDataSize, ref buffer);
                }
                finally { ReceiveBigBuffer.Free(); }
                if (buffer.Buffer == null) return false;
                if (buffer.PoolBuffer.Pool == null) ++Server.ReceiveNewBufferCount;
                doCommand(ref buffer);
            }
            return true;
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <returns></returns>
        private bool isDoCommandLoop()
        {
            if (compressionDataSize == dataSize)
            {
                SubArray<byte> data = new SubArray<byte> { Array = ReceiveBuffer.Buffer, Start = ReceiveBuffer.StartIndex + receiveIndex, Length = compressionDataSize };
                doCommandMark(ref data);
                receiveIndex += compressionDataSize;
                return true;
            }
            if (MarkData != 0) TcpServer.CommandBuffer.Mark(ReceiveBuffer.Buffer, MarkData, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize);
            SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
            AutoCSer.IO.Compression.DeflateDeCompressor.Get(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize, ref buffer);
            if (buffer.Buffer != null)
            {
                doCommand(ref buffer);
                receiveIndex += compressionDataSize;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void doCommandMark(ref SubArray<byte> data)
        {
            if (MarkData != 0) TcpServer.CommandBuffer.Mark(ref data, MarkData);
            doCommand(ref data);
        }
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void doCommand(ref SubArray<byte> data)
        {
            switch (command - TcpServer.Server.MinCommandIndex)
            {
                case TcpServer.Server.MergeCommandIndex - TcpServer.Server.MinCommandIndex: Merge(ref data); return;
                case TcpServer.Server.CheckCommandIndex - TcpServer.Server.MinCommandIndex: return;
                case TcpServer.Server.CustomDataCommandIndex - TcpServer.Server.MinCommandIndex:
                    data.Length = customDataSize;
                    Server.CustomData(ref data);
                    return;
                case TcpServer.Server.RemoteExpressionCommandIndex - TcpServer.Server.MinCommandIndex: Sender.GetRemoteExpression(ref data); return;
                case TcpServer.Server.RemoteExpressionNodeIdCommandIndex - TcpServer.Server.MinCommandIndex: Sender.GetRemoteExpressionNodeId(ref data); return;
                default: Server.DoCommand(command, Sender, ref data); return;//isShutdown = true; 
            }
            
        }
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void doCommand(ref SubBuffer.PoolBufferFull buffer)
        {
            SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = dataSize };
            doCommand(ref data);
            buffer.PoolBuffer.Free();
        }
        /// <summary>
        /// 流合并命令处理
        /// </summary>
        /// <param name="data">输入数据</param>
        internal void Merge(ref SubArray<byte> data)
        {
            int receiveCount = data.Length;
            if (receiveCount >= (sizeof(int) + sizeof(uint)))
            {
                try
                {
                    byte[] dataArray = data.Array;
                    fixed (byte* dataFixed = dataArray)
                    {
                        int receiveIndex = data.Start, receiveSize;
                        receiveCount += data.Start;
                        do
                        {
                            byte* start = dataFixed + receiveIndex;
                            if (!Server.IsCommand(command = *(int*)start) || !IsCommand(command)) break;
                            switch (command - TcpServer.Server.MinCommandIndex)
                            {
                                case TcpServer.Server.CancelKeepCommandIndex - TcpServer.Server.MinCommandIndex:
                                    if (*(int*)(start + (sizeof(int) * 2)) != 0 || receiveCount - (receiveIndex += sizeof(int) * 3) < 0)
                                    {
                                        DisposeSocket();
                                        return;
                                    }
                                    Sender.CancelKeepCallback(*(int*)(start + sizeof(int)));
                                    break;
                                case TcpServer.Server.CustomDataCommandIndex - TcpServer.Server.MinCommandIndex:
                                    if ((dataSize = *(int*)(start + (sizeof(uint) + sizeof(int)))) < 0
                                        || dataSize > receiveCount - (receiveIndex += (sizeof(int) * 2 + sizeof(uint)))
                                        || (customDataSize = *(int*)(start + sizeof(int))) < 0 || (uint)(dataSize - customDataSize) >= 4)
                                    {
                                        DisposeSocket();
                                        return;
                                    }
                                    data.Set(receiveIndex, customDataSize);
                                    Server.CustomData(ref data);
                                    receiveIndex += dataSize;
                                    break;
                                case TcpServer.Server.RemoteExpressionCommandIndex - TcpServer.Server.MinCommandIndex:
                                case TcpServer.Server.RemoteExpressionNodeIdCommandIndex - TcpServer.Server.MinCommandIndex:
                                    if (((CommandIndex = *(uint*)(start + sizeof(int))) & (uint)TcpServer.CommandFlags.NullData) != 0
                                        || (dataSize = *(int*)(start + (sizeof(uint) + sizeof(int)))) <= 0
                                        || dataSize > receiveCount - (receiveIndex += (sizeof(int) * 2 + sizeof(uint))))
                                    {
                                        DisposeSocket();
                                        return;
                                    }
                                    data.Set(receiveIndex, dataSize);
                                    if (command == TcpServer.Server.RemoteExpressionCommandIndex) Sender.GetRemoteExpression(ref data);
                                    else Sender.GetRemoteExpressionNodeId(ref data);
                                    receiveIndex += dataSize;
                                    break;
                                default:
                                    if (((CommandIndex = *(uint*)(start + sizeof(int))) & (uint)TcpServer.CommandFlags.NullData) == 0)
                                    {
                                        if ((dataSize = *(int*)(start + (sizeof(uint) + sizeof(int)))) <= 0
                                            || dataSize > receiveCount - (receiveIndex += (sizeof(int) * 2 + sizeof(uint))))
                                        {
                                            DisposeSocket();
                                            return;
                                        }
                                        data.Set(receiveIndex, dataSize);
                                        Server.DoCommand(command, Sender, ref data);
                                        receiveIndex += dataSize;
                                    }
                                    else
                                    {
                                        Server.DoCommand(command, Sender, ref SubArray<byte>.Null);
                                        receiveIndex += (sizeof(int) + sizeof(uint));
                                    }
                                    break;
                            }
                            if ((receiveSize = receiveCount - receiveIndex) == 0) return;
                        }
                        while (receiveSize >= (sizeof(int) + sizeof(uint)));
                    }
                }
                catch (Exception error)
                {
                    Server.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
            }
            DisposeSocket();
        }

        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SendCustomData(byte[] data)
        {
            return Sender.CustomData(data);
        }
        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SendCustomData(ref SubArray<byte> data)
        {
            return Sender.CustomData(ref data);
        }
        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SendCustomData(SubArray<byte> data)
        {
            return Sender.CustomData(ref data);
        }
    }
}
