using System;
using System.Threading;
using System.Net;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

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
