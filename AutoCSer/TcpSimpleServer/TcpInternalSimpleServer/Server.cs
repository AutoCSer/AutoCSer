using System;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalSimpleServer
{
    /// <summary>
    /// TCP 内部服务端
    /// </summary>
    public abstract unsafe partial class Server : TcpSimpleServer.Server<Server, ServerSocket>, TcpRegister.IServer
    {
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
                IsSingle = Attribute.GetIsSingleRegister,
                IsMain = Attribute.GetIsMainRegister,
                LogType = logType,
                Name = ServerAttribute.ServerName,
            };
        }
        /// <summary>
        /// TCP 内部服务端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">同步验证接口</param>
        /// <param name="log">日志接口</param>
        /// <param name="isSynchronousVerifyMethod">验证函数是否同步调用</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public Server(ServerAttribute attribute, Func<System.Net.Sockets.Socket, bool> verify, ILog log, bool isSynchronousVerifyMethod)
            : base(attribute, log, verify, isSynchronousVerifyMethod)
        {
            if (!attribute.IsServer) Log.Add(AutoCSer.Log.LogType.Warn, "配置未指明的 TCP 服务端 " + ServerAttribute.ServerName);
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
                            Log.Add(AutoCSer.Log.LogType.Info, ServerAttribute.ServerName + " 注册 " + Attribute.Host + ":" + Attribute.Port.toString() + " => " + Attribute.ClientRegisterHost + ":" + Attribute.ClientRegisterPort.toString());
                        }
                        else Log.Add(AutoCSer.Log.LogType.Error, "TCP 内部服务注册 " + ServerAttribute.ServerName + " 失败 ");
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取客户端请求
        /// </summary>
        internal override void GetSocket()
        {
            ReceiveVerifyCommandTimeout = SocketTimeoutLink.TimerLink.Get(ServerAttribute.ReceiveVerifyCommandSeconds > 0 ? ServerAttribute.ReceiveVerifyCommandSeconds : TcpInternalServer.ServerAttribute.DefaultReceiveVerifyCommandSeconds);
            if (verify == null) getSocket();
            else getSocketVerify();
            SocketTimeoutLink.TimerLink.Free(ref ReceiveVerifyCommandTimeout);
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
                        ServerSocketTask.Task.Add(serverSocket);
                    }
                    while (true);
                }
                catch (Exception error)
                {
                    if (isListen == 0) return;
                    Log.Add(AutoCSer.Log.LogType.Error, error);
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
                        if (verify(serverSocket.Socket)) ServerSocketTask.Task.Add(serverSocket);
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
                    Log.Add(AutoCSer.Log.LogType.Error, error);
                    Thread.Sleep(1);
                }
            }
        }
    }
}
