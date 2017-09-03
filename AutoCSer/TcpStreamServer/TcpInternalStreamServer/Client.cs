using System;
using System.Net;
using System.Threading;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalStreamServer
{
    /// <summary>
    /// TCP 内部服务客户端
    /// </summary>
    public abstract class Client : TcpStreamServer.Client<ServerAttribute>, TcpRegister.IClient
    {
        /// <summary>
        /// TCP 内部注册服务客户端
        /// </summary>
        private TcpRegister.Client tcpRegisterClient;
        /// <summary>
        /// 服务名称
        /// </summary>
        string TcpRegister.IClient.ServerName { get { return base.ServerName; } }
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        public Client(ServerAttribute attribute, ILog log)
            : base(attribute, log)
        {
            if (attribute.TcpRegisterName != null)
            {
                tcpRegisterClient = AutoCSer.Net.TcpRegister.Client.Get(attribute.TcpRegisterName, Log);
                tcpRegisterClient.Register(this);
            }
        }
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public ClientSocketSender Sender
        {
            get
            {
                TcpServer.ClientSocketBase socket = this.Socket ?? waitSocket();
                return socket == null ? null : new UnionType { Value = socket.Sender }.ClientSocketSender;
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
            if (serverSet == null) SocketWait.PulseReset();
            else
            {
                TcpRegister.ServerInfo server = serverSet.Server.Server;
                IPAddress ipAddress = HostPort.HostToIPAddress(server.Host, Log);
                if (server.Port == Port && ipAddress.Equals(IpAddress))
                {
                    if (!server.IsCheckRegister) TryCreateSocket();
                }
                else
                {
                    Host = server.Host;
                    createSocket(IpAddress = ipAddress, Port = server.Port, Interlocked.Increment(ref CreateVersion));
                }
            }
        }
        /// <summary>
        /// 等待套接字
        /// </summary>
        /// <returns></returns>
        private TcpServer.ClientSocketBase waitSocket()
        {
            if (Attribute.TcpRegisterName == null) TryCreateSocket();
            SocketWait.Wait();
            return Socket;
        }
        /// <summary>
        /// 尝试创建第一个套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void TryCreateSocket()
        {
            if (Interlocked.CompareExchange(ref CreateVersion, 1, 0) == 0) createSocket(IpAddress, Port, 1);
        }
        /// <summary>
        /// 创建套接字
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="createVersion"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void createSocket(IPAddress ipAddress, int port, int createVersion)
        {
            if (check(ipAddress, port)) CreateSocket = new ClientSocket(this, ipAddress, port, createVersion);
            else SocketWait.Set();
        }
    }
    /// <summary>
    /// TCP 内部服务客户端
    /// </summary>
    /// <typeparam name="clientType">客户端代理类型</typeparam>
    public sealed class Client<clientType> : Client
    //where clientType : class
    {
        /// <summary>
        /// TCP 内部服务客户端代理对象
        /// </summary>
        private readonly clientType client;
        /// <summary>
        /// 验证委托
        /// </summary>
        private readonly Func<clientType, ClientSocketSender, bool> verifyMethod;
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        /// <param name="client">TCP 服务客户端对象</param>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        /// <param name="verifyMethod">验证委托</param>
        public Client(clientType client, ServerAttribute attribute, ILog log, Func<clientType, ClientSocketSender, bool> verifyMethod = null)
            : base(attribute, log)
        {
            this.client = client;
            this.verifyMethod = verifyMethod;
        }
        /// <summary>
        /// 套接字验证
        /// </summary>
        /// <param name="socket">TCP 调用客户端套接字</param>
        /// <returns></returns>
        internal override bool SocketVerifyMethod(TcpServer.ClientSocketSenderBase socket)
        {
            return verifyMethod == null || verifyMethod(client, new UnionType { Value = socket }.ClientSocketSender);
        }
    }
}
