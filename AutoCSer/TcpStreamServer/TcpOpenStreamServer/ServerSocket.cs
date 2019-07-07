using System;
using System.Net.Sockets;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpOpenStreamServer
{
    /// <summary>
    /// TCP 服务端套接字
    /// </summary>
    public sealed unsafe class ServerSocket : TcpStreamServer.ServerSocket<ServerAttribute, Server, ServerSocket, ServerSocketSender>
    {
        /// <summary>
        /// TCP 内部服务套接字数据发送
        /// </summary>
        internal ServerSocketSender Sender;
        /// <summary>
        /// 最大输入数据长度
        /// </summary>
        private int maxInputSize;
        /// <summary>
        /// 最大合并输入数据长度
        /// </summary>
        private int maxMergeInputSize;
        /// <summary>
        /// TCP 内部服务端套接字
        /// </summary>
        /// <param name="server">TCP调用服务端</param>
        /// <param name="socket">客户端信息</param>
        internal ServerSocket(Server server, ref Socket socket)
            : base(server)
        {
            Socket = socket;
            socket = null;
        }
        /// <summary>
        /// TCP 服务端套接字任务处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerSocket RunTask()
        {
            ServerSocket value = NextTask;
            runTask();
            NextTask = null;
            return value;
        }
        /// <summary>
        /// TCP 服务端套接字任务错误处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServerSocket ErrorTask()
        {
            ServerSocket value = NextTask;
            Close();
            NextTask = null;
            return value;
        }
        /// <summary>
        /// 释放接收数据缓冲区与异步事件对象
        /// </summary>
        internal void Close()
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
        internal void Start()
        {
#if !MONO
            Socket.ReceiveBufferSize = Server.ReceiveBufferPool.Size;
            Socket.SendBufferSize = Server.SendBufferPool.Size;
#endif
            if ((maxInputSize = Server.Attribute.MaxInputSize) <= 0) maxInputSize = int.MaxValue;
            if ((maxMergeInputSize = maxInputSize + Server.ReceiveBufferPool.Size) < 0) maxMergeInputSize = int.MaxValue;
#if !DOTNET2
            receiveAsyncEventArgs = SocketAsyncEventArgsPool.Get();
#endif
            Server.ReceiveBufferPool.Get(ref ReceiveBuffer);
            Sender = new ServerSocketSender(this);
            receiveBufferSize = ReceiveBuffer.PoolBuffer.Pool.Size;
#if DOTNET2
            onReceiveAsyncCallback = onReceive;
#else
            receiveAsyncEventArgs.Completed += onReceive;
            receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
#endif
            if (Server.VerifyCommandIdentity == 0)
            {
                IsVerifyMethod = true;
                receiveCount = receiveIndex = 0;
                ReceiveType = TcpServer.ServerSocketReceiveType.Command;
#if DOTNET2
                Socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, Socket);
                if (socketError == SocketError.Success) return;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex, receiveBufferSize);
                if (Socket.ReceiveAsync(receiveAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return;
                }
#if !DotNetStandard
                Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
#endif
            }
            else
            {
                IsVerifyMethod = false;
                ReceiveType = TcpServer.ServerSocketReceiveType.VerifyCommand;
                receiveTimeout = Date.NowTime.Now.AddSeconds(Server.Attribute.ReceiveVerifyCommandSeconds + 1);
#if DOTNET2
                IAsyncResult async = Socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, Socket);
                if (socketError == SocketError.Success)
                {
                    if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                    return;
                }
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex, receiveBufferSize);
                if (Socket.ReceiveAsync(receiveAsyncEventArgs))
                {
                    Server.PushReceiveVerifyCommandTimeout(this, Socket);
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return;
                }
#if !DotNetStandard
                Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
#endif
            }
#if !DOTNET2
            ServerSocketTask.Task.Add(this);
#endif
        }
        /// <summary>
        /// TCP 服务端套接字任务处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void runTask()
        {
            if (IsVerifyMethod ? isCommand() : isVerifyCommand()) return;
            Close();
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
            try
            {
                switch (ReceiveType)
                {
                    case TcpServer.ServerSocketReceiveType.VerifyCommand:
#if DOTNET2
                        if (!receiveAsyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
#else
                        Server.CancelReceiveVerifyCommandTimeout(this);
#endif
                        if (isVerifyCommand()) return;
                        break;
                    case TcpServer.ServerSocketReceiveType.VerifyData: if (verifyDataAsync()) return; break;
                    case TcpServer.ServerSocketReceiveType.Command: if (isCommand()) return; break;
                    case TcpServer.ServerSocketReceiveType.Data: if (dataAsync()) return; break;
                    case TcpServer.ServerSocketReceiveType.BigData: if (bigDataAsync()) return; break;
                    case TcpServer.ServerSocketReceiveType.CompressionData: if (compressionDataAsync()) return; break;
                    case TcpServer.ServerSocketReceiveType.CompressionBigData: if (compressionBigDataAsync()) return; break;
                }
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            Close();
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
                    if (receiveCount >= sizeof(int) * 2)
                    {
                        fixed (byte* receiveDataFixed = ReceiveBuffer.Buffer)
                        {
                            CommandIndex = (uint)(command = *(int*)(receiveDataStart = receiveDataFixed + ReceiveBuffer.StartIndex)) & TcpServer.Server.CommandFlagsAnd;
                            if ((command &= (int)TcpServer.Server.CommandIndexAnd) == Server.VerifyCommandIdentity)
                            {
                                if ((compressionDataSize = *(int*)(receiveDataStart + sizeof(uint))) > 0)
                                {
                                    int maxVerifyDataSize = Math.Min(Server.Attribute.MaxVerifyDataSize, receiveBufferSize - sizeof(int) * 2);
                                    if (compressionDataSize <= maxVerifyDataSize)
                                    {
                                        int nextSize = compressionDataSize - (receiveCount - sizeof(int) * 2);
                                        if (nextSize == 0)
                                        {
                                            if (doVerifyCommand()) return true;
                                        }
                                        else if (nextSize > 0 && isReceiveVerifyData()) return true;
                                        if (IsVerifyMethod) return false;
                                    }
                                    else
                                    {
                                        if (Server.Log.IsAnyType(AutoCSer.Log.LogType.Debug)) Server.Log.Add(AutoCSer.Log.LogType.Debug, "TCP 验证函数接收数据长度超限 " + compressionDataSize.toString() + " > " + maxVerifyDataSize.toString());
                                        return false;
                                    }
                                }
                                else return false;
                            }
                            if (Server.Log.IsAnyType(AutoCSer.Log.LogType.Info))
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
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    Server.PushReceiveVerifyCommandTimeout(this, socket);
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
        /// <returns></returns>
        private bool verifyDataAsync()
        {
#if DOTNET2
            if (!receiveAsyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
#else
            Server.CancelReceiveVerifyCommandTimeout(this);
#endif
            if (isVerifyData()) return true;
            if (!IsVerifyMethod && Server.Log.IsAnyType(AutoCSer.Log.LogType.Info))
            {
                Socket socket = Socket;
                Server.Log.Add(AutoCSer.Log.LogType.Info, socket == null ? "TCP 验证函数调用失败" : ("TCP 验证函数调用失败 " + socket.RemoteEndPoint.ToString()));
            }
            return false;
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
                        int nextSize = compressionDataSize - ((receiveCount += count) - sizeof(int) * 2);
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
            SubArray<byte> data = new SubArray<byte> { Array = ReceiveBuffer.Buffer, Start = ReceiveBuffer.StartIndex + sizeof(int) * 2, Length = compressionDataSize };
            if (MarkData != 0) TcpServer.CommandBuffer.Mark(ref data, MarkData);
            Server.DoCommand(Server.VerifyCommandIdentity, Sender, ref data);
            return IsVerifyMethod ? isReceiveCommand() : --verifyMethodCount != 0 && isReceiveVerifyCommand();
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
                ReceiveType = TcpServer.ServerSocketReceiveType.Command;
                receiveIndex = receiveCount = 0;
#if DOTNET2
                socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex, receiveBufferSize);
                if(socket.ReceiveAsync(receiveAsyncEventArgs))
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
                if (receiveSize >= sizeof(int))
                {
                    byte* start = receiveDataStart + receiveIndex;
                    if ((command = *(int*)start) < 0)
                    {
                        if (receiveSize >= sizeof(int) * 2)
                        {
                            if ((dataSize = *(int*)(start + sizeof(int))) <= (compressionDataSize = -command) || compressionDataSize == 0) return false;
                            receiveIndex += sizeof(int) * 2;
                            receiveSize -= sizeof(int) * 2;
                            if (compressionDataSize <= receiveSize)
                            {
                                if (doCompressionCommand())
                                {
                                    isCommand = false;
                                    goto START;
                                }
                                return false;
                            }
                            return receiveCompressionCommandData();
                        }
                        if (isCommand) return false;
                        goto COPY;
                    }
                    else
                    {
                        CommandIndex = (uint)command & TcpServer.Server.CommandFlagsAnd;
                        if (Server.IsCommand(command &= (int)TcpServer.Server.CommandIndexAnd) && IsCommand(command))
                        {
                            if (command != TcpServer.Server.CheckCommandIndex)
                            {
                                if ((CommandIndex & (uint)TcpServer.CommandFlags.NullData) == 0)
                                {
                                    if (receiveSize >= sizeof(int) * 2)
                                    {
                                        if ((compressionDataSize = *(int*)(start + sizeof(int))) > 0)
                                        {
                                            if (compressionDataSize <= receiveCount - (receiveIndex += sizeof(int) * 2))
                                            {
                                                doCommandLoop();
                                                isCommand = false;
                                                goto START;
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
                                    if (isCommand) return false;
                                    goto COPY;
                                }
                                if (command != TcpServer.Server.RemoteExpressionCommandIndex && command != TcpServer.Server.RemoteExpressionNodeIdCommandIndex) Server.DoCommand(command, Sender, ref SubArray<byte>.Null);
                                else return false;
                            }
                            receiveIndex += sizeof(int);
                            isCommand = false;
                            goto START;
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
                    IAsyncResult async = socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                        return true;
                    }
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
                        Server.PushReceiveVerifyCommandTimeout(this, Socket);
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
                        if (compressionDataSize <= (receiveCount += receiveAsyncEventArgs.BytesTransferred) - receiveIndex)
                        {
                            doCommandLoop();
                            return isDoCommand = true;
                        }
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
                    IAsyncResult async = socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, compressionDataSize - receiveBigBufferCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                        return true;
                    }
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, compressionDataSize - receiveBigBufferCount);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
                        Server.PushReceiveVerifyCommandTimeout(this, Socket);
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
                        if (compressionDataSize == (receiveBigBufferCount += receiveAsyncEventArgs.BytesTransferred))
                        {
                            doCommandBig();
                            return isDoCommand = true;
                        }
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
        /// <returns></returns>
        private bool dataAsync()
        {
#if DOTNET2
            if (!receiveAsyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
            Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
            if (socket == Socket)
            {
                int count = socket.EndReceive(receiveAsyncEventArgs, out socketError);
                if (socketError == SocketError.Success)
                {
#else
            Server.CancelReceiveVerifyCommandTimeout(this);
            CHECK:
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                int count = receiveAsyncEventArgs.BytesTransferred;
#endif
                    if (compressionDataSize <= (receiveCount += count) - receiveIndex)
                    {
                        ReceiveSizeLessCount = 0;
                        fixed (byte* receiveDataFixed = ReceiveBuffer.Buffer)
                        {
                            receiveDataStart = receiveDataFixed + ReceiveBuffer.StartIndex;
                            doCommandLoop();
                            if (loop(false)) return true;
                        }
                    }
                    else if (count >= TcpServer.Server.MinSocketSize || (count > 0 && ReceiveSizeLessCount++ == 0))
                    {
#if DOTNET2
                        if (socket == Socket)
                        {
                            IAsyncResult async = socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, receiveBufferSize - receiveCount, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                            if (socketError == SocketError.Success)
                            {
                                if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                                return true;
                            }
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
                            Server.PushReceiveVerifyCommandTimeout(this, Socket);
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            return true;
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
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        private bool bigDataAsync()
        {
#if DOTNET2
            if (!receiveAsyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
            Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
            if (socket == Socket)
            {
                int count = socket.EndReceive(receiveAsyncEventArgs, out socketError);
                if (socketError == SocketError.Success)
                {
#else
            Server.CancelReceiveVerifyCommandTimeout(this);
            CHECK:
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                int count = receiveAsyncEventArgs.BytesTransferred;
#endif
                    int nextSize = compressionDataSize - (receiveBigBufferCount += count);
                    if (nextSize == 0)
                    {
                        ReceiveSizeLessCount = 0;
                        doCommandBig();
                        if (isReceiveCommand()) return true;
                    }
                    else if (count >= TcpServer.Server.MinSocketSize || (count > 0 && ReceiveSizeLessCount++ == 0))
                    {
#if DOTNET2
                        if (socket == Socket)
                        {
                            IAsyncResult async = socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                            if (socketError == SocketError.Success)
                            {
                                if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                                return true;
                            }
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
                            Server.PushReceiveVerifyCommandTimeout(this, Socket);
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            return true;
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
            return false;
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        private void doCommandBig()
        {
#if !DOTNET2
            receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
#endif
            Buffer.BlockCopy(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, receiveCount - receiveIndex);
            SubArray<byte> data = new SubArray<byte> { Array = ReceiveBigBuffer.Buffer, Start = ReceiveBigBuffer.StartIndex, Length = compressionDataSize };
            doCommandMark(ref data);
            ReceiveBigBuffer.Free();
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void doCommandLoop()
        {
            SubArray<byte> data = new SubArray<byte> { Array = ReceiveBuffer.Buffer, Start = ReceiveBuffer.StartIndex + receiveIndex, Length = compressionDataSize };
            doCommandMark(ref data);
            receiveIndex += compressionDataSize;
        }
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void doCommandMark(ref SubArray<byte> data)
        {
            if (MarkData != 0) TcpServer.CommandBuffer.Mark(ref data, MarkData);
            switch (command - TcpServer.Server.RemoteExpressionNodeIdCommandIndex)
            {
                case TcpServer.Server.RemoteExpressionNodeIdCommandIndex - TcpServer.Server.RemoteExpressionNodeIdCommandIndex: Sender.GetRemoteExpressionNodeId(ref data, Server.Attribute.IsServerBuildOutputThread); return;
                case TcpServer.Server.RemoteExpressionCommandIndex - TcpServer.Server.RemoteExpressionNodeIdCommandIndex: Sender.GetRemoteExpression(ref data, Server.Attribute.IsServerBuildOutputThread); return;
                default: Server.DoCommand(command, Sender, ref data);return;
            }
        }
        ///// <summary>
        ///// 命令处理委托
        ///// </summary>
        ///// <param name="buffer"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //private void doCommand(ref SubBuffer.PoolBufferFull buffer)
        //{
        //    SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = compressionDataSize };
        //    Server.DoCommand(command, Sender, ref data);
        //    buffer.PoolBuffer.Free();
        //}
        /// <summary>
        /// 检查命令数据
        /// </summary>
        /// <returns></returns>
        private bool receiveCompressionCommandData()
        {
            int nextSize = compressionDataSize - (receiveCount - receiveIndex);
            if (compressionDataSize <= receiveBufferSize)
            {
                if (receiveIndex + compressionDataSize > receiveBufferSize)
                {
                    Memory.CopyNotNull(receiveDataStart + receiveIndex, receiveDataStart, receiveCount -= receiveIndex);
                    receiveIndex = 0;
                }
                ReceiveType = TcpServer.ServerSocketReceiveType.CompressionData;
#if !DOTNET2
                RECEIVE:
#endif
                Socket socket = Socket;
                if (socket != null)
                {
#if DOTNET2
                    IAsyncResult async = socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                        return true;
                    }
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, nextSize);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
                        Server.PushReceiveVerifyCommandTimeout(this, Socket);
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        return true;
                    }
                    if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                    {
                        int count = receiveAsyncEventArgs.BytesTransferred;
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        receiveCount += count;
                        if ((nextSize -= count) == 0) return doCompressionCommand() && isReceiveCommand();
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
                ReceiveType = TcpServer.ServerSocketReceiveType.CompressionBigData;
#if !DOTNET2
                BIGRECEIVE:
#endif
                Socket socket = Socket;
                if (socket != null)
                {
#if DOTNET2
                    IAsyncResult async = socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success)
                    {
                        if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                        return true;
                    }
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
                        Server.PushReceiveVerifyCommandTimeout(this, Socket);
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        return true;
                    }
                    if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                    {
                        int count = receiveAsyncEventArgs.BytesTransferred;
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        receiveBigBufferCount += count;
                        if ((nextSize -= count) == 0) return doCompressionBigDataCommand() && isReceiveCommand();
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
        /// <returns></returns>
        private bool compressionDataAsync()
        {
#if DOTNET2
            if (!receiveAsyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
            Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
            if (socket == Socket)
            {
                int count = socket.EndReceive(receiveAsyncEventArgs, out socketError);
                if (socketError == SocketError.Success)
                {
#else
            Server.CancelReceiveVerifyCommandTimeout(this);
            CHECK:
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                int count = receiveAsyncEventArgs.BytesTransferred;
#endif
                    int nextSize = compressionDataSize - ((receiveCount += count) - receiveIndex);
                    if (nextSize == 0)
                    {
                        ReceiveSizeLessCount = 0;
                        if (doCompressionCommand() && isReceiveCommand()) return true;
                    }
                    else if (count >= TcpServer.Server.MinSocketSize || (count > 0 && ReceiveSizeLessCount++ == 0))
                    {
#if DOTNET2
                        if (socket == Socket)
                        {
                            IAsyncResult async = socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                            if (socketError == SocketError.Success)
                            {
                                if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                                return true;
                            }
                        }
#else
                    Socket socket = Socket;
                    if (socket != null)
                    {
#if !DotNetStandard
                        while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                        receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, nextSize);
                        if (socket.ReceiveAsync(receiveAsyncEventArgs))
                        {
                            Server.PushReceiveVerifyCommandTimeout(this, Socket);
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            return true;
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
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        private bool compressionBigDataAsync()
        {
#if DOTNET2
            if (!receiveAsyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
            Socket socket = new Net.UnionType { Value = receiveAsyncEventArgs.AsyncState }.Socket;
            if (socket == Socket)
            {
                int count = socket.EndReceive(receiveAsyncEventArgs, out socketError);
                if (socketError == SocketError.Success)
                {
#else
            Server.CancelReceiveVerifyCommandTimeout(this);
            CHECK:
            if (receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                int count = receiveAsyncEventArgs.BytesTransferred;
#endif
                    int nextSize = compressionDataSize - (receiveBigBufferCount += count);
                    if (nextSize == 0)
                    {
                        ReceiveSizeLessCount = 0;
                        if (doCompressionBigDataCommand() && isReceiveCommand()) return true;
                    }
                    else if (count >= TcpServer.Server.MinSocketSize || (count > 0 && ReceiveSizeLessCount++ == 0))
                    {
#if DOTNET2
                        if (socket == Socket)
                        {
                            IAsyncResult async = socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                            if (socketError == SocketError.Success)
                            {
                                if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, Socket);
                                return true;
                            }
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
                            Server.PushReceiveVerifyCommandTimeout(this, Socket);
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            return true;
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
            return false;
        }
        /// <summary>
        /// 解压缩并执行命令
        /// </summary>
        /// <returns></returns>
        private bool doCompressionBigDataCommand()
        {
#if !DOTNET2
            receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
#endif
            Buffer.BlockCopy(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, receiveCount - receiveIndex);
            SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
            try
            {
                AutoCSer.IO.Compression.DeflateDeCompressor.Get(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, compressionDataSize, ref buffer);
            }
            finally { ReceiveBigBuffer.Free(); }
            return doCompressionCommand(ref buffer);
        }
        /// <summary>
        /// 解压缩并执行命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool doCompressionCommand()
        {
            SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
            AutoCSer.IO.Compression.DeflateDeCompressor.Get(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize, ref buffer);
            return doCompressionCommand(ref buffer);
        }
        /// <summary>
        /// 执行压缩命令
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private bool doCompressionCommand(ref SubBuffer.PoolBufferFull buffer)
        {
            if (buffer.Buffer != null)
            {
                receiveIndex += compressionDataSize;
                SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer };
                fixed (byte* dataFixed = buffer.Buffer)
                {
                    byte* start = dataFixed + buffer.StartIndex, end = start + dataSize;
                    do
                    {
                        CommandIndex = (uint)(command = *(int*)start) & TcpServer.Server.CommandFlagsAnd;
                        if (Server.IsCommand(command &= (int)TcpServer.Server.CommandIndexAnd) && IsCommand(command))
                        {
                            switch (command - TcpServer.Server.MinCommandIndex)
                            {
                                case TcpServer.Server.CheckCommandIndex - TcpServer.Server.MinCommandIndex:
                                    start += sizeof(int);
                                    break;
                                case TcpServer.Server.RemoteExpressionCommandIndex - TcpServer.Server.MinCommandIndex:
                                case TcpServer.Server.RemoteExpressionNodeIdCommandIndex - TcpServer.Server.MinCommandIndex:
                                    if ((CommandIndex & (uint)TcpServer.CommandFlags.NullData) == 0
                                        && (compressionDataSize = *(int*)(start + sizeof(int))) > 0 && (start += sizeof(int) * 2) + compressionDataSize <= end)
                                    {
                                        if (MarkData != 0) TcpServer.CommandBuffer.Mark(start, MarkData, compressionDataSize);
                                        data.Set((int)(start - dataFixed), compressionDataSize);
                                        if(command== TcpServer.Server.RemoteExpressionCommandIndex) Sender.GetRemoteExpression(ref data, Server.Attribute.IsServerBuildOutputThread);
                                        else Sender.GetRemoteExpressionNodeId(ref data, Server.Attribute.IsServerBuildOutputThread);
                                        start += compressionDataSize;
                                        break;
                                    }
                                    buffer.PoolBuffer.Free();
                                    return false;
                                default:
                                    if ((CommandIndex & (uint)TcpServer.CommandFlags.NullData) == 0)
                                    {
                                        if ((compressionDataSize = *(int*)(start + sizeof(int))) > 0 && (start += sizeof(int) * 2) + compressionDataSize <= end)
                                        {
                                            if (MarkData != 0) TcpServer.CommandBuffer.Mark(start, MarkData, compressionDataSize);
                                            data.Set((int)(start - dataFixed), compressionDataSize);
                                            Server.DoCommand(command, Sender, ref data);
                                            start += compressionDataSize;
                                            break;
                                        }
                                        buffer.PoolBuffer.Free();
                                        return false;
                                    }
                                    Server.DoCommand(command, Sender, ref SubArray<byte>.Null);
                                    start += sizeof(int);
                                    break;
                            }
                        }
                        else
                        {
                            buffer.PoolBuffer.Free();
                            return false;
                        }
                    }
                    while (start < end);
                }
                return true;
            }
            return false;
        }
    }
}
