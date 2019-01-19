using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 客户端路由
    /// </summary>
    /// <typeparam name="ClientSocketSenderType">TCP 服务客户端套接字数据发送类型</typeparam>
    public abstract class ClientLoadRoute<ClientSocketSenderType>
        where ClientSocketSenderType : ClientSocketSenderBase
    {
        
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        public abstract ClientSocketBase Socket { get; }
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        public abstract ClientSocketSenderType Sender { get; }
        /// <summary>
        /// 设置 TCP 客户端套接字事件
        /// </summary>
        public abstract void OnSetSocket();
        /// <summary>
        /// 移除 TCP 服务客户端套接字
        /// </summary>
        /// <param name="socket">TCP 服务客户端套接字</param>
        public abstract void OnDisposeSocket(ClientSocketBase socket);
        /// <summary>
        /// 释放套接字
        /// </summary>
        public abstract void DisposeSocket();
        /// <summary>
        /// 尝试创建第一个套接字
        /// </summary>
        public abstract void TryCreateSocket();
        /// <summary>
        /// 服务更新
        /// </summary>
        /// <param name="serverSet">TCP 服务信息集合</param>
        public abstract void OnServerChange(TcpRegister.ServerSet serverSet);
    }
}
