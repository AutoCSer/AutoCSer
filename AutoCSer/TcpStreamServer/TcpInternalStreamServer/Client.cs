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
        /// TCP 客户端路由
        /// </summary>
        private readonly AutoCSer.Net.TcpServer.ClientLoadRoute<ClientSocketSender> clientRoute;
        /// <summary>
        /// TCP 内部注册服务客户端
        /// </summary>
        private TcpRegister.Client tcpRegisterClient;
        /// <summary>
        /// 服务名称
        /// </summary>
        string TcpRegister.IClient.ServerName { get { return base.ServerName; } }
        /// <summary>
        /// 注册当前服务的 TCP 注册服务名称
        /// </summary>
        internal override string TcpRegisterName { get { return Attribute.TcpRegisterName; } }
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
        /// <param name="clientRoute">TCP 客户端路由</param>
        internal Client(ServerAttribute attribute, ILog log, AutoCSer.Net.TcpServer.ClientLoadRoute<ClientSocketSender> clientRoute)
            : base(attribute, log)
        {
            this.clientRoute = clientRoute;
            if (attribute.TcpRegisterName != null)
            {
                tcpRegisterClient = AutoCSer.Net.TcpRegister.Client.Get(attribute.TcpRegisterName, Log);
                tcpRegisterClient.Register(this);
            }
        }
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public ClientSocketSender Sender
        {
            get
            {
                if (clientRoute == null)
                {
                    TcpServer.ClientSocketBase socket = clientCreator.WaitSocket();
                    return socket == null ? null : new UnionType { Value = socket.Sender }.ClientSocketSender;
                }
                return clientRoute.Sender;
            }
        }
#if !NOJIT
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ClientSocketSender GetSender()
        {
            return Sender;
        }
#endif
        /// <summary>
        /// 套接字发送数据次数
        /// </summary>
        public override int SendCount
        {
            get
            {
                ClientSocketSender sender = Sender;
                return sender != null ? sender.SendCount : 0;
            }
        }
        /// <summary>
        /// 套接字接收数据次数
        /// </summary>
        public override int ReceiveCount
        {
            get
            {
                TcpServer.ClientSocketBase socket = clientRoute == null ? clientCreator.Socket : clientRoute.Socket;
                return socket != null ? socket.ReceiveCount : 0;
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
        /// 释放套接字
        /// </summary>
        internal override void DisposeSocket()
        {
            if (clientRoute == null) clientCreator.DisposeSocket();
            else clientRoute.DisposeSocket();
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
                if (clientRoute == null) clientCreator.OnServerChange(serverSet);
                else clientRoute.OnServerChange(serverSet);
            }
        }
        /// <summary>
        /// 尝试创建第一个套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void TryCreateSocket()
        {
            if (clientRoute == null) clientCreator.TryCreateSocket();
            else clientRoute.TryCreateSocket();
        }
        /// <summary>
        /// 创建套接字
        /// </summary>
        /// <param name="clientCreator"></param>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="createVersion"></param>
        internal override TcpServer.ClientSocketBase CreateSocketByCreator(TcpServer.ClientSocketCreator<ServerAttribute> clientCreator, IPAddress ipAddress, int port, int createVersion)
        {
            return new ClientSocket(clientCreator, ipAddress, port, createVersion);
        }
        /// <summary>
        /// 设置 TCP 客户端套接字事件
        /// </summary>
        internal override void OnSetSocket()
        {
            if (clientRoute == null) clientCreator.OnSetSocket();
            else clientRoute.OnSetSocket();
        }
        /// <summary>
        /// 获取客户端远程表达式节点
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <param name="clientNode">客户端远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ReturnType GetRemoteExpressionClientNode(RemoteExpression.Node node, out RemoteExpression.ClientNode clientNode)
        {
            return Sender.GetRemoteExpressionClientNode(node, out clientNode);
        }
        /// <summary>
        /// 获取客户端远程表达式节点
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>客户端远程表达式节点</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public RemoteExpression.ClientNode GetRemoteExpressionClientNode(RemoteExpression.Node node)
        {
            return Sender.GetRemoteExpressionClientNode(node);
        }
        /// <summary>
        /// 获取客户端远程表达式参数节点
        /// </summary>
        /// <typeparam name="returnType">返回值类型</typeparam>
        /// <param name="node">远程表达式参数节点</param>
        /// <returns>客户端远程表达式参数节点</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public RemoteExpression.ClientNode<returnType> GetRemoteExpressionClientNodeParameter<returnType>(RemoteExpression.Node<returnType> node)
        {
            return Sender.GetRemoteExpressionClientNodeParameter(node);
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ReturnType CallRemoteExpression(RemoteExpression.Node node)
        {
            return Sender.CallRemoteExpression(node);
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ReturnValue<returnType> GetRemoteExpression<returnType>(RemoteExpression.Node<returnType> node)
        {
            return Sender.GetRemoteExpression(node);
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.ReturnValue<RemoteExpression.ReturnValue> GetRemoteExpression(RemoteExpression.ClientNode node)
        {
            return Sender.GetRemoteExpression(node);
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.AwaiterBox<RemoteExpression.ReturnValue> GetRemoteExpressionAwaiter(RemoteExpression.Node node)
        {
            return Sender.GetRemoteExpressionAwaiter(node);
        }
        /// <summary>
        /// 获取远程表达式数据
        /// </summary>
        /// <param name="node">远程表达式节点</param>
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TcpServer.AwaiterBox<RemoteExpression.ReturnValue> GetRemoteExpressionAwaiter(RemoteExpression.ClientNode node)
        {
            return Sender.GetRemoteExpressionAwaiter(node);
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
        /// <param name="clientRoute">TCP 客户端路由</param>
        /// <param name="verifyMethod">验证委托</param>
        public Client(clientType client, ServerAttribute attribute, ILog log, AutoCSer.Net.TcpServer.ClientLoadRoute<ClientSocketSender> clientRoute = null, Func<clientType, ClientSocketSender, bool> verifyMethod = null)
            : base(attribute, log, clientRoute)
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
