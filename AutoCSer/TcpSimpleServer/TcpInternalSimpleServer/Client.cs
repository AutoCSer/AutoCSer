using System;
using System.Net;
using AutoCSer.Log;
using AutoCSer.Extensions;
using System.Threading;
using System.Net.Sockets;

namespace AutoCSer.Net.TcpInternalSimpleServer
{
    /// <summary>
    /// TCP 内部服务客户端
    /// </summary>
    public abstract class Client : TcpSimpleServer.Client, TcpRegister.IClient
    {
        /// <summary>
        /// TCP 内部注册服务客户端
        /// </summary>
        private TcpRegister.Client tcpRegisterClient;
#if !NOJIT
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        internal Client() : base() { }
#endif
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        public Client(ServerAttribute attribute, ILog log)
            : base(attribute, log, int.MaxValue)
        {
            if (attribute.TcpRegisterName != null)
            {
                tcpRegisterClient = AutoCSer.Net.TcpRegister.Client.Get(attribute.TcpRegisterName, Log);
                tcpRegisterClient.Register(this);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (tcpRegisterClient != null)
            {
                tcpRegisterClient.Remove(this);
                tcpRegisterClient = null;
            }
        }
        /// <summary>
        /// 服务更新
        /// </summary>
        /// <param name="serverSet"></param>
        void TcpRegister.IClient.OnServerChange(TcpRegister.ServerSet serverSet)
        {
            if (serverSet != null)
            {
                TcpRegister.ServerLog server = serverSet.Server;
                IPAddress ipAddress = HostPort.HostToIPAddress(server.Host, Log);
                if (server.Port != Port || !ipAddress.Equals(IpAddress))
                {
                    Host = server.Host;
                    IpAddress = ipAddress;
                    Port = server.Port;
                }
                createSocket();
            }
        }
        /// <summary>
        /// 创建套接字
        /// </summary>
        private void createSocket()
        {
            if (check(IpAddress, Port))
            {
                Socket socket = null, oldSocket = null;
                bool isVerifyMethod = false;
                Monitor.Enter(SocketLock);
                try
                {
                    socket = new Socket(IpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
#if !MONO
                    socket.ReceiveBufferSize = socket.SendBufferSize = Buffer.Length;
#endif
                    socket.Connect(IpAddress, Port);

                    oldSocket = Socket;
                    Socket = socket;
                    socket = null;

                    if ((isVerifyMethod = CallVerifyMethod()) && Attribute.GetCheckSeconds > 0 && CheckTimer == null)
                    {
                        TcpSimpleServer.ClientCheckTimer checkTimer = new TcpSimpleServer.ClientCheckTimer(this, Math.Max(Attribute.GetCheckSeconds,1));
                        if (Interlocked.CompareExchange(ref CheckTimer, checkTimer, null) == null)
                        {
                            if (IsDisposed == 0) checkTimer.AppendTaskArray();
                            else isVerifyMethod = false;
                        }
                    }
                }
                finally
                {
                    if (!isVerifyMethod) closeSocket();
                    Monitor.Exit(SocketLock);
                    if (socket != null) ShutdownClient(socket);
                    if (oldSocket != null) ShutdownClient(oldSocket);
                }
            }
        }
    }
    /// <summary>
    /// TCP 内部服务客户端
    /// </summary>
    /// <typeparam name="clientType">客户端代理类型</typeparam>
    public sealed class Client<clientType> : Client
    {
        /// <summary>
        /// TCP 内部服务客户端代理对象
        /// </summary>
        public readonly clientType MethodClient;
        /// <summary>
        /// 验证委托
        /// </summary>
        private readonly Func<clientType, bool> verifyMethod;
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        /// <param name="client">TCP 服务客户端对象</param>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        /// <param name="verifyMethod">验证委托</param>
        public Client(clientType client, ServerAttribute attribute, ILog log, Func<clientType, bool> verifyMethod = null)
            : base(attribute, log)
        {
            this.MethodClient = client;
            this.verifyMethod = verifyMethod;
        }
        /// <summary>
        /// 套接字验证
        /// </summary>
        /// <returns></returns>
        internal override bool CallVerifyMethod()
        {
            return verifyMethod == null || verifyMethod(MethodClient);
        }
    }
}
