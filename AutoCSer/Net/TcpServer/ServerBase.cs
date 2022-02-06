using System;
using System.Net.Sockets;
using AutoCSer.Log;
using System.Threading;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using System.Net;
using System.Diagnostics;
using AutoCSer.Memory;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务基类
    /// </summary>
    public abstract unsafe class ServerBase : CommandBuffer
    {
        /// <summary>
        /// TCP 服务配置副本
        /// </summary>
        internal ServerAttributeCache ServerAttribute;
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
        /// TCP 服务器端同步调用队列数组
        /// </summary>
        internal readonly KeyValue<ServerCallCanDisposableQueue, ServerCallCanDisposableQueue.LowPriorityLink>[] CallQueueArray;
        /// <summary>
        /// TCP 服务器端同步调用队列
        /// </summary>
        internal readonly ServerCallCanDisposableQueue CallQueue;
        /// <summary>
        /// TCP 服务器端同步调用队列（低优先级）
        /// </summary>
        internal readonly ServerCallCanDisposableQueue.LowPriorityLink CallQueueLink;
#if !NOJIT
        /// <summary>
        /// 扩展服务集合
        /// </summary>
        private readonly ExtendServerSet extendServerSet;
#endif
        /// <summary>
        /// 命令位图
        /// </summary>
        private AutoCSer.Memory.Pointer commandData;
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
        internal SocketTimeoutLink ReceiveVerifyCommandTimeout;
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
        /// 验证函数调用次数
        /// </summary>
        internal readonly byte VerifyMethodCount;
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
        /// <param name="extendCommandBits">扩展服务命令二进制位数</param>
        /// <param name="log">日志接口</param>
        /// <param name="callQueueCount">独占的 TCP 服务器端同步调用队列数量</param>
        /// <param name="isCallQueueLink">是否提供独占的 TCP 服务器端同步调用队列（低优先级）</param>
        /// <param name="isSynchronousVerifyMethod">验证函数是否同步调用</param>
        internal ServerBase(ServerBaseAttribute attribute, Func<System.Net.Sockets.Socket, bool> verify, byte extendCommandBits, ILog log, int callQueueCount, bool isCallQueueLink, bool isSynchronousVerifyMethod)
            : base(attribute, attribute.GetReceiveBufferSize, attribute.GetSendBufferSize, attribute.GetServerSendBufferMaxSize, log)
        {
#if !NOJIT
            if (extendCommandBits == 0) extendServerSet = ExtendServerSet.Null;
            else if (extendCommandBits < 9 || extendCommandBits > 16) throw new IndexOutOfRangeException("扩展服务命令二进制位数 " + extendCommandBits.toString() + " 超出范围 9-16");
            else extendServerSet = new ExtendServerSet(this, extendCommandBits);
#endif
            this.verify = verify;
            if (callQueueCount > 0)
            {
                if (callQueueCount == 1)
                {
                    CallQueue = new ServerCallCanDisposableQueue(true, Log);
                    if (isCallQueueLink) CallQueueLink = CallQueue.CreateLink();
                }
                else
                {
                    CallQueueArray = new KeyValue<ServerCallCanDisposableQueue, Threading.TaskQueueThread<ServerCallBase>.LowPriorityLink>[Math.Min(callQueueCount, 256)];
                    if (isCallQueueLink)
                    {
                        for (int index = 0; index != CallQueueArray.Length; ++index)
                        {
                            ServerCallCanDisposableQueue callQueue = new ServerCallCanDisposableQueue(true, Log);
                            CallQueueArray[index].Set(callQueue, callQueue.CreateLink());
                        }
                        CallQueueLink = CallQueueArray[0].Value;
                    }
                    else
                    {
                        for (int index = 0; index != CallQueueArray.Length; ++index) CallQueueArray[index].Key = new ServerCallCanDisposableQueue(true, Log);
                    }
                    CallQueue = CallQueueArray[0].Key;
                }
            }
            ServerAttribute.Set(attribute);
            Port = attribute.Port;
            IpAddress = HostPort.HostToIPAddress(attribute.Host, Log);
            int binaryDeSerializeMaxArraySize = attribute.GetBinaryDeSerializeMaxArraySize;
            BinaryDeSerializeConfig = AutoCSer.Net.TcpOpenServer.ServerAttribute.GetBinaryDeSerializeConfig(binaryDeSerializeMaxArraySize <= 0 ? AutoCSer.BinaryDeSerializer.DefaultConfig.MaxArraySize : binaryDeSerializeMaxArraySize);
            VerifyMethodCount = isSynchronousVerifyMethod ? Server.DefaultVerifyMethodCount : (byte)(Server.DefaultVerifyMethodCount + 1);
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposed, 1, 0) == 0)
            {
                if (isListen != 0)
                {
                    isListen = 0;
                    if (Log.IsAnyLevel(LogLevel.Info)) Log.Info("停止服务 " + Attribute.ServerName + " " + IpAddress.ToString() + "[" + Attribute.Host + "]:" + Port.toString(), LogLevel.Info | LogLevel.AutoCSer);
                    AutoCSer.DomainUnload.Unloader.Remove(this, DomainUnload.Type.TcpCommandBaseDispose, false);
                    StopListen();
                }
                if (CallQueueArray != null)
                {
                    foreach (KeyValue<ServerCallCanDisposableQueue, ServerCallCanDisposableQueue.LowPriorityLink> callQueue in CallQueueArray) callQueue.Key.Dispose();
                }
                else if (CallQueue != null) CallQueue.Dispose();
            }
        }
        /// <summary>
        /// 停止服务监听
        /// </summary>
        public virtual void StopListen()
        {
            if (Socket != null)
            {
                ShutdownServer(Socket);
                Socket = null;
            }
            Unmanaged.Free(ref commandData);
#if !NOJIT
            extendServerSet.Free();
#endif
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
                if (Port == 0) Log.Warn(GetType().FullName + "服务器端口为 0", LogLevel.Warn | LogLevel.AutoCSer);
            }
            catch (Exception error)
            {
                Dispose();
                Log.Exception(error, GetType().FullName + "服务器端口 " + Attribute.ServerName + " " + IpAddress.ToString() + "[" + Attribute.Host + "]:" + Port.toString() + " TCP连接失败)", LogLevel.Exception | LogLevel.AutoCSer);
            }
            return isListen != 0;
        }
        /// <summary>
        /// 获取 TCP 内部注册服务客户端
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="tcpRegisterName"></param>
        /// <param name="tcpRegisterClient"></param>
        /// <returns></returns>
        internal bool GetRegisterClient(ServerBaseAttribute attribute, string tcpRegisterName, ref TcpRegister.Client tcpRegisterClient)
        {
            if (tcpRegisterName != null)
            {
                tcpRegisterClient = TcpRegister.Client.Get(tcpRegisterName, Log);
                if (tcpRegisterClient == null)
                {
                    Log.Error("TCP 内部注册服务 " + tcpRegisterName + " 客户端获取失败", LogLevel.Error | LogLevel.AutoCSer);
                    return false;
                }
                if (attribute.ClientRegisterHost == null) attribute.ClientRegisterHost = attribute.Host;
                if (attribute.ClientRegisterPort == 0) attribute.ClientRegisterPort = attribute.Port;
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
            commandData = Unmanaged.GetPointer(((count + TcpServer.Server.CommandStartIndex + 63) >> 6) << 3, true);
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
        protected internal void setCommand(int methodIndex)
        {
            int command = methodIndex + TcpServer.Server.CommandStartIndex;
            if (command > MaxCommand)
            {
#if NOJIT
                if (command > (int)Server.CommandIndexAnd)
#else
                if (!extendServerSet.CheckMaxCommand(command))
#endif
                {
                    throw new IndexOutOfRangeException("命令索引超出最大范围");
                }
                MaxCommand = command;
            }
            commands.Set(command);
        }
        /// <summary>
        /// 创建客户端命令数据
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        internal bool CreateCommandData(ref AutoCSer.Memory.Pointer commandData, ref MemoryMap commands)
        {
            if (this.commandData.Data != null)
            {
                commandData = Unmanaged.GetPointer8(this.commandData.ByteSize, true);
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
#if NOJIT
            if (Log.IsAllType(AutoCSer.LogLevel.Info)) Log.Add(AutoCSer.LogLevel.Info, ServerAttribute.ServerName + " 缺少命令处理委托 [" + index.toString() + "]");
            return false;
#else
            return extendServerSet.IsCommand(index);
#endif
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
        internal bool PushReceiveVerifyCommandTimeout(SocketTimeoutNode serverSocket, Socket socket)
        {
            SocketTimeoutLink receiveVerifyCommandTimeout = this.ReceiveVerifyCommandTimeout;
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
        internal void CancelReceiveVerifyCommandTimeout(SocketTimeoutNode socket)
        {
            SocketTimeoutLink receiveVerifyCommandTimeout = this.ReceiveVerifyCommandTimeout;
            if (receiveVerifyCommandTimeout != null) receiveVerifyCommandTimeout.Cancel(socket);
        }
        /// <summary>
        /// 检测默认验证字符串
        /// </summary>
        /// <returns></returns>
        internal bool CheckVerifyString()
        {
            string verify = Attribute.VerifyString;
            if (verify == null)
            {
                if (AutoCSer.Common.Config.IsDebug)
                {
                    Log.Debug("警告：调试模式未启用服务验证 " + ServerAttribute.ServerName, LogLevel.Debug | LogLevel.Warn | LogLevel.AutoCSer);
                    return true;
                }
                Log.Error("服务 " + ServerAttribute.ServerName + " 验证字符串不能为空", LogLevel.Error | LogLevel.AutoCSer);
            }
            return false;
        }
#if !NOJIT
        /// <summary>
        /// 添加扩展服务
        /// </summary>
        /// <param name="interfaceType">服务接口类型</param>
        /// <param name="name">服务绑定名称</param>
        /// <returns>是否添加成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool AppendExtendServer(Type interfaceType, string name = null)
        {
            return extendServerSet.Append(interfaceType, name);
        }
        /// <summary>
        /// 创建扩展服务
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        internal virtual ExtendServer CreateExtendServer(Type interfaceType)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
