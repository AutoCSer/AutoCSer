using System;
using System.Threading;
using System.Net;
using AutoCSer.Log;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpOpenStreamServer
{
    /// <summary>
    /// TCP 开放服务客户端
    /// </summary>
    public abstract class Client : TcpStreamServer.Client<ServerAttribute>
    {
        /// <summary>
        /// TCP 开放服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        public Client(ServerAttribute attribute, ILog log)
            : base(attribute, log)
        {
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
        /// 等待套接字
        /// </summary>
        /// <returns></returns>
        private TcpServer.ClientSocketBase waitSocket()
        {
            TryCreateSocket();
            SocketWait.Wait();
            return Socket;
        }
        /// <summary>
        /// 尝试创建第一个套接字
        /// </summary>
        public void TryCreateSocket()
        {
            if (Interlocked.CompareExchange(ref CreateVersion, 1, 0) == 0)
            {
                IPAddress ipAddress = IpAddress;
                int port = Port;
                if (check(ipAddress, port)) CreateSocket = new ClientSocket(this, ipAddress, port, 1);
                else SocketWait.Set();
            }
        }
    }
    /// <summary>
    /// TCP 开放服务客户端
    /// </summary>
    /// <typeparam name="clientType">客户端代理类型</typeparam>
    public sealed class Client<clientType> : Client
    //where clientType : class
    {
        /// <summary>
        /// TCP 开放服务客户端代理对象
        /// </summary>
        private clientType client;
        /// <summary>
        /// 验证委托
        /// </summary>
        private Func<clientType, ClientSocketSender, bool> verifyMethod;
        /// <summary>
        /// TCP 开放服务客户端
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
