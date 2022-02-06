using System;
using AutoCSer.Log;
using AutoCSer.Extensions;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// TCP 内部服务端
    /// </summary>
    public abstract unsafe partial class Server : TcpServer.Server<Server, ServerSocketSender>, TcpRegister.IServer
    {
        ///// <summary>
        ///// 读取服务名称后缀
        ///// </summary>
        //public const string TcpRegisterReaderServerNameSuffix = "Reader";
        /// <summary>
        /// TCP 内部注册随机标识
        /// </summary>
        private readonly ulong RegisterRandom = AutoCSer.Random.Default.SecureNextULong() ^ (ulong)AutoCSer.Random.Hash;
        /// <summary>
        /// TCP 内部注册服务客户端
        /// </summary>
        private TcpRegister.Client tcpRegisterClient;
        /// <summary>
        /// TCP 内部注册服务日志
        /// </summary>
        private TcpRegister.ServerLog tcpRegisterServerLog;
        /// <summary>
        /// 创建 TCP 服务注册信息
        /// </summary>
        /// <param name="logType">TCP 内部注册服务更新日志类型</param>
        /// <returns></returns>
        TcpRegister.ServerLog TcpRegister.IServer.CreateServerLog(TcpRegister.LogType logType)
        {
            if (logType == TcpRegister.LogType.RegisterServer)
            {
                if (tcpRegisterServerLog == null) tcpRegisterServerLog = createServerLog(logType);
                return tcpRegisterServerLog;
            }
            return createServerLog(logType);
        }
        /// <summary>
        /// 创建 TCP 服务注册信息
        /// </summary>
        /// <param name="logType">TCP 内部注册服务更新日志类型</param>
        /// <returns></returns>
        private TcpRegister.ServerLog createServerLog(TcpRegister.LogType logType)
        {
            return new TcpRegister.ServerLog
            {
                Random = RegisterRandom,
                Host = Attribute.ClientRegisterHost,
                Port = Attribute.ClientRegisterPort,
                LogType = logType,
                IsSingle = Attribute.GetIsSingleRegister,
                IsMain = Attribute.GetIsMainRegister,
                Name = ServerAttribute.ServerName,
            };
        }
        /// <summary>
        /// TCP 内部服务端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">同步验证接口</param>
        /// <param name="serverCallQueue">自定义队列</param>
        /// <param name="extendCommandBits">扩展服务命令二进制位数</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="callQueueCount">独占的 TCP 服务器端同步调用队列数量</param>
        /// <param name="isCallQueueLink">是否提供独占的 TCP 服务器端同步调用队列（低优先级）</param>
        /// <param name="isSynchronousVerifyMethod">验证函数是否同步调用</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public Server(ServerAttribute attribute, Func<System.Net.Sockets.Socket, bool> verify, AutoCSer.Net.TcpServer.IServerCallQueueSet serverCallQueue, byte extendCommandBits, Action<SubArray<byte>> onCustomData, ILog log, int callQueueCount, bool isCallQueueLink, bool isSynchronousVerifyMethod)
            : base(attribute, verify, serverCallQueue, extendCommandBits, onCustomData, log, AutoCSer.Threading.ThreadTaskType.TcpInternalServerGetSocket, callQueueCount, isCallQueueLink, isSynchronousVerifyMethod)
        {
            if (!attribute.IsServer) Log.Warn("配置未指明的 TCP 服务端 " + ServerAttribute.ServerName, LogLevel.Warn | LogLevel.AutoCSer);
        }
        /// <summary>
        /// 停止服务监听
        /// </summary>
        public override void StopListen()
        {
            base.StopListen();
            if (tcpRegisterClient != null) tcpRegisterClient.RemoveRegister(this);
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns>是否成功</returns>
        public override bool Start()
        {
            if (isListen != 0) return true;
            if (IsDisposed == 0 && Interlocked.CompareExchange(ref isStart, 1, 0) == 0)
            {
                if (!GetRegisterClient(Attribute, Attribute.TcpRegisterName, ref tcpRegisterClient)) return false;
                if (listen())
                {
                    startGetSocket();
                    if (Attribute.TcpRegisterName != null)
                    {
                        if (tcpRegisterClient.Register(this)) 
                        {
                            Log.Info(ServerAttribute.ServerName + " 注册 " + Attribute.Host + ":" + Port.toString() + " => " + Attribute.ClientRegisterHost + ":" + Attribute.ClientRegisterPort.toString(), LogLevel.Info | LogLevel.AutoCSer);
                        }
                        else Log.Error("TCP 内部服务注册 " + ServerAttribute.ServerName + " 失败 ", LogLevel.Error | LogLevel.AutoCSer);
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        internal void GetSocket()
        {
            ReceiveVerifyCommandTimeout = new SocketTimeoutLink(ServerAttribute.ReceiveVerifyCommandSeconds > 0 ? ServerAttribute.ReceiveVerifyCommandSeconds : TcpInternalServer.ServerAttribute.DefaultReceiveVerifyCommandSeconds);
            try
            {
                if (verify == null) getSocket();
                else getSocketVerify();
            }
            finally { SocketTimeoutLink.Free(ref ReceiveVerifyCommandTimeout); }
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        private void getSocket()
        {
            Socket listenSocket = this.Socket;
            while (isListen != 0)
            {
                try
                {
                    do
                    {
                        ServerSocket serverSocket = new ServerSocket(this);
                        serverSocket.Socket = listenSocket.Accept();
                        if (isListen == 0)
                        {
                            if (this.Socket != null)
                            {
                                this.Socket = null;
                                ShutdownServer(listenSocket);
                            }
                            return;
                        }
                        ServerSocketThreadArray.Default.CurrentThread.Add(serverSocket);
                    }
                    while (true);
                }
                catch (Exception error)
                {
                    if (isListen == 0) return;
                    AutoCSer.LogHelper.Exception(error);
                    Thread.Sleep(1);
                }
            }
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        private void getSocketVerify()
        {
            Socket listenSocket = this.Socket;
            while (isListen != 0)
            {
                try
                {
                    do
                    {
                        ServerSocket serverSocket = new ServerSocket(this);
                        ACCEPT:
                        serverSocket.Socket = listenSocket.Accept();
                        if (isListen == 0)
                        {
                            if (this.Socket != null)
                            {
                                this.Socket = null;
                                ShutdownServer(listenSocket);
                            }
                            return;
                        }
                        if (verify(serverSocket.Socket)) ServerSocketThreadArray.Default.CurrentThread.Add(serverSocket);
                        else
                        {
#if !DotNetStandard
                            serverSocket.Socket.Dispose();
#endif
                            serverSocket.Socket = null;
                            goto ACCEPT;
                        }
                    }
                    while (true);
                }
                catch (Exception error)
                {
                    if (isListen == 0) return;
                    AutoCSer.LogHelper.Exception(error);
                    Thread.Sleep(1);
                }
            }
        }
    }
}
