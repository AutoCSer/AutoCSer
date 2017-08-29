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
    public abstract unsafe partial class Server : TcpSimpleServer.Server<ServerAttribute, ServerSocket>, TcpRegister.IServer
    {
        /// <summary>
        /// TCP 内部注册服务客户端
        /// </summary>
        private TcpRegister.Client tcpRegisterClient;
        /// <summary>
        /// TCP 服务注册信息
        /// </summary>
        TcpRegister.ServerInfo TcpRegister.IServer.TcpRegisterInfo { get; set; }
        /// <summary>
        /// 创建 TCP 服务注册信息
        /// </summary>
        /// <returns></returns>
        TcpRegister.ServerInfo TcpRegister.IServer.CreateServerInfo()
        {
            return new TcpRegister.ServerInfo
            {
                Host = Attribute.RegisterHost,
                Port = Attribute.RegisterPort,
                IsSingle = Attribute.IsSingleRegister,
                Name = Attribute.ServerName,
            };
        }
        /// <summary>
        /// TCP 内部服务端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="verify">同步验证接口</param>
        /// <param name="log">日志接口</param>
        /// <param name="isCallQueue">是否提供独占的 TCP 服务器端同步调用队列</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public Server(ServerAttribute attribute, Func<System.Net.Sockets.Socket, bool> verify, ILog log, bool isCallQueue)
            : base(attribute, log, verify, isCallQueue)
        {
            if (!attribute.IsServer) Log.add(AutoCSer.Log.LogType.Warn, "配置未指明的 TCP 服务端 " + attribute.ServerName);
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
                if (Attribute.TcpRegisterName != null)
                {
                    tcpRegisterClient = TcpRegister.Client.Get(Attribute.TcpRegisterName, Log);
                    if (tcpRegisterClient == null)
                    {
                        Log.add(AutoCSer.Log.LogType.Error, "TCP 内部注册服务 " + Attribute.TcpRegisterName + " 客户端获取失败");
                        return false;
                    }
                    if (Attribute.RegisterHost == null) Attribute.RegisterHost = Attribute.Host;
                    if (Attribute.RegisterPort == 0) Attribute.RegisterPort = Attribute.Port;
                    if (Attribute.RegisterPort == 0)
                    {
                        if (tcpRegisterClient.GetPort(Attribute)) Port = Attribute.Port;
                        else
                        {
                            Log.add(AutoCSer.Log.LogType.Error, "TCP 内部服务 " + Attribute.ServerName + " 端口获取失败");
                            return false;
                        }
                    }
                }
                if (listen())
                {
                    startGetSocket();
                    if (Attribute.TcpRegisterName != null)
                    {
                        if (tcpRegisterClient.Register(this))
                        {
                            Log.add(AutoCSer.Log.LogType.Info, Attribute.ServerName + " 注册 " + Attribute.Host + ":" + Attribute.Port.toString() + " => " + Attribute.RegisterHost + ":" + Attribute.RegisterPort.toString());
                        }
                        else Log.add(AutoCSer.Log.LogType.Error, "TCP 内部服务注册 " + Attribute.ServerName + " 失败 ");
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
            ReceiveVerifyCommandTimeout = TcpServer.ServerSocket.TimerLink.Get(Attribute.ReceiveVerifyCommandSeconds > 0 ? Attribute.ReceiveVerifyCommandSeconds : TcpInternalServer.ServerAttribute.DefaultReceiveVerifyCommandSeconds);
            if (verify == null) getSocket();
            else getSocketVerify();
            TcpSimpleServer.ServerSocket.TimerLink.Free(ref ReceiveVerifyCommandTimeout);
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
#if DotNetStandard
                                AutoCSer.Net.TcpServer.CommandBase.CloseServer(listenSocket);
#else
                                listenSocket.Dispose();
#endif
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
                    Log.add(AutoCSer.Log.LogType.Error, error);
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
#if DotNetStandard
                                AutoCSer.Net.TcpServer.CommandBase.CloseServer(listenSocket);
#else
                                listenSocket.Dispose();
#endif
                            }
                            return;
                        }
                        if (verify(serverSocket.Socket)) ServerSocketTask.Task.Add(serverSocket);
                        else
                        {
#if DotNetStandard
#else
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
                    Log.add(AutoCSer.Log.LogType.Error, error);
                    Thread.Sleep(1);
                }
            }
        }
    }
}
