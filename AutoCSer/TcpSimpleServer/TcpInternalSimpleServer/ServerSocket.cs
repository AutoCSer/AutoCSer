using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;
using System.Threading;

namespace AutoCSer.Net.TcpInternalSimpleServer
{
    /// <summary>
    /// TCP 内部服务端套接字
    /// </summary>
    public sealed unsafe class ServerSocket : TcpSimpleServer.ServerSocket<ServerAttribute, Server, ServerSocket>
    {
#if !NOJIT
        /// <summary>
        /// TCP 内部服务端套接字
        /// </summary>
        internal ServerSocket() : base() { }
#endif
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
            if (Socket != null)
            {
                try
                {
#if DOTNET2
                DisposeSocket();
#else
                    if (asyncEventArgs == null) DisposeSocket();
                    else
                    {
                        asyncEventArgs.Completed -= onReceive;
                        DisposeSocket();
                        SocketAsyncEventArgsPool.PushNotNull(ref asyncEventArgs);
                    }
#endif
                }
                catch (Exception error)
                {
                    Server.AddLog(error);
                }
                Buffer.Free();
                ReceiveBigBuffer.TryFree();
                FreeSerializer();
            }
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
            bufferSize = Server.ReceiveBufferPool.Size;
#if !MONO
            Socket.ReceiveBufferSize = Socket.SendBufferSize = bufferSize;
#endif
#if !DOTNET2
            asyncEventArgs = SocketAsyncEventArgsPool.Get();
#endif
            Server.ReceiveBufferPool.Get(ref Buffer);
            OutputStream = (OutputSerializer = BinarySerialize.Serializer.YieldPool.Default.Pop() ?? new BinarySerialize.Serializer()).SetTcpServer();
#if DOTNET2
            asyncCallback = onReceive;
#else
            asyncEventArgs.Completed += onReceive;
            asyncEventArgs.SetBuffer(Buffer.Buffer, Buffer.StartIndex, bufferSize);
#endif
            if (Server.VerifyCommandIdentity == 0)
            {
                IsVerifyMethod = true;
                if (isReceiveCommand()) return;
            }
            else
            {
                IsVerifyMethod = false;
                Socket.SendTimeout = Server.Attribute.ReceiveVerifyCommandSeconds * 1000;
                if (isReceiveVerifyCommand()) return;
            }
            close();
        }
        /// <summary>
        /// 通过函数验证处理
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public override void SetVerifyMethod()
        {
            IsVerifyMethod = true;
            Socket.SendTimeout = 0;
        }
#if DOTNET2
        /// <summary>
        /// 套接字操作完成后的回调委托
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private void onReceive(IAsyncResult async)
#else
        /// <summary>
        /// 套接字操作完成后的回调委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private void onReceive(object sender, SocketAsyncEventArgs async)
#endif
        {
#if DOTNET2
            asyncEventArgs = async;
#endif
            switch (SocketType)
            {
                case TcpSimpleServer.ServerSocketType.VerifyCommand: verifyCommandAsync(); return;
                case TcpSimpleServer.ServerSocketType.VerifyData: verifyDataAsync(); return;
                case TcpSimpleServer.ServerSocketType.Command: commandAsync(); return;
                case TcpSimpleServer.ServerSocketType.Data: dataAsync(); return;
                case TcpSimpleServer.ServerSocketType.BigData: bigDataAsync(); return;
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
                SocketType = TcpSimpleServer.ServerSocketType.VerifyCommand;
                receiveTimeout = Date.NowTime.Now.AddSeconds(Server.Attribute.ReceiveVerifyCommandSeconds + 1);
#if DOTNET2
                IAsyncResult async = socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex, bufferSize, SocketFlags.None, out socketError, asyncCallback, socket);
                if (socketError == SocketError.Success)
                {
                    if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, socket);
                    return true;
                }
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                asyncEventArgs.SetBuffer(Buffer.StartIndex, bufferSize);
                if (socket.ReceiveAsync(asyncEventArgs))
                {
                    Server.PushReceiveVerifyCommandTimeout(this, socket);
#if !DotNetStandard
                    Interlocked.Exchange(ref asyncLock, 0);
#endif
                    return true;
                }
#if !DotNetStandard
                Interlocked.Exchange(ref asyncLock, 0);
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
            if (!asyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
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
            Socket socket = new Net.UnionType { Value = asyncEventArgs.AsyncState }.Socket;
            if (socket == Socket)
            {
                receiveCount = socket.EndReceive(asyncEventArgs, out socketError);
                if (socketError == SocketError.Success)
                {
#else
            if (asyncEventArgs.SocketError == SocketError.Success)
            {
                receiveCount = asyncEventArgs.BytesTransferred;
#endif
                    if (receiveCount >= (sizeof(int) + sizeof(uint)))
                    {
                        fixed (byte* receiveDataFixed = Buffer.Buffer)
                        {
                            byte* start = receiveDataFixed + Buffer.StartIndex;
                            command = *(int*)start;
                            if (commandIndex == Server.VerifyCommandIdentity)
                            {
                                if ((compressionDataSize = *(int*)(start + sizeof(uint))) > 0)
                                {
                                    dataSize = compressionDataSize;
                                    receiveIndex = sizeof(int) + sizeof(uint);
                                    return checkVerifyCommandFixed();
                                }
                                else if (compressionDataSize < 0 && receiveCount >= (sizeof(int) * 2 + sizeof(uint)))
                                {
                                    if ((dataSize = *(int*)(start + (sizeof(uint) + sizeof(int)))) > (compressionDataSize = -compressionDataSize))
                                    {
                                        receiveIndex = sizeof(int) * 2 + sizeof(uint);
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
            else socketError = asyncEventArgs.SocketError;
#endif
            return false;
        }
        /// <summary>
        /// 检查验证命令
        /// </summary>
        /// <returns></returns>
        private bool checkVerifyCommandFixed()
        {
            int maxVerifyDataSize = Math.Min(Server.Attribute.MaxVerifyDataSize, bufferSize - receiveIndex);
            if (dataSize <= maxVerifyDataSize)
            {
                int receiveSize = receiveCount - receiveIndex;
                if (compressionDataSize == receiveSize)
                {
                    if (doVerifyCommand()) return true;
                }
                else if (compressionDataSize > receiveSize && isReceiveVerifyData()) return true;
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
                SocketType = TcpSimpleServer.ServerSocketType.VerifyData;
#if DOTNET2
                IAsyncResult async = socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex + receiveCount, bufferSize - receiveCount, SocketFlags.None, out socketError, asyncCallback, socket);
                if (socketError == SocketError.Success)
                {
                    if (!async.CompletedSynchronously) Server.PushReceiveVerifyCommandTimeout(this, socket);
                    return true;
                }
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                asyncEventArgs.SetBuffer(Buffer.StartIndex + receiveCount, bufferSize - receiveCount);
                if (socket.ReceiveAsync(asyncEventArgs))
                {
                    Server.PushReceiveVerifyCommandTimeout(this, socket);
#if !DotNetStandard
                    Interlocked.Exchange(ref asyncLock, 0);
#endif
                    return true;
                }
#if !DotNetStandard
                Interlocked.Exchange(ref asyncLock, 0);
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
            if (!asyncEventArgs.CompletedSynchronously) Server.CancelReceiveVerifyCommandTimeout(this);
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
                Socket socket = new Net.UnionType { Value = asyncEventArgs.AsyncState }.Socket;
                if (socket == Socket)
                {
                    int count = socket.EndReceive(asyncEventArgs, out socketError);
                    if (socketError == SocketError.Success)
                    {
#else
                if (asyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = asyncEventArgs.BytesTransferred;
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
                socketError = asyncEventArgs.SocketError;
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
                SubArray<byte> data = new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + receiveIndex, Length = compressionDataSize };
                if (MarkData != 0) TcpServer.CommandBuffer.Mark(ref data, MarkData);
                return Server.DoCommand(Server.VerifyCommandIdentity, this, ref data);
            }
            if (MarkData != 0) TcpServer.CommandBuffer.Mark(Buffer.Buffer, MarkData, Buffer.StartIndex + receiveIndex, compressionDataSize);
            SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
            AutoCSer.IO.Compression.DeflateDeCompressor.Get(Buffer.Buffer, Buffer.StartIndex + receiveIndex, compressionDataSize, ref buffer);
            return buffer.Buffer != null && Server.DoCommand(Server.VerifyCommandIdentity, this, ref buffer, dataSize);
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
                SocketType = TcpSimpleServer.ServerSocketType.Command;
#if DOTNET2
                socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex, bufferSize, SocketFlags.None, out socketError, asyncCallback, socket);
                if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                asyncEventArgs.SetBuffer(Buffer.StartIndex, bufferSize);
                if (socket.ReceiveAsync(asyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref asyncLock, 0);
#endif
                    return true;
                }
#if !DotNetStandard
                Interlocked.Exchange(ref asyncLock, 0);
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
            Socket socket = new Net.UnionType { Value = asyncEventArgs.AsyncState }.Socket;
            if (socket == Socket)
            {
                receiveCount = socket.EndReceive(asyncEventArgs, out socketError);
                if (socketError == SocketError.Success)
                {
#else
            if (asyncEventArgs.SocketError == SocketError.Success)
            {
                receiveCount = asyncEventArgs.BytesTransferred;
#endif
                if (receiveCount >= sizeof(int))
                {
                    fixed (byte* receiveDataFixed = Buffer.Buffer)
                    {
                        command = *(int*)(bufferStart = receiveDataFixed + Buffer.StartIndex);
                        int commandIndex = base.commandIndex;
                        if (Server.IsCommand(commandIndex) && IsCommand(commandIndex))
                        {
                            if (commandIndex != TcpServer.Server.CheckCommandIndex)
                            {
                                if (((uint)command & (uint)TcpServer.CommandFlags.NullData) == 0)
                                {
                                    if (receiveCount >= sizeof(int) * 2)
                                    {
                                        if ((compressionDataSize = *(int*)(bufferStart + sizeof(uint))) > 0)
                                        {
                                            dataSize = compressionDataSize;
                                            receiveIndex = sizeof(int) + sizeof(uint);
                                            return checkCommandData();
                                        }
                                        else if (compressionDataSize < 0 && receiveCount >= (sizeof(int) * 2 + sizeof(uint)))
                                        {
                                            if ((dataSize = *(int*)(bufferStart + (sizeof(uint) + sizeof(int)))) > (compressionDataSize = -compressionDataSize))
                                            {
                                                receiveIndex = sizeof(int) * 2 + sizeof(uint);
                                                return checkCommandData();
                                            }
                                        }
                                    }
                                }
                                else if (receiveCount == sizeof(int) && commandIndex != TcpServer.Server.RemoteExpressionCommandIndex && commandIndex != TcpServer.Server.RemoteExpressionNodeIdCommandIndex)
                                {
                                    return Server.DoCommand(commandIndex, this, ref SubArray<byte>.Null);
                                }
                            }
                            else if (receiveCount == sizeof(int)) return Send(TcpServer.ReturnType.Success);
                        }
                    }
                }
#if DOTNET2
                }
#endif
            }
#if !DOTNET2
            else socketError = asyncEventArgs.SocketError;
#endif
            return false;
        }
        /// <summary>
        /// 检查命令数据
        /// </summary>
        /// <returns></returns>
        private bool checkCommandData()
        {
            int receiveSize = receiveCount - receiveIndex, nextSize = compressionDataSize - receiveSize;
            if (nextSize == 0) return isDoCommand();
            if (nextSize > 0)
            {
                if (compressionDataSize > bufferSize)
                {
                    SubBuffer.Pool.GetBuffer(ref ReceiveBigBuffer, compressionDataSize);
                    if (ReceiveBigBuffer.Length > Server.SendBufferMaxSize)
                    {
                        if (ReceiveBigBuffer.PoolBuffer.Pool == null) ++Server.ReceiveNewBufferCount;
                        receiveBigBufferCount = receiveSize;
                        SocketType = TcpSimpleServer.ServerSocketType.BigData;
#if !DOTNET2
                        BIGRECEIVE:
#endif
                        Socket socket = Socket;
                        if (socket != null)
                        {
#if DOTNET2
                    socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, compressionDataSize - receiveBigBufferCount, SocketFlags.None, out socketError, asyncCallback, socket);
                    if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                            while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                            asyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, compressionDataSize - receiveBigBufferCount);
                            if (socket.ReceiveAsync(asyncEventArgs))
                            {
#if !DotNetStandard
                                Interlocked.Exchange(ref asyncLock, 0);
#endif
                                return true;
                            }
                            if (asyncEventArgs.SocketError == SocketError.Success)
                            {
#if !DotNetStandard
                                Interlocked.Exchange(ref asyncLock, 0);
#endif
                                if (compressionDataSize == (receiveBigBufferCount += asyncEventArgs.BytesTransferred)) return isDoCommandBig();
                                goto BIGRECEIVE;
                            }
                            socketError = asyncEventArgs.SocketError;
#endif
                        }
                        return false;
                    }
                    System.Buffer.BlockCopy(Buffer.Buffer, Buffer.StartIndex + receiveIndex, ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, receiveCount -= receiveIndex);
                    Buffer.Free();
                    ReceiveBigBuffer.CopyToClear(ref Buffer);
#if !DOTNET2
                    asyncEventArgs.SetBuffer(Buffer.Buffer, Buffer.StartIndex, bufferSize = Buffer.Length);
#endif
                    receiveIndex = 0;
                }
                {
                    if (receiveIndex + compressionDataSize > bufferSize)
                    {
                        Memory.CopyNotNull(bufferStart + receiveIndex, bufferStart, receiveCount = receiveSize);
                        receiveIndex = 0;
                    }
                    SocketType = TcpSimpleServer.ServerSocketType.Data;
#if !DOTNET2
                    RECEIVE:
#endif
                    Socket socket = Socket;
                    if (socket != null)
                    {
#if DOTNET2
                    socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex + receiveCount, nextSize, SocketFlags.None, out socketError, asyncCallback, socket);
                    if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                        while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                        asyncEventArgs.SetBuffer(Buffer.StartIndex + receiveCount, nextSize);
                        if (socket.ReceiveAsync(asyncEventArgs))
                        {
#if !DotNetStandard
                            Interlocked.Exchange(ref asyncLock, 0);
#endif
                            return true;
                        }
                        if (asyncEventArgs.SocketError == SocketError.Success)
                        {
#if !DotNetStandard
                            Interlocked.Exchange(ref asyncLock, 0);
#endif
                            receiveCount += (receiveSize = asyncEventArgs.BytesTransferred);
                            if ((nextSize -= receiveSize) == 0) return isDoCommand();
                            goto RECEIVE;
                        }
                        socketError = asyncEventArgs.SocketError;
#endif
                    }
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
                Socket socket = new Net.UnionType { Value = asyncEventArgs.AsyncState }.Socket;
                if (socket == Socket)
                {
                    receiveCount += socket.EndReceive(asyncEventArgs, out socketError);
                    if (socketError == SocketError.Success)
                    {
#else
                CHECK:
                if (asyncEventArgs.SocketError == SocketError.Success)
                {
                    receiveCount += asyncEventArgs.BytesTransferred;
#endif
                        int nextSize = compressionDataSize - (receiveCount - receiveIndex);
                        if (nextSize == 0)
                        {
                            if (isDoCommand()) return;
                        }
                        else
                        {
#if DOTNET2
                            if (socket == Socket)
                            {
                                socket.BeginReceive(Buffer.Buffer, Buffer.StartIndex + receiveCount, nextSize, SocketFlags.None, out socketError, asyncCallback, socket);
                                if (socketError == SocketError.Success) return;
                            }
#else
                        Socket socket = Socket;
                        if (socket != null)
                        {
#if !DotNetStandard
                            while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                            asyncEventArgs.SetBuffer(Buffer.StartIndex + receiveCount, nextSize);
                            if (socket.ReceiveAsync(asyncEventArgs))
                            {
#if !DotNetStandard
                                Interlocked.Exchange(ref asyncLock, 0);
#endif
                                return;
                            }
#if !DotNetStandard
                            Interlocked.Exchange(ref asyncLock, 0);
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
                else socketError = asyncEventArgs.SocketError;
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
                Socket socket = new Net.UnionType { Value = asyncEventArgs.AsyncState }.Socket;
                if (socket == Socket)
                {
                    receiveBigBufferCount += socket.EndReceive(asyncEventArgs, out socketError);
                    if (socketError == SocketError.Success)
                    {
#else
                CHECK:
                if (asyncEventArgs.SocketError == SocketError.Success)
                {
                    receiveBigBufferCount += asyncEventArgs.BytesTransferred;
#endif
                        int nextSize = compressionDataSize - receiveBigBufferCount;
                        if (nextSize == 0)
                        {
                            if (isDoCommandBig()) return;
                        }
                        else
                        {
#if DOTNET2
                            if (socket == Socket)
                            {
                                socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize, SocketFlags.None, out socketError, asyncCallback, socket);
                                if (socketError == SocketError.Success) return;
                            }
#else
                        Socket socket = Socket;
                        if (socket != null)
                        {
#if !DotNetStandard
                            while (Interlocked.CompareExchange(ref asyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                            asyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize);
                            if (socket.ReceiveAsync(asyncEventArgs))
                            {
#if !DotNetStandard
                                Interlocked.Exchange(ref asyncLock, 0);
#endif
                                return;
                            }
#if !DotNetStandard
                            Interlocked.Exchange(ref asyncLock, 0);
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
                else socketError = asyncEventArgs.SocketError;
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
        private bool isDoCommand()
        {
            if (compressionDataSize == dataSize)
            {
                SubArray<byte> data = new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + receiveIndex, Length = compressionDataSize };
                return doCommandMark(ref data);
            }
            if (MarkData != 0) TcpServer.CommandBuffer.Mark(Buffer.Buffer, MarkData, Buffer.StartIndex + receiveIndex, compressionDataSize);
            SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
            AutoCSer.IO.Compression.DeflateDeCompressor.Get(Buffer.Buffer, Buffer.StartIndex + receiveIndex, compressionDataSize, ref buffer);
            return buffer.Buffer != null && doCommand(ref buffer);
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <returns></returns>
        private bool isDoCommandBig()
        {
#if !DOTNET2
            asyncEventArgs.SetBuffer(Buffer.Buffer, Buffer.StartIndex, bufferSize);
#endif
            SubBuffer.PoolBufferFull receiveBigBuffer = ReceiveBigBuffer;
            ReceiveBigBuffer.Clear();
            try
            {
                System.Buffer.BlockCopy(Buffer.Buffer, Buffer.StartIndex + receiveIndex, receiveBigBuffer.Buffer, receiveBigBuffer.StartIndex, receiveCount - receiveIndex);
                if (compressionDataSize == dataSize)
                {
                    SubArray<byte> data = new SubArray<byte> { Array = receiveBigBuffer.Buffer, Start = receiveBigBuffer.StartIndex, Length = dataSize };
                    return doCommandMark(ref data);
                }
                if (MarkData != 0) TcpServer.CommandBuffer.Mark(receiveBigBuffer.Buffer, MarkData, receiveBigBuffer.StartIndex, compressionDataSize);
                SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
                AutoCSer.IO.Compression.DeflateDeCompressor.Get(receiveBigBuffer.Buffer, receiveBigBuffer.StartIndex, compressionDataSize, ref buffer);
                if (buffer.Buffer == null) return false;
                if (buffer.PoolBuffer.Pool == null) ++Server.ReceiveNewBufferCount;
                return doCommand(ref buffer);
            }
            finally { receiveBigBuffer.Free(); }
        }
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool doCommandMark(ref SubArray<byte> data)
        {
            if (MarkData != 0) TcpServer.CommandBuffer.Mark(ref data, MarkData);
            switch (commandIndex - TcpServer.Server.RemoteExpressionNodeIdCommandIndex)
            {
                case TcpServer.Server.RemoteExpressionNodeIdCommandIndex - TcpServer.Server.RemoteExpressionNodeIdCommandIndex: return getRemoteExpressionNodeId(ref data);
                case TcpServer.Server.RemoteExpressionCommandIndex - TcpServer.Server.RemoteExpressionNodeIdCommandIndex: return getRemoteExpression(ref data);
                default: return Server.DoCommand(commandIndex, this, ref data);
            }
        }
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool doCommand(ref SubBuffer.PoolBufferFull buffer)
        {
            SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = dataSize };
            bool value;
            switch (commandIndex - TcpServer.Server.RemoteExpressionNodeIdCommandIndex)
            {
                case TcpServer.Server.RemoteExpressionNodeIdCommandIndex - TcpServer.Server.RemoteExpressionNodeIdCommandIndex: value = getRemoteExpressionNodeId(ref data); break;
                case TcpServer.Server.RemoteExpressionCommandIndex - TcpServer.Server.RemoteExpressionNodeIdCommandIndex: value = getRemoteExpression(ref data); break;
                default: value = Server.DoCommand(commandIndex, this, ref data); break;
            }
            buffer.PoolBuffer.Free();
            return value;
        }
        /// <summary>
        /// 获取远程表达式服务端节点标识
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool getRemoteExpressionNodeId(ref SubArray<byte> data)
        {
            AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
            try
            {
                RemoteExpression.ServerNodeIdChecker.Input inputParameter = default(RemoteExpression.ServerNodeIdChecker.Input);
                if (DeSerialize(ref data, ref inputParameter, false))
                {
                    RemoteExpression.ServerNodeIdChecker.Output outputParameter = new RemoteExpression.ServerNodeIdChecker.Output { Return = RemoteExpression.Node.Get(inputParameter.Types) };
                    return Send(TcpSimpleServer.OutputInfo.RemoteExpressionNodeId, ref outputParameter);
                }
                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
            }
            catch (Exception error)
            {
                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                Log(error);
            }
            return SendOutput(returnType);
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool getRemoteExpression(ref SubArray<byte> data)
        {
            AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
            try
            {
                RemoteExpression.ClientNode inputParameter = default(RemoteExpression.ClientNode);
                if (DeSerialize(ref data, ref inputParameter, false))
                {
                    RemoteExpression.ReturnValue.Output outputParameter = new RemoteExpression.ReturnValue.Output { Return = inputParameter.GetReturnValue() };
                    return Send(TcpSimpleServer.OutputInfo.RemoteExpression, ref outputParameter);
                }
                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
            }
            catch (Exception error)
            {
                returnType = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                Log(error);
            }
            return SendOutput(returnType);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        /// <returns>是否发送成功</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool SendAsync(TcpServer.ReturnType returnType)
        {
            bool isSend = false;
            try
            {
                Socket socket = Socket;
                if (socket != null)
                {
                    Buffer.Buffer[Buffer.StartIndex] = (byte)returnType;
                    if (socket.Send(Buffer.Buffer, Buffer.StartIndex, sizeof(int), SocketFlags.None, out socketError) == sizeof(int))
                    {
                        if (isReceiveCommand()) return true;
                        isSend = true;
                    }
                }
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            close();
            return isSend;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Send(TcpServer.ReturnType returnType)
        {
            Socket socket = Socket;
            if (socket != null)
            {
                Buffer.Buffer[Buffer.StartIndex] = (byte)returnType;
                return socket.Send(Buffer.Buffer, Buffer.StartIndex, sizeof(int), SocketFlags.None, out socketError) == sizeof(int) && isReceiveCommand();
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Send()
        {
            return Send(TcpServer.ReturnType.Success);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="outputParameterType">输出数据类型</typeparam>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出数据</param>
        /// <returns>是否发送成功</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool SendAsync<outputParameterType>(TcpSimpleServer.OutputInfo outputInfo, ref TcpServer.ReturnValue<outputParameterType> outputParameter)
            where outputParameterType : struct
        {
            bool isSend = false;
            try
            {
                if (outputParameter.Type == TcpServer.ReturnType.Success)
                {
                    if (send(outputInfo, ref outputParameter.Value, ref isSend)) return isSend;
                }
                else
                {
                    Socket socket = Socket;
                    if (socket != null)
                    {
                        fixed (byte* bufferFixed = Buffer.Buffer)
                        {
                            byte* start = bufferFixed + Buffer.StartIndex;
                            *(int*)start = 0;
                            *(start + sizeof(int)) = (byte)outputParameter.Type;
                        }
                        if (socket.Send(Buffer.Buffer, Buffer.StartIndex, sizeof(int) * 2, SocketFlags.None, out socketError) == sizeof(int) * 2)
                        {
                            if (IsVerifyMethod ? isReceiveCommand() : (--verifyMethodCount != 0 && isReceiveVerifyCommand())) return true;
                            isSend = true;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Server.Log.Add(AutoCSer.Log.LogType.Debug, error);
            }
            close();
            return isSend;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool SendOutput(TcpServer.ReturnType returnType)
        {
            Socket socket = Socket;
            if (socket != null)
            {
                fixed (byte* bufferFixed = Buffer.Buffer)
                {
                    byte* start = bufferFixed + Buffer.StartIndex;
                    *(int*)start = 0;
                    *(start + sizeof(int)) = (byte)returnType;
                }
                if (socket.Send(Buffer.Buffer, Buffer.StartIndex, sizeof(int) * 2, SocketFlags.None, out socketError) == sizeof(int) * 2)
                {
                    return IsVerifyMethod ? isReceiveCommand() : (--verifyMethodCount != 0 && isReceiveVerifyCommand());
                }
            }
            return false;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="outputParameterType">输出数据类型</typeparam>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public bool Send<outputParameterType>(TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
            where outputParameterType : struct
        {
            bool isSend = false;
            return send(outputInfo, ref outputParameter, ref isSend);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="outputParameterType">输出数据类型</typeparam>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出数据</param>
        /// <param name="isSend">是否发送成功</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool send<outputParameterType>(TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter, ref bool isSend)
            where outputParameterType : struct
        {
            Socket socket = Socket;
            if (socket != null)
            {
                try
                {
                    BuildOutput(outputInfo, ref outputParameter);
                    isSend = OutputBuffer.Send(Socket);
                }
                finally { OutputBuffer.Free(); }
                return isSend && (IsVerifyMethod ? isReceiveCommand() : (--verifyMethodCount != 0 && isReceiveVerifyCommand()));
            }
            return false;
        }

        /// <summary>
        /// 异步回调
        /// </summary>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Func<TcpServer.ReturnValue, bool> GetCallback()
        {
            return (AutoCSer.Threading.RingPool<ServerCallback>.Default.Pop() ?? new ServerCallback()).Set(this);
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Func<TcpServer.ReturnValue<returnType>, bool> GetCallback<outputParameterType, returnType>(TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
#if NOJIT
            where outputParameterType : struct, IReturnParameter
#else
            where outputParameterType : struct, IReturnParameter<returnType>
#endif
        {
            return (AutoCSer.Threading.RingPool<ServerCallback<outputParameterType, returnType>>.Default.Pop() ?? new ServerCallback<outputParameterType, returnType>()).Set(this, outputInfo, ref outputParameter);
        }
        /// <summary>
        /// 验证函数异步回调
        /// </summary>
        /// <param name="outputInfo">服务端输出信息</param>
        /// <param name="outputParameter">输出参数</param>
        /// <returns>验证函数异步回调</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Func<TcpServer.ReturnValue<bool>, bool> GetCallback<outputParameterType>(TcpSimpleServer.OutputInfo outputInfo, ref outputParameterType outputParameter)
#if NOJIT
            where outputParameterType : struct, IReturnParameter
#else
            where outputParameterType : struct, IReturnParameter<bool>
#endif
        {
            return (AutoCSer.Threading.RingPool<ServerCallback<outputParameterType>>.Default.Pop() ?? new ServerCallback<outputParameterType>()).Set(this, outputInfo, ref outputParameter);
        }
    }
}
