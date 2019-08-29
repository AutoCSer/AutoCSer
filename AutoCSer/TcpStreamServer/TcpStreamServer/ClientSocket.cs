using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStreamServer
{
    /// <summary>
    /// TCP 服务客户端套接字
    /// </summary>
    public abstract unsafe class ClientSocket : TcpServer.ClientSocketBase
    {
        /// <summary>
        /// TCP 客户端输出信息队列链表
        /// </summary>
        internal TcpServer.ClientCommand.CommandBase CommandQueue;
        ///// <summary>
        ///// TCP 服务客户端套接字数据发送
        ///// </summary>
        //internal new ClientSocketSender Sender;
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="log"></param>
        /// <param name="maxInputSize"></param>
        internal ClientSocket(IPAddress ipAddress, int port, ILog log, int maxInputSize) : base(ipAddress, port, log, maxInputSize)
        {
            ClientCommand.Command command = ClientCommand.CheckCommand.Get(this);
            Interlocked.Exchange(ref command.FreeLock, 1);
            CommandQueue = command;
        }
        /// <summary>
        /// 设置 TCP 服务客户端套接字数据发送
        /// </summary>
        /// <param name="sender"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetSender(ClientSocketSender sender)
        {
            base.Sender = Sender = sender;
        }
        /// <summary>
        /// 接收数据处理
        /// </summary>
        /// <param name="type"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onReceive(TcpServer.ReturnType type)
        {
            SubArray<byte> data = new SubArray<byte> { Start = (int)(byte)type };
            onReceive(ref data);
        }
        /// <summary>
        /// 接收数据处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onReceive()
        {
            SubArray<byte> data = new SubArray<byte> { Array = ReceiveBuffer.Buffer, Start = ReceiveBuffer.StartIndex + receiveIndex, Length = compressionDataSize };
            onReceive(ref data);
        }
        /// <summary>
        /// 接收数据处理
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onReceive(ref SubArray<byte> data)
        {
            getCurrentCommand().OnReceive(ref data);
            freeCommand();
        }
        /// <summary>
        /// 接收数据处理
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnReceive(ref SubBuffer.PoolBufferFull buffer)
        {
            SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = compressionDataSize };
            try
            {
                getCurrentCommand().OnReceive(ref data);
            }
            finally { buffer.Free(); }
            freeCommand();
        }
        /// <summary>
        /// 获取当前执行命令
        /// </summary>
        /// <returns></returns>
        private TcpServer.ClientCommand.CommandBase getCurrentCommand()
        {
            TcpServer.ClientCommand.CommandBase command = CommandQueue, nextCommand = command.LinkNext;
            if (new UnionType { Value = nextCommand }.ClientCommand.IsBuildError)
            {
                do
                {
                    new UnionType { Value = command }.ClientCommand.Free();
                    command = nextCommand;
                    nextCommand = nextCommand.LinkNext;
                }
                while (new UnionType { Value = nextCommand }.ClientCommand.IsBuildError);
                CommandQueue = command;
            }
            return nextCommand;
        }
        /// <summary>
        /// 释放 TCP 客户端输出信息
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void freeCommand()
        {
            TcpServer.ClientCommand.CommandBase command = CommandQueue.LinkNext;
            new UnionType { Value = CommandQueue }.ClientCommand.Free();
            TcpServer.ClientCommand.CommandBase nextCommand = command.LinkNext;
            while (nextCommand != null && new UnionType { Value = nextCommand }.ClientCommand.IsBuildError)
            {
                new UnionType { Value = command }.ClientCommand.Free();
                command = nextCommand;
                nextCommand = nextCommand.LinkNext;
            }
            CommandQueue = command;
        }
        /// <summary>
        /// 释放 TCP 客户端输出信息队列
        /// </summary>
        protected void freeCommandQueue()
        {
            TcpServer.ClientCommand.CommandBase head = CommandQueue;
            CommandQueue = null;
            if ((head = head.LinkNext) != null) TcpServer.ClientCommand.CommandBase.CancelLink(head);
        }
    }
    /// <summary>
    /// TCP 服务客户端套接字
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    public unsafe abstract class ClientSocket<attributeType> : ClientSocket
        where attributeType : ServerAttribute
    {
        /// <summary>
        /// TCP 服务客户端创建器
        /// </summary>
        internal readonly TcpServer.ClientSocketCreator<attributeType> ClientCreator;
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        /// <param name="clientCreator">TCP 服务客户端创建器</param>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="createVersion"></param>
        /// <param name="maxInputSize">最大输入数据字节数</param>
        internal ClientSocket(TcpServer.ClientSocketCreator<attributeType> clientCreator, IPAddress ipAddress, int port, int createVersion, int maxInputSize)
            : base(ipAddress, port, clientCreator.CommandClient.Log, maxInputSize)
        {
            ClientCreator = clientCreator;

            CreateVersion = createVersion;
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, Threading.Thread.CallType.TcpClientSocketBaseCreate);
        }
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        /// <param name="socket">TCP 内部服务客户端套接字</param>
        internal ClientSocket(ClientSocket<attributeType> socket)
            : base(socket.ipAddress, socket.port, socket.ClientCreator.CommandClient.Log, socket.MaxInputSize)
        {
            isSleep = true;
            this.ClientCreator = socket.ClientCreator;

            CreateVersion = socket.CreateVersion + 1;
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, Threading.Thread.CallType.TcpClientSocketBaseCreate);
        }
        /// <summary>
        /// 释放套接字
        /// </summary>
        internal override void DisposeSocket()
        {
            Socket socket = Socket;
            Socket = null;
            if (socket != null)
            {
                ClientCreator.OnDisposeSocket(this);
                AutoCSer.Net.TcpServer.CommandBuffer.CloseClientNotNull(socket);
            }
        }
        /// <summary>
        /// 套接字操作失败重新创建版本检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckCreateVersion()
        {
            return (ClientCreator.CommandClient.IsDisposed | (CreateVersion ^ ClientCreator.CreateVersion)) == 0
                && Interlocked.CompareExchange(ref ClientCreator.CreateVersion, CreateVersion + 1, CreateVersion) == CreateVersion;
        }
        /// <summary>
        /// 创建 TCP 服务客户端套接字
        /// </summary>
        internal abstract void CreateNew();
        /// <summary>
        /// 创建 TCP 服务客户端套接字失败休眠
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CreateSleep()
        {
            //ClientCreator.CommandClient.SocketWait.PulseReset();
            if (Socket != null)
            {
#if DotNetStandard
                    AutoCSer.Net.TcpServer.CommandBase.CloseClientNotNull(Socket);
#else
                Socket.Dispose();
#endif
                Socket = null;
            }
            Thread.Sleep(ClientCreator.CommandClient.TryCreateSleep);
        }
        /// <summary>
        /// 获取数据大小
        /// </summary>
        /// <returns></returns>
        private bool isReceiveDataSize()
        {
            Socket socket = Socket;
            if (socket != null)
            {
                receiveCount = receiveIndex = 0;
                ReceiveType = TcpServer.ClientSocketReceiveType.CommandIdentity;
#if DOTNET2
                socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = receiveAsyncEventArgs.BytesTransferred;
                    if (count > 0)
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        ++ReceiveCount;
                        return dataSizeAsync(count);
                    }
                }
                else socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            return false;
        }
        /// <summary>
        /// 获取数据大小
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected bool dataSizeAsync(int count)
        {
            int receiveSize = (receiveCount += count) - receiveIndex;
            if (receiveSize >= sizeof(int))
            {
                fixed (byte* receiveDataFixed = ReceiveBuffer.Buffer)
                {
                    receiveDataStart = receiveDataFixed + ReceiveBuffer.StartIndex;
                    byte* start = receiveDataStart + receiveIndex;
                    if ((compressionDataSize = *(int*)start) < 0)
                    {
                        if (receiveSize >= sizeof(int) * 2 && (dataSize = *(int*)(start + sizeof(int))) > (compressionDataSize = -compressionDataSize) && compressionDataSize != 0)
                        {
                            receiveIndex += sizeof(int) * 2;
                            receiveSize -= sizeof(int) * 2;
                            if (compressionDataSize <= receiveSize) return onCompressionData() && loop();
                            return receiveCompressionData();
                        }
                    }
                    else if (compressionDataSize < ServerOutput.OutputLink.OutputDataSize)
                    {
                        TcpServer.ReturnType type = (TcpServer.ReturnType)(byte)compressionDataSize;
                        onReceive(type);
                        receiveIndex += sizeof(int);
                        return loop();
                    }
                    else if ((compressionDataSize -= ServerOutput.OutputLink.OutputDataSize) > 0)
                    {
                        receiveIndex += sizeof(int);
                        if (compressionDataSize <= receiveCount - receiveIndex)
                        {
                            onDataLoopFixed();
                            return loop();
                        }
                        bool isOnData = false;
                        if (checkDataLoopFixed(ref isOnData))
                        {
                            if (isOnData) return loop();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 检查命令数据
        /// </summary>
        /// <param name="isOnData">是否接收完数据执行</param>
        /// <returns></returns>
        private bool checkDataLoopFixed(ref bool isOnData)
        {
            if (compressionDataSize <= receiveBufferSize)
            {
                if (receiveIndex + compressionDataSize > receiveBufferSize)
                {
                    Memory.CopyNotNull(receiveDataStart + receiveIndex, receiveDataStart, receiveCount -= receiveIndex);
                    receiveIndex = 0;
                }
                ReceiveType = TcpServer.ClientSocketReceiveType.Data;
#if !DOTNET2
                RECEIVE:
#endif
                Socket socket = this.Socket;
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
                        int count = receiveAsyncEventArgs.BytesTransferred;
                        if (count > 0)
                        {
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            ++ReceiveCount;
                            if (compressionDataSize <= (receiveCount += count) - receiveIndex)
                            {
                                onDataLoopFixed();
                                return isOnData = true;
                            }
                            goto RECEIVE;
                        }
                    }
                    else socketError = receiveAsyncEventArgs.SocketError;
#endif
                }
            }
            else
            {
                SubBuffer.Pool.GetBuffer(ref ReceiveBigBuffer, compressionDataSize);
                if (ReceiveBigBuffer.PoolBuffer.Pool == null) ++ClientCreator.CommandClient.ReceiveNewBufferCount;
                receiveBigBufferCount = receiveCount - receiveIndex;
                ReceiveType = TcpServer.ClientSocketReceiveType.BigData;
#if !DOTNET2
                RECEIVEBIG:
#endif
                Socket socket = this.Socket;
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
                        int count = receiveAsyncEventArgs.BytesTransferred;
                        if (count > 0)
                        {
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            ++ReceiveCount;
                            if (compressionDataSize == (receiveBigBufferCount += count))
                            {
                                onBigDataLoopFixed();
                                receiveIndex = receiveCount = 0;
#if !DOTNET2
                                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
#endif
                                return isOnData = true;
                            }
                            goto RECEIVEBIG;
                        }
                    }
                    else socketError = receiveAsyncEventArgs.SocketError;
#endif
                }
            }
            return false;
        }
        /// <summary>
        /// 回调命令数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onDataLoopFixed()
        {
            if (ReceiveMarkData != 0) TcpServer.CommandBuffer.Mark(ReceiveBuffer.Buffer, ReceiveMarkData, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize);
            onReceive();
            receiveIndex += compressionDataSize;
        }
        /// <summary>
        /// 回调命令数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onBigDataLoopFixed()
        {
//#if !DOTNET2
//            receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
//#endif
            Buffer.BlockCopy(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, receiveCount - receiveIndex);
            if (ReceiveMarkData != 0) TcpServer.CommandBuffer.Mark(ReceiveBigBuffer.Buffer, ReceiveMarkData, ReceiveBigBuffer.StartIndex, compressionDataSize);
            OnReceive(ref ReceiveBigBuffer);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected bool dataAsync(int count)
        {
#if !DOTNET2
            START:
#endif
            if (compressionDataSize <= (receiveCount += count) - receiveIndex)
            {
                fixed (byte* receiveDataFixed = ReceiveBuffer.Buffer)
                {
                    receiveDataStart = receiveDataFixed + ReceiveBuffer.StartIndex;
                    onDataLoopFixed();
                    return loop();
                }
            }
            Socket socket = this.Socket;
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
                    if ((count = receiveAsyncEventArgs.BytesTransferred) > 0)
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        ++ReceiveCount;
                        goto START;
                    }
                }
                else socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected bool bigDataAsync(int count)
        {
#if !DOTNET2
            START:
#endif
            int nextSize = compressionDataSize - (receiveBigBufferCount += count);
            if (nextSize == 0)
            {
                onBigDataLoopFixed();
                return isReceiveDataSize();
            }
            Socket socket = this.Socket;
            if (socket != null)
            {
#if DOTNET2
                    socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    if ((count = receiveAsyncEventArgs.BytesTransferred) > 0)
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        ++ReceiveCount;
                        goto START;
                    }
                }
                else socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            return false;
        }
        /// <summary>
        /// 循环处理命令回调
        /// </summary>
        /// <returns></returns>
        private bool loop()
        {
        START:
            int receiveSize = receiveCount - receiveIndex;
            if (receiveSize >= sizeof(int)) goto ONRECEIVE;
        COPY:
            *(ulong*)receiveDataStart = *(ulong*)(receiveDataStart + receiveIndex);
            receiveCount = receiveSize;
            receiveIndex = 0;
            Socket socket = this.Socket;
            if (socket != null)
            {
                ReceiveType = TcpServer.ClientSocketReceiveType.CommandIdentity;
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
                    if ((receiveCount += receiveAsyncEventArgs.BytesTransferred) - receiveIndex >= sizeof(int))
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        ++ReceiveCount;
                        goto ONRECEIVE;
                    }
                }
                else socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            return false;
        ONRECEIVE:
            byte* start = receiveDataStart + receiveIndex;
            if ((compressionDataSize = *(int*)start) < 0)
            {
                if (receiveSize >= sizeof(int) * 2)
                {
                    if ((dataSize = *(int*)(start + sizeof(int))) > (compressionDataSize = -compressionDataSize) && compressionDataSize != 0)
                    {
                        receiveIndex += sizeof(int) * 2;
                        receiveSize -= sizeof(int) * 2;
                        if (compressionDataSize <= receiveSize)
                        {
                            if (onCompressionData()) goto START;
                        }
                        else return receiveCompressionData();
                    }
                    return false;
                }
                goto COPY;
            }
            else if (compressionDataSize < ServerOutput.OutputLink.OutputDataSize)
            {
                TcpServer.ReturnType type = (TcpServer.ReturnType)(byte)compressionDataSize;
                onReceive(type);
                receiveIndex += sizeof(int);
                goto START;
            }
            else if ((compressionDataSize -= ServerOutput.OutputLink.OutputDataSize) > 0)
            {
                receiveIndex += sizeof(int);
                if (compressionDataSize <= receiveCount - receiveIndex)
                {
                    onDataLoopFixed();
                    goto START;
                }
                bool isOnData = false;
                if (checkDataLoopFixed(ref isOnData))
                {
                    if (isOnData) goto START;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查命令数据
        /// </summary>
        /// <returns></returns>
        private bool receiveCompressionData()
        {
            int nextSize = compressionDataSize - (receiveCount - receiveIndex);
            if (compressionDataSize <= receiveBufferSize)
            {
                if (receiveIndex + compressionDataSize > receiveBufferSize)
                {
                    Memory.CopyNotNull(receiveDataStart + receiveIndex, receiveDataStart, receiveCount -= receiveIndex);
                    receiveIndex = 0;
                }
                ReceiveType = TcpServer.ClientSocketReceiveType.CompressionData;
#if !DOTNET2
                RECEIVE:
#endif
                Socket socket = this.Socket;
                if (socket != null)
                {
#if DOTNET2
                    socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, nextSize);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        return true;
                    }
                    if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                    {
                        int count = receiveAsyncEventArgs.BytesTransferred;
                        if (count > 0)
                        {
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            receiveCount += count;
                            ++ReceiveCount;
                            if ((nextSize -= count) == 0) return onCompressionData() && isReceiveDataSize();
                            goto RECEIVE;
                        }
                    }
                    else socketError = receiveAsyncEventArgs.SocketError;
#endif
                }
            }
            else
            {
                SubBuffer.Pool.GetBuffer(ref ReceiveBigBuffer, compressionDataSize);
                if (ReceiveBigBuffer.PoolBuffer.Pool == null) ++ClientCreator.CommandClient.ReceiveNewBufferCount;
                receiveBigBufferCount = receiveCount - receiveIndex;
                ReceiveType = TcpServer.ClientSocketReceiveType.CompressionBigData;
#if !DOTNET2
                RECEIVEBIG:
#endif
                Socket socket = this.Socket;
                if (socket != null)
                {
#if DOTNET2
                    socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                    while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                    receiveAsyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize);
                    if (socket.ReceiveAsync(receiveAsyncEventArgs))
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        return true;
                    }
                    if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                    {
                        int count = receiveAsyncEventArgs.BytesTransferred;
                        if (count > 0)
                        {
#if !DotNetStandard
                            Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                            receiveBigBufferCount += count;
                            ++ReceiveCount;
                            if ((nextSize -= count) == 0) return onCompressionBigData() && isReceiveDataSize();
                            goto RECEIVEBIG;
                        }
                    }
                    else socketError = receiveAsyncEventArgs.SocketError;
#endif
                }
            }
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected bool compressionDataAsync(int count)
        {
#if !DOTNET2
            START:
#endif
            int nextSize = compressionDataSize - ((receiveCount += count) - receiveIndex);
            if (nextSize == 0) return onCompressionData() && isReceiveDataSize();
            Socket socket = this.Socket;
            if (socket != null)
            {
#if DOTNET2
                socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.StartIndex + receiveCount, nextSize);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    if ((count = receiveAsyncEventArgs.BytesTransferred) > 0)
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        ++ReceiveCount;
                        goto START;
                    }
                }
                else socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected bool compressionBigDataAsync(int count)
        {
#if !DOTNET2
            START:
#endif
            int nextSize = compressionDataSize - (receiveBigBufferCount += count);
            if (nextSize == 0) return onCompressionBigData() && isReceiveDataSize();
            Socket socket = this.Socket;
            if (socket != null)
            {
#if DOTNET2
                    socket.BeginReceive(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize, SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                    if (socketError == SocketError.Success) return true;
#else
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                receiveAsyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveBigBufferCount, nextSize);
                if (socket.ReceiveAsync(receiveAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
                if (receiveAsyncEventArgs.SocketError == SocketError.Success)
                {
                    if ((count = receiveAsyncEventArgs.BytesTransferred) > 0)
                    {
#if !DotNetStandard
                        Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                        ++ReceiveCount;
                        goto START;
                    }
                }
                else socketError = receiveAsyncEventArgs.SocketError;
#endif
            }
            return false;
        }
        /// <summary>
        /// 解压缩并回调命令数据
        /// </summary>
        /// <returns></returns>
        private bool onCompressionBigData()
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
            return onCompressionData(ref buffer);
        }
        /// <summary>
        /// 回调命令数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onCompressionData()
        {
            SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
            AutoCSer.IO.Compression.DeflateDeCompressor.Get(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize, ref buffer);
            return onCompressionData(ref buffer);
        }
        /// <summary>
        /// 回调命令数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private bool onCompressionData(ref SubBuffer.PoolBufferFull buffer)
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
                        if ((compressionDataSize = *(int*)start) >= 0)
                        {
                            if (compressionDataSize < ServerOutput.OutputLink.OutputDataSize)
                            {
                                TcpServer.ReturnType type = (TcpServer.ReturnType)(byte)compressionDataSize;
                                onReceive(type);
                                start += sizeof(int);
                            }
                            else if ((compressionDataSize -= ServerOutput.OutputLink.OutputDataSize) > 0 && (start += sizeof(int)) + compressionDataSize <= end)
                            {
                                data.Set((int)(start - dataFixed), compressionDataSize);
                                if (ReceiveMarkData != 0) TcpServer.CommandBuffer.Mark(data.Array, ReceiveMarkData, data.StartIndex, compressionDataSize);
                                onReceive(ref data);
                                start += compressionDataSize;
                            }
                            else
                            {
                                buffer.PoolBuffer.Free();
                                return false;
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
