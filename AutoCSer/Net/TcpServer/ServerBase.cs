using System;
using System.Net.Sockets;
using AutoCSer.Log;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Net;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    /// <typeparam name="attributeType"></typeparam>
    public abstract unsafe class ServerBase<attributeType> : CommandBuffer<attributeType>
        where attributeType : ServerBaseAttribute
    {
        /// <summary>
        /// 同步验证接口
        /// </summary>
        protected readonly Func<System.Net.Sockets.Socket, bool> verify;

        /// <summary>
        /// 二进制反序列化配置参数
        /// </summary>
        internal readonly AutoCSer.BinarySerialize.DeSerializeConfig BinaryDeSerializeConfig;
        /// <summary>
        /// 服务 IP 地址
        /// </summary>
        internal readonly IPAddress IpAddress;
        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; internal set; }
        /// <summary>
        /// TCP 服务器端同步调用队列
        /// </summary>
        internal ServerCallCanDisposableQueue CallQueue;
        /// <summary>
        /// 添加任务队列（不允许添加重复的任务实例，否则可能造成严重后果）
        /// </summary>
        /// <param name="call">TCP 服务器端同步调用</param>
        public void AppendQueue(ServerCallBase call)
        {
            if (call != null)
            {
                if (CallQueue.CheckAdd(call)) return;
                throw new InvalidOperationException();
            }
        }
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
        internal int MaxCommand;
        /// <summary>
        /// 验证超时
        /// </summary>
        internal SocketTimeoutLink.TimerLink ReceiveVerifyCommandTimeout;
        /// <summary>
        /// TCP 套接字
        /// </summary>
        internal Socket Socket;
        /// <summary>
        /// 是否已启动服务
        /// </summary>
        protected int isStart;
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
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">获取客户端请求线程调用类型</param>
        /// <param name="log">日志接口</param>
        /// <param name="isCallQueue">是否提供独占的 TCP 服务器端同步调用队列</param>
        internal ServerBase(attributeType attribute, Func<System.Net.Sockets.Socket, bool> verify, ILog log, bool isCallQueue)
            : base(attribute, attribute.GetReceiveBufferSize, attribute.GetSendBufferSize, attribute.GetServerSendBufferMaxSize, log)
        {
            this.verify = verify;
            if (isCallQueue) CallQueue = new ServerCallCanDisposableQueue();
            Port = attribute.Port;
            IpAddress = HostPort.HostToIPAddress(attribute.Host, Log);
            int binaryDeSerializeMaxArraySize = attribute.GetBinaryDeSerializeMaxArraySize;
            BinaryDeSerializeConfig = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetBinaryDeSerializeConfig(binaryDeSerializeMaxArraySize <= 0 ? AutoCSer.BinarySerialize.DeSerializer.DefaultConfig.MaxArraySize : binaryDeSerializeMaxArraySize);
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposed, 1, 0) == 0 && isListen != 0)
            {
                isListen = 0;
                if (Log.IsAnyType(AutoCSer.Log.LogType.Info)) Log.Add(AutoCSer.Log.LogType.Info, "停止服务 " + ServerName + " " + IpAddress.ToString() + "[" + Attribute.Host + "]:" + Port.toString());
                AutoCSer.DomainUnload.Unloader.Remove(this, DomainUnload.Type.TcpCommandBaseDispose, false);
                StopListen();
                if (CallQueue != null)
                {
                    CallQueue.Dispose();
                    CallQueue = null;
                }
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
                if (Port == 0) Log.Add(AutoCSer.Log.LogType.Warn, GetType().FullName + "服务器端口为 0");
            }
            catch (Exception error)
            {
                Dispose();
                Log.Add(AutoCSer.Log.LogType.Error, error, GetType().FullName + "服务器端口 " + ServerName + " " + IpAddress.ToString() + "[" + Attribute.Host + "]:" + Port.toString() + " TCP连接失败)");
            }
            return isListen != 0;
        }
        /// <summary>
        /// 获取 TCP 内部注册服务客户端
        /// </summary>
        /// <typeparam name="serverAttributeType"></typeparam>
        /// <param name="attribute"></param>
        /// <param name="tcpRegisterName"></param>
        /// <param name="tcpRegisterClient"></param>
        /// <returns></returns>
        internal bool GetRegisterClient<serverAttributeType>(serverAttributeType attribute, string tcpRegisterName, ref TcpRegister.Client tcpRegisterClient)
            where serverAttributeType : ServerBaseAttribute, TcpRegister.IServerAttribute
        {
            if (tcpRegisterName != null)
            {
                tcpRegisterClient = TcpRegister.Client.Get(tcpRegisterName, Log);
                if (tcpRegisterClient == null)
                {
                    Log.Add(AutoCSer.Log.LogType.Error, "TCP 内部注册服务 " + tcpRegisterName + " 客户端获取失败");
                    return false;
                }
                if (attribute.ClientRegisterHost == null) attribute.ClientRegisterHost = attribute.Host;
                if (attribute.ClientRegisterPort == 0) attribute.ClientRegisterPort = attribute.Port;
                if (attribute.ClientRegisterPort == 0)
                {
                    if (tcpRegisterClient.GetPort(attribute)) Port = attribute.Port;
                    else
                    {
                        Log.Add(AutoCSer.Log.LogType.Error, "TCP 内部服务 " + attribute.ServerName + " 端口获取失败");
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 是否支持自定义数据包
        /// </summary>
        protected virtual bool isCustomData
        {
            get { return true; }
        }
        /// <summary>
        /// 是否支持保持回调
        /// </summary>
        protected virtual bool isKeepCallback
        {
            get { return true; }
        }
        /// <summary>
        /// 是否支持合并命令处理
        /// </summary>
        protected virtual bool isMergeCommand
        {
            get { return true; }
        }
        /// <summary>
        /// 初始化序号识别命令处理委托集合
        /// </summary>
        /// <param name="count">命令数量</param>
        protected internal void setCommandData(int count)
        {
            commandData = Unmanaged.GetSizeUnsafe64(((count + TcpServer.Server.CommandStartIndex + 63) >> 6) << 3, true);
            commands = new MemoryMap(commandData.Data);
            commands.Set(TcpServer.Server.CheckCommandIndex);
            if (isCustomData) commands.Set(TcpServer.Server.CustomDataCommandIndex);
            if (isKeepCallback) commands.Set(TcpServer.Server.CancelKeepCommandIndex);
            if (isMergeCommand) commands.Set(TcpServer.Server.MergeCommandIndex);
            if (Attribute.IsRemoteExpression)
            {
                commands.Set(TcpServer.Server.RemoteExpressionNodeIdCommandIndex);
                commands.Set(TcpServer.Server.RemoteExpressionCommandIndex);
            }
        }
        /// <summary>
        /// 设置命令索引信息
        /// </summary>
        /// <param name="methodIndex">命令处理索引</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected internal void setCommand(int methodIndex)
        {
            int command = methodIndex + TcpServer.Server.CommandStartIndex;
            if (command > MaxCommand) MaxCommand = command;
            commands.Set(command);
        }
        /// <summary>
        /// 创建客户端命令数据
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        internal bool CreateCommandData(ref Pointer.Size commandData, ref MemoryMap commands)
        {
            if (this.commandData.Data != null)
            {
                commandData = Unmanaged.GetSizeUnsafe64(this.commandData.ByteSize, true);
                commands = new MemoryMap(commandData.Data);
                commands.Set(TcpServer.Server.CheckCommandIndex);
                //if (isCustomData) commands.Set(TcpServer.Server.CustomDataCommandIndex);
                if (isKeepCallback) commands.Set(TcpServer.Server.CancelKeepCommandIndex);
                if (isMergeCommand) commands.Set(TcpServer.Server.MergeCommandIndex);
                //if (Attribute.IsRemoteExpression)
                //{
                //    commands.Set(TcpServer.Server.RemoteExpressionNodeIdCommandIndex);
                //    commands.Set(TcpServer.Server.RemoteExpressionCommandIndex);
                //}
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断命令是否有效
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsCommand(int index)
        {
            if ((uint)index <= (uint)MaxCommand)
            {
                if (commands.Get(index) != 0)
                {
                    if (commandData.Data == null) return false;
                    return true;
                }
                if (commandData.Data == null) return false;
            }
            if (Log.IsAllType(AutoCSer.Log.LogType.Info)) Log.Add(AutoCSer.Log.LogType.Info, Attribute.ServerName + " 缺少命令处理委托 [" + index.toString() + "]");
            return false;
        }
        /// <summary>
        /// 设置验证命令序号
        /// </summary>
        /// <param name="methodIndex"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected internal void setVerifyCommand(int methodIndex)
        {
            VerifyCommandIdentity = methodIndex + Server.CommandStartIndex;
        }
        /// <summary>
        /// 添加超时套接字
        /// </summary>
        /// <param name="serverSocket"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool PushReceiveVerifyCommandTimeout(SocketTimeoutLink serverSocket, Socket socket)
        {
            SocketTimeoutLink.TimerLink receiveVerifyCommandTimeout = this.ReceiveVerifyCommandTimeout;
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
        internal void CancelReceiveVerifyCommandTimeout(SocketTimeoutLink socket)
        {
            SocketTimeoutLink.TimerLink receiveVerifyCommandTimeout = this.ReceiveVerifyCommandTimeout;
            if (receiveVerifyCommandTimeout != null) receiveVerifyCommandTimeout.Cancel(socket);
        }
    }
}
