﻿using System;
using AutoCSer.Log;
using AutoCSer.Extensions;
using System.Threading;
using System.Net;
using AutoCSer.Net.TcpServer;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// TCP 内部服务客户端
    /// </summary>
    public abstract class Client : TcpServer.Client, TcpRegister.IClient
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
        /// <param name="maxTimeoutSeconds">最大超时秒数</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="clientRoute">TCP 客户端路由</param>
        internal Client(ServerAttribute attribute, ushort maxTimeoutSeconds, Action<SubArray<byte>> onCustomData, ILog log, AutoCSer.Net.TcpServer.ClientLoadRoute<ClientSocketSender> clientRoute)
            : base(attribute, maxTimeoutSeconds, onCustomData, log)
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
                    return socket == null ? null : new UnionType.ClientSocketSender { Object = socket.Sender }.Value;
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
            if (serverSet == null || IsDisposed != 0) SocketWait.PulseReset();
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
        internal override TcpServer.ClientSocketBase CreateSocketByCreator(TcpServer.ClientSocketCreator clientCreator, IPAddress ipAddress, int port, int createVersion)
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
        /// 创建客户端等待连接
        /// </summary>
        /// <param name="clientWaitConnected">客户端等待连接</param>
        /// <param name="onCheckSocketVersion">TCP 客户端套接字初始化处理</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateWaitConnected(out ClientWaitConnected clientWaitConnected, Action<ClientSocketEventParameter> onCheckSocketVersion = null)
        {
            clientWaitConnected = new ClientWaitConnected(this, onCheckSocketVersion);
            TryCreateSocket();
        }
        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SendCustomData(byte[] data)
        {
            if (data.Length > maxCustomDataSize) throw new ArgumentOutOfRangeException("data.Length" + data.Length.toString() + " > maxCustomDataSize[" + maxCustomDataSize.toString() + "]");
            ClientSocketSender sender = Sender;
            return sender.CustomData(data);
        }
        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SendCustomData(ref SubArray<byte> data)
        {
            if (data.Length > maxCustomDataSize) throw new ArgumentOutOfRangeException("data.Length" + data.Length.toString() + " > maxCustomDataSize[" + maxCustomDataSize.toString() + "]");
            ClientSocketSender sender = Sender;
            return sender.CustomData(ref data);
        }
        /// <summary>
        /// 发送自定义数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否添加到发送队列</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool SendCustomData(SubArray<byte> data)
        {
            if (data.Length > maxCustomDataSize) throw new ArgumentOutOfRangeException("data.Length" + data.Length.toString() + " > maxCustomDataSize[" + maxCustomDataSize.toString() + "]");
            ClientSocketSender sender = Sender;
            return sender.CustomData(ref data);
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
        public readonly clientType MethodClient;
        /// <summary>
        /// 验证委托
        /// </summary>
        private readonly Func<clientType, ClientSocketSender, bool> verifyMethod;
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        /// <param name="client">TCP 服务客户端对象</param>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="maxTimeoutSeconds">最大超时秒数</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="clientRoute">TCP 客户端路由</param>
        /// <param name="verifyMethod">验证委托</param>
        public Client(clientType client, ServerAttribute attribute, ushort maxTimeoutSeconds, Action<SubArray<byte>> onCustomData, ILog log, AutoCSer.Net.TcpServer.ClientLoadRoute<ClientSocketSender> clientRoute = null, Func<clientType, ClientSocketSender, bool> verifyMethod = null)
            : base(attribute, maxTimeoutSeconds, onCustomData, log, clientRoute)
        {
            this.MethodClient = client;
            this.verifyMethod = verifyMethod;
        }
        /// <summary>
        /// 套接字验证
        /// </summary>
        /// <param name="socket">TCP 调用客户端套接字</param>
        /// <returns></returns>
        internal override bool SocketVerifyMethod(TcpServer.ClientSocketSenderBase socket)
        {
            return verifyMethod == null || verifyMethod(MethodClient, new UnionType.ClientSocketSender { Object = socket }.Value);
        }
    }
}
