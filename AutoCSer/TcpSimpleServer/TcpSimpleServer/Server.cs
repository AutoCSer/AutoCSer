using System;
using System.Net.Sockets;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Net;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    public abstract unsafe class Server : TcpServer.CommandBase
    {
        /// <summary>
        /// 数据缓存区池
        /// </summary>
        internal readonly SubBuffer.Pool BufferPool;
        /// <summary>
        /// 同步验证接口
        /// </summary>
        protected readonly Func<System.Net.Sockets.Socket, bool> verify;

        /// <summary>
        /// TCP 服务器端同步调用队列
        /// </summary>
        internal TcpServer.ServerCallCanDisposableQueue CallQueue;
        /// <summary>
        /// 命令位图
        /// </summary>
        private Pointer.Size commandData;
        /// <summary>
        /// 命令位图
        /// </summary>
        private MemoryMap commands;
        /// <summary>
        /// 最大命令
        /// </summary>
        private int maxCommand;
        /// <summary>
        /// 验证超时
        /// </summary>
        internal ServerSocket.TimerLink ReceiveVerifyCommandTimeout;
        /// <summary>
        /// TCP 套接字
        /// </summary>
        internal Socket Socket;
        /// <summary>
        /// 是否已启动服务
        /// </summary>
        protected volatile int isStart;
        /// <summary>
        /// 验证命令序号
        /// </summary>
        internal int VerifyCommandIdentity;
        /// <summary>
        /// 是否处于监听状态
        /// </summary>
        protected byte isListen;
        /// <summary>
        /// 是否服务处于监听状态
        /// </summary>
        public bool IsListen
        {
            get { return isListen != 0; }
        }
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="host">监听主机名称</param>
        /// <param name="port">监听端口</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="bufferPool">数据缓存区池</param>
        /// <param name="sendBufferMaxSize">发送数据缓存区最大字节大小</param>
        /// <param name="minCompressSize">压缩启用最低字节数量</param>
        /// <param name="log">日志接口</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="isCallQueue">是否提供独占的 TCP 服务器端同步调用队列</param>
        internal Server(string host, int port, string serviceName, SubBuffer.Pool bufferPool, int sendBufferMaxSize, int minCompressSize, ILog log, Func<Socket, bool> verify, bool isCallQueue)
            : base(host, port, serviceName, sendBufferMaxSize, minCompressSize, log)
        {
            BufferPool = bufferPool;
            this.verify = verify;
            if (isCallQueue) CallQueue = new TcpServer.ServerCallCanDisposableQueue();
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposed, 1, 0) == 0 && isListen != 0)
            {
                isListen = 0;
                if (Log.isAnyType(AutoCSer.Log.LogType.Info)) Log.add(AutoCSer.Log.LogType.Info, "停止服务 " + ServerName + " " + IpAddress.ToString() + "[" + Host + "]:" + Port.toString());
                AutoCSer.DomainUnload.Unloader.Remove(this, DomainUnload.Type.TcpCommandBaseDispose, false);
                StopListen();
            }
        }
        /// <summary>
        /// 停止服务监听
        /// </summary>
        public virtual void StopListen()
        {
            if (Socket != null)
            {
#if DotNetStandard
                AutoCSer.Net.TcpServer.CommandBase.CloseServer(Socket);
#else
                Socket.Dispose();
#endif
                Socket = null;
            }
            Unmanaged.Free(ref commandData);
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected bool start()
        {
            return IsDisposed == 0 && Interlocked.CompareExchange(ref isStart, 1, 0) == 0 && listen();
        }
        /// <summary>
        /// 服务端监听
        /// </summary>
        /// <returns></returns>
        protected bool listen()
        {
            try
            {
                Socket = new Socket(IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.Bind(new IPEndPoint(IpAddress, Port));
                Socket.Listen(int.MaxValue);
                isListen = 1;
                if (Port == 0) Log.add(AutoCSer.Log.LogType.Warn, GetType().FullName + "服务器端口为 0");
            }
            catch (Exception error)
            {
                Dispose();
                Log.add(AutoCSer.Log.LogType.Error, error, GetType().FullName + "服务器端口 " + ServerName + " " + IpAddress.ToString() + "[" + Host + "]:" + Port.toString() + " TCP连接失败)");
            }
            return isListen != 0;
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns>是否成功</returns>
        public virtual bool Start()
        {
            if (start())
            {
                startGetSocket();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取客户端套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void startGetSocket()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(GetSocket);
            Thread.Sleep(0);
            AutoCSer.DomainUnload.Unloader.Add(this, DomainUnload.Type.TcpCommandBaseDispose);
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        internal abstract void GetSocket();
        /// <summary>
        /// 初始化序号识别命令处理委托集合
        /// </summary>
        /// <param name="count">命令数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected internal void setCommandData(int count)
        {
            commandData = Unmanaged.GetSizeUnsafe64(((count + TcpServer.Server.CommandStartIndex + 63) >> 6) << 3, true);
            commands = new MemoryMap(commandData.Data);
            commands.Set(TcpServer.Server.CheckCommandIndex);
            commands.Set(TcpServer.Server.CustomDataCommandIndex);
            commands.Set(TcpServer.Server.CancelKeepCommandIndex);
            commands.Set(TcpServer.Server.MergeCommandIndex);
        }
        /// <summary>
        /// 设置命令索引信息
        /// </summary>
        /// <param name="methodIndex">命令处理索引</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected internal void setCommand(int methodIndex)
        {
            int command = methodIndex + TcpServer.Server.CommandStartIndex;
            if (command > maxCommand) maxCommand = command;
            commands.Set(command);
        }
        /// <summary>
        /// 判断命令是否有效
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsCommand(int index)
        {
            if ((uint)index <= (uint)maxCommand)
            {
                if (commands.Get(index) != 0)
                {
                    if (commandData.Data == null) return false;
                    return true;
                }
                if (commandData.Data == null) return false;
            }
            if (Log.isAllType(AutoCSer.Log.LogType.Info)) Log.add(AutoCSer.Log.LogType.Info, ServerName + " 缺少命令处理委托 [" + index.toString() + "]");
            return false;
        }
        /// <summary>
        /// 设置验证命令序号
        /// </summary>
        /// <param name="methodIndex"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected internal void setVerifyCommand(int methodIndex)
        {
            VerifyCommandIdentity = methodIndex + TcpServer.Server.CommandStartIndex;
        }
        /// <summary>
        /// 添加超时套接字
        /// </summary>
        /// <param name="serverSocket"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool PushReceiveVerifyCommandTimeout(ServerSocket serverSocket, Socket socket)
        {
            ServerSocket.TimerLink receiveVerifyCommandTimeout = this.ReceiveVerifyCommandTimeout;
            if (receiveVerifyCommandTimeout != null)
            {
                receiveVerifyCommandTimeout.Push(serverSocket, socket);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取消超时套接字
        /// </summary>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CancelReceiveVerifyCommandTimeout(ServerSocket socket)
        {
            ServerSocket.TimerLink receiveVerifyCommandTimeout = this.ReceiveVerifyCommandTimeout;
            if (receiveVerifyCommandTimeout != null) receiveVerifyCommandTimeout.Cancel(socket);
        }
    }
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    /// <typeparam name="attributeType"></typeparam>
    public abstract unsafe class Server<attributeType> : Server
        where attributeType : ServerAttribute
    {
        /// <summary>
        /// TCP 服务调用配置
        /// </summary>
        internal readonly attributeType Attribute;
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="isCallQueue">是否提供独占的 TCP 服务器端同步调用队列</param>
        internal Server(attributeType attribute, ILog log, Func<System.Net.Sockets.Socket, bool> verify, bool isCallQueue)
            : base(attribute.Host, attribute.Port, attribute.ServerName, SubBuffer.Pool.GetPool(attribute.GetSendBufferSize), attribute.GetServerSendBufferMaxSize, attribute.MinCompressSize, log, verify, isCallQueue)
        {
            Attribute = attribute;
        }
    }
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    /// <typeparam name="attributeType"></typeparam>
    /// <typeparam name="serverSocketType"></typeparam>
    public abstract unsafe class Server<attributeType, serverSocketType> : Server<attributeType>
        where attributeType : ServerAttribute
        where serverSocketType : ServerSocket
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="log">日志接口</param>
        /// <param name="isCallQueue">是否提供独占的 TCP 服务器端同步调用队列</param>
        internal Server(attributeType attribute, ILog log, Func<System.Net.Sockets.Socket, bool> verify, bool isCallQueue)
            : base(attribute, log, verify, isCallQueue)
        {
        }
        /// <summary>
        /// 命令处理
        /// </summary>
        /// <param name="index">命令序号</param>
        /// <param name="socket">TCP 内部服务套接字数据发送</param>
        /// <param name="data">命令数据</param>
        /// <returns>是否成功</returns>
        public abstract bool DoCommand(int index, serverSocketType socket, ref SubArray<byte> data);
        /// <summary>
        /// 命令处理委托
        /// </summary>
        /// <param name="index"></param>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="dataSize"></param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool DoCommand(int index, serverSocketType socket, ref SubBuffer.PoolBufferFull buffer, int dataSize)
        {
            SubArray<byte> data = new SubArray<byte> { Array = buffer.Buffer, Start = buffer.StartIndex, Length = dataSize };
            bool value = DoCommand(index, socket, ref data);
            buffer.PoolBuffer.Free();
            return value;
        }
    }
}
