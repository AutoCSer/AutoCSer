using System;
using System.Net;
using AutoCSer.Extension;
using System.Threading;
using System.Net.Sockets;
using AutoCSer.Log;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端套接字
    /// </summary>
    public abstract unsafe class ClientSocket : ClientSocketBase
    {
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        internal new ClientSocketSender Sender;
        ///// <summary>
        ///// 命令索引池
        ///// </summary>
        //internal AutoCSer.Threading.IndexValuePool<CommandPoolIndex> CommandPool;
        /// <summary>
        /// 命令索引池
        /// </summary>
        internal CommandPool CommandPool;
        ///// <summary>
        ///// 保持回调命令
        ///// </summary>
        //private ClientCommand.Command keepCallbackCommand;
        /// <summary>
        /// 当前处理会话标识
        /// </summary>
        internal uint CommandIndex;
        ///// <summary>
        ///// 保持回调命令会话标识
        ///// </summary>
        //private uint keepCallbackCommandIndex = uint.MaxValue;
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="log"></param>
        /// <param name="maxInputSize"></param>
        internal ClientSocket(IPAddress ipAddress, int port, ILog log, int maxInputSize) : base(ipAddress, port, log, maxInputSize) { }
        /// <summary>
        /// 设置 TCP 服务客户端套接字数据发送
        /// </summary>
        /// <param name="sender"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetSender(ClientSocketSender sender)
        {
            base.Sender = Sender = sender;
        }
        ///// <summary>
        ///// 获取回调客户端命令
        ///// </summary>
        ///// <returns>回调客户端命令</returns>
        //private ClientCommand.Command getCommand()
        //{
        //    //int index = (int)(CommandIndex & Server.CommandIndexAnd);
        //    int isKeepCallback = 0;
        //    object arrayLock = CommandPool.ArrayLock;
        //    Monitor.Enter(arrayLock);
        //    ClientCommand.Command command = CommandPool.Array[(int)CommandIndex].Get(ref isKeepCallback);
        //    if (command != null)
        //    {
        //        if (isKeepCallback == 0)
        //        {
        //            CommandPool.FreeExit((int)CommandIndex);
        //        }
        //        else
        //        {
        //            Monitor.Exit(arrayLock);
        //            keepCallbackCommand = command;
        //            keepCallbackCommandIndex = CommandIndex;
        //        }
        //        return command;
        //    }
        //    Monitor.Exit(arrayLock);
        //    return null;
        //}
        /// <summary>
        /// 接收数据处理
        /// </summary>
        /// <param name="type"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onReceive(ReturnType type)
        {
            ClientCommand.Command command = CommandPool.GetCommand((int)CommandIndex);
            if (command != null)
            {
                SubArray<byte> data = new SubArray<byte> { Start = (int)(byte)type };
                command.OnReceive(ref data);
            }
        }
        /// <summary>
        /// 接收数据处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onReceive()
        {
            ClientCommand.Command command = CommandPool.GetCommand((int)CommandIndex);
            if (command != null)
            {
                SubArray<byte> data = new SubArray<byte> { Array = ReceiveBuffer.Buffer, Start = ReceiveBuffer.StartIndex + receiveIndex, Length = dataSize };
                command.OnReceive(ref data);
            }
        }
        /// <summary>
        /// 接收数据处理
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnReceive(ref SubBuffer.PoolBufferFull buffer)
        {
            ClientCommand.Command command = CommandPool.GetCommand((int)CommandIndex);
            if (command == null) buffer.Free();
            else
            {
                SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = dataSize };
                try
                {
                    command.OnReceive(ref data);
                }
                finally { buffer.Free(); }
            }
        }
        ///// <summary>
        ///// 释放保持回调
        ///// </summary>
        ///// <param name="command">TCP 客户端命令</param>
        //internal void FreeKeep(ClientCommand.Command command)
        //{
        //    int index = (int)(command.CommandIndex & Server.CommandIndexAnd);
        //    object arrayLock = CommandPool.ArrayLock;
        //    Monitor.Enter(arrayLock);
        //    if (CommandPool.Array[index].IsCancel(command) == 0)
        //    {
        //        CommandPool.FreeExit(index);
        //    }
        //    else
        //    {
        //        Monitor.Exit(arrayLock);
        //    }
        //}
        ///// <summary>
        ///// 通知服务端取消保持回调
        ///// </summary>
        ///// <param name="command">TCP 客户端命令</param>
        //internal void CancelKeep(ClientCommand.Command command)
        //{
        //    ClientSocketSender sender = Sender;
        //    if (sender == null)
        //    {
        //        int index = (int)(command.CommandIndex & Server.CommandIndexAnd);
        //        object arrayLock = CommandPool.ArrayLock;
        //        Monitor.Enter(arrayLock);
        //        CommandPool.Array[index].Cancel(command);
        //        Monitor.Exit(arrayLock);
        //    }
        //    else if (sender.IsSocket)
        //    {
        //        int index = (int)(command.CommandIndex & Server.CommandIndexAnd);
        //        object arrayLock = CommandPool.ArrayLock;
        //        Monitor.Enter(arrayLock);
        //        if (CommandPool.Array[index].Command == command)
        //        {
        //            Monitor.Exit(arrayLock);
        //            CancelKeep(index);
        //        }
        //        else
        //        {
        //            Monitor.Exit(arrayLock);
        //        }
        //    }
        //}
        /// <summary>
        /// 通知服务端取消保持回调
        /// </summary>
        /// <param name="command">TCP 客户端命令</param>
        /// <param name="commandIndex">命令会话标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CancelKeep(ClientCommand.Command command, int commandIndex)
        {
            ClientSocketSender sender = Sender;
            if (sender == null) CommandPool.Cancel(commandIndex, command);
            else if (sender.IsSocket && CommandPool[commandIndex] == command) CancelKeep(commandIndex);
        }
        /// <summary>
        /// 通知服务端取消保持回调
        /// </summary>
        /// <param name="commandIndex">会话标识</param>
        internal abstract void CancelKeep(int commandIndex);
        ///// <summary>
        ///// 释放命令信息集合索引
        ///// </summary>
        ///// <param name="index">命令信息集合索引</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void FreeIndex(int index)
        //{
        //    Monitor.Enter(CommandPool.ArrayLock);
        //    CommandPool.Array[index].Command = null;
        //    CommandPool.FreeExit(index);
        //}
        ///// <summary>
        ///// 获取命令信息集合索引
        ///// </summary>
        ///// <param name="command">客户端命令</param>
        //internal bool NewIndex(ClientCommand.Command command)
        //{
        //    object arrayLock = CommandPool.ArrayLock;
        //    Monitor.Enter(arrayLock);
        //    if (isClose)
        //    {
        //        Monitor.Exit(arrayLock);
        //    }
        //    else
        //    {
        //        int index;
        //        try
        //        {
        //            index = CommandPool.GetIndexContinue();//不能写成一行，可能造成Pool先入栈然后被修改，导致索引溢出
        //            CommandPool.Array[index].Command = command;
        //        }
        //        finally
        //        {
        //            Monitor.Exit(arrayLock);
        //        }
        //        if (index <= Server.CommandIndexAnd)
        //        {
        //            command.CommandIndex = (uint)index;
        //            return true;
        //        }
        //        Log.add(AutoCSer.Log.LogType.Error, "活动会话数量过多");
        //    }
        //    return false;
        //}
        /// <summary>
        /// 合并命令处理错误
        /// </summary>
        protected abstract void mergeError();
        /// <summary>
        /// 合并命令处理
        /// </summary>
        /// <param name="data"></param>
        internal void Merge(ref SubArray<byte> data)
        {
            try
            {
                byte[] dataArray = data.Array;
                ClientCommand.Command command;
                int receiveIndex = data.Start, receiveCount = data.EndIndex;
                ReturnType type;
                fixed (byte* dataFixed = data.Array)
                {
                    byte* start;
                    do
                    {
                        int receiveSize = receiveCount - receiveIndex;
                        if (receiveSize < sizeof(uint))
                        {
                            if (receiveSize == 0)
                            {
                                return;
                            }
                            break;
                        }
                        CommandIndex = *(uint*)(start = dataFixed + receiveIndex);
                        if ((type = Server.GetReturnType(ref CommandIndex)) == ReturnType.Unknown)
                        {
                            if (receiveSize < (sizeof(uint) + sizeof(int))) break;
                            if ((dataSize = *(int*)(start + sizeof(uint))) <= 0) break;
                            if (dataSize > (receiveSize -= (sizeof(uint) + sizeof(int)))) break;
                            receiveIndex += (sizeof(uint) + sizeof(int));

                            //if ((command = CommandIndex == keepCallbackCommandIndex ? keepCallbackCommand : getCommand()) != null)
                            if ((command = CommandPool.GetCommand((int)CommandIndex)) != null)
                            {
                                //if (command.CommandInfo.TaskType == ClientTaskType.Synchronous)
                                //{
                                //    data.Set(receiveIndex, dataSize);
                                //    command.OnReceiveSynchronous(ref data);
                                //}
                                //else if (command.CommandInfo.IsKeepCallback == 0) command.CopyDataRunTask(dataArray, receiveIndex, dataSize);
                                //else new CommandKeepDataTask().CopyData(command, dataArray, receiveIndex, dataSize);
                                data.Set(receiveIndex, dataSize);
                                command.OnReceive(ref data);
                            }
                            receiveIndex += dataSize;
                        }
                        else
                        {
                            onReceive(type);
                            receiveIndex += sizeof(uint);
                        }
                    }
                    while (true);
                }
            }
            catch (Exception error)
            {
                Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            mergeError();
        }
        /// <summary>
        /// 服务端自定义数据处理
        /// </summary>
        /// <param name="data"></param>
        internal abstract void CustomData(ref SubArray<byte> data);
    }
    /// <summary>
    /// TCP 服务客户端套接字
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    public unsafe abstract class ClientSocket<attributeType> : ClientSocket
        where attributeType : ServerAttribute
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        internal readonly ClientSocketCreator<attributeType> ClientCreator;
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        /// <param name="clientCreator">TCP 服务客户端创建器</param>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="createVersion"></param>
        /// <param name="maxInputSize">最大输入数据字节数</param>
        internal ClientSocket(ClientSocketCreator<attributeType> clientCreator, IPAddress ipAddress, int port, int createVersion, int maxInputSize)
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
        /// 合并命令处理错误
        /// </summary>
        protected override void mergeError()
        {
            DisposeSocket();
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
            //Thread.Sleep(0);
            //CommandClient.SocketWait.Reset();
            if (Socket != null)
            {
                //try
                //{
                //    Socket.Shutdown(SocketShutdown.Both);
                //}
                //catch { AutoCSer.Log.CatchCount.Add(AutoCSer.Log.CatchCount.Type.TcpClientSocket_Dispose); }
                //finally { Socket.Dispose(); }
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
        /// 验证函数调用
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected bool verifyMethod()
        {
            if (CommandPool == null)
            {
                CommandPool = new CommandPool(ClientCreator.Attribute.GetCommandPoolBitSize, ClientCommand.KeepCommand.CommandPoolIndex, ClientCreator.CommandClient.Log);
                CommandPool.Array[ClientCommand.KeepCommand.MergeIndex].Command = new UnionType { Value = Sender.Outputs.Head }.ClientCommand;
                CommandPool.Array[ClientCommand.KeepCommand.CustomDataIndex].Command = new ClientCommand.CustomDataCommand(this, ClientCommand.KeepCommand.KeepCallbackCommandInfo);
            }
            //if (CommandPool.Array == null)
            //{
            //    CommandPool.Reset(1 << CommandClient.Attribute.GetCommandPoolBitSize);
            //    object arrayLock = CommandPool.ArrayLock;
            //    Monitor.Enter(arrayLock);
            //    try
            //    {
            //        CommandPool.PoolIndex = ClientCommand.KeepCommand.CommandPoolIndex;
            //        CommandPool.Array[ClientCommand.KeepCommand.MergeIndex].Command = Sender.Outputs.Head;
            //        CommandPool.Array[ClientCommand.KeepCommand.CustomDataIndex].Command = new ClientCommand.CustomDataCommand(this, ClientCommand.KeepCommand.KeepCallbackCommandInfo);
            //    }
            //    finally { Monitor.Exit(arrayLock); }
            //}
            return verifyMethod(ClientCreator.CommandClient);
        }
        /// <summary>
        /// 获取命令回调序号
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected bool commandIdentityAsync(int count)
        {
            int receiveSize = (receiveCount += count) - receiveIndex;
            if (receiveSize >= sizeof(uint))
            {
                fixed (byte* receiveDataFixed = ReceiveBuffer.Buffer)
                {
                    receiveDataStart = receiveDataFixed + ReceiveBuffer.StartIndex;
                    byte* start = receiveDataStart + receiveIndex;
                    CommandIndex = *(uint*)start;
                    ReturnType type = Server.GetReturnType(ref CommandIndex);
                    if (type == ReturnType.Unknown)
                    {
                        if (receiveSize >= (sizeof(uint) + sizeof(int)))
                        {
                            if ((compressionDataSize = *(int*)(start + sizeof(uint))) < 0)
                            {
                                if (receiveSize < (sizeof(uint) + sizeof(int) * 2)) return false;
                                if ((compressionDataSize = -compressionDataSize) >= (dataSize = *(int*)(start + (sizeof(uint) + sizeof(int))))) return false;
                                receiveIndex += (sizeof(uint) + sizeof(int) * 2);
                            }
                            else if (compressionDataSize == 0) return false;
                            else
                            {
                                dataSize = compressionDataSize;
                                receiveIndex += (sizeof(uint) + sizeof(int));
                            }
                            if (compressionDataSize <= receiveCount - receiveIndex) return isOnDataLoopFixed() && loop();
                            bool isOnData = false;
                            if (checkDataLoopFixed(ref isOnData))
                            {
                                if (isOnData) return loop();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        onReceive(type);
                        receiveIndex += sizeof(uint);
                        return loop();
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
                ReceiveType = ClientSocketReceiveType.Data;
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
                            if (compressionDataSize <= (receiveCount += count) - receiveIndex) return isOnData = isOnDataLoopFixed();
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
//#if !DOTNET2
//                receiveAsyncEventArgs.SetBuffer(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex + receiveSize, compressionDataSize - receiveSize);
//#endif
                ReceiveType = ClientSocketReceiveType.BigData;
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
                            if (compressionDataSize == (receiveBigBufferCount += count)) return isOnData = isOnBigDataLoopFixed();
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
        /// <returns></returns>
        private bool isOnDataLoopFixed()
        {
            if (compressionDataSize == dataSize)
            {
                if (ReceiveMarkData != 0) CommandBuffer.Mark(ReceiveBuffer.Buffer, ReceiveMarkData, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize);
                onReceive();
                receiveIndex += compressionDataSize;
                return true;
            }
            if (ReceiveMarkData != 0) CommandBuffer.Mark(ReceiveBuffer.Buffer, ReceiveMarkData, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize);
            SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
            AutoCSer.IO.Compression.DeflateDeCompressor.Get(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, compressionDataSize, ref buffer);
            if (buffer.Buffer != null)
            {
                OnReceive(ref buffer);
                receiveIndex += compressionDataSize;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 回调命令数据
        /// </summary>
        /// <returns></returns>
        private bool isOnBigDataLoopFixed()
        {
//#if !DOTNET2
//            receiveAsyncEventArgs.SetBuffer(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex, receiveBufferSize);
//#endif
            Buffer.BlockCopy(ReceiveBuffer.Buffer, ReceiveBuffer.StartIndex + receiveIndex, ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, receiveCount - receiveIndex);
            if (ReceiveMarkData != 0) CommandBuffer.Mark(ReceiveBigBuffer.Buffer, ReceiveMarkData, ReceiveBigBuffer.StartIndex, compressionDataSize);
            if (compressionDataSize == dataSize) OnReceive(ref ReceiveBigBuffer);
            else
            {
                SubBuffer.PoolBufferFull buffer = new SubBuffer.PoolBufferFull { StartIndex = dataSize };
                try
                {
                    AutoCSer.IO.Compression.DeflateDeCompressor.Get(ReceiveBigBuffer.Buffer, ReceiveBigBuffer.StartIndex, compressionDataSize, ref buffer);
                }
                finally { ReceiveBigBuffer.Free(); }
                if (buffer.Buffer == null) return false;
                if (buffer.PoolBuffer.Pool == null) ++ClientCreator.CommandClient.ReceiveNewBufferCount;
                OnReceive(ref buffer);
            }
            receiveIndex = receiveCount = 0;
            return true;
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
                    return isOnDataLoopFixed() && loop();
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
                if (isOnBigDataLoopFixed())
                {
                    Socket socket = this.Socket;
                    if (socket != null)
                    {
                        ReceiveType = ClientSocketReceiveType.CommandIdentity;
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
                            if ((count = receiveAsyncEventArgs.BytesTransferred) > 0)
                            {
#if !DotNetStandard
                                Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                                ++ReceiveCount;
                                return commandIdentityAsync(count);
                            }
                        }
                        else socketError = receiveAsyncEventArgs.SocketError;
#endif
                    }
                }
            }
            else
            {
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
            if (receiveSize < sizeof(uint))
            {
                if (receiveSize == 0)
                {
                    receiveCount = receiveIndex = 0;
                    goto RECEIVE;
                }
                goto COPY;
            }
#if !DOTNET2
            ONRECEIVE:
#endif
            byte* start = receiveDataStart + receiveIndex;
            CommandIndex = *(uint*)start;
            ReturnType type = Server.GetReturnType(ref CommandIndex);
            if (type == ReturnType.Unknown)
            {
                if (receiveSize >= (sizeof(uint) + sizeof(int)))
                {
                    if ((compressionDataSize = *(int*)(start + sizeof(uint))) < 0)
                    {
                        if (receiveSize < (sizeof(uint) + sizeof(int) * 2)) goto COPY;
                        dataSize = *(int*)(start + (sizeof(uint) + sizeof(int)));
                        receiveIndex += (sizeof(uint) + sizeof(int) * 2);
                        compressionDataSize = -compressionDataSize;
                    }
                    else
                    {
                        dataSize = compressionDataSize;
                        receiveIndex += (sizeof(uint) + sizeof(int));
                    }
                    if (compressionDataSize <= receiveCount - receiveIndex)
                    {
                        if (isOnDataLoopFixed()) goto START;
                        return false;
                    }
                    bool isOnData = false;
                    if (checkDataLoopFixed(ref isOnData))
                    {
                        if (isOnData) goto START;
                        return true;
                    }
                }
            }
            else
            {
                onReceive(type);
                receiveIndex += sizeof(uint);
                goto START;
            }
            COPY:
            Memory.SimpleCopyNotNull64(receiveDataStart + receiveIndex, receiveDataStart, receiveCount = receiveSize);
            receiveIndex = 0;
            RECEIVE:
            Socket socket = this.Socket;
            if (socket != null)
            {
                ReceiveType = ClientSocketReceiveType.CommandIdentity;
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
                    if ((receiveSize = (receiveCount += receiveAsyncEventArgs.BytesTransferred) - receiveIndex) >= sizeof(uint))
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
        }
        /// <summary>
        /// 服务端自定义数据处理
        /// </summary>
        /// <param name="data"></param>
        internal override void CustomData(ref SubArray<byte> data)
        {
            ClientCreator.CommandClient.CustomData(ref data);
        }
    }
}
