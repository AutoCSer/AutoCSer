using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 客户端套接字事件参数
    /// </summary>
    public sealed class ClientSocketEventParameter
    {
        /// <summary>
        /// TCP 服务客户端创建器
        /// </summary>
        public readonly ClientSocketCreator Creator;
        /// <summary>
        /// TCP 服务客户端套接字
        /// </summary>
        public readonly ClientSocketBase Socket;
        /// <summary>
        /// 事件类型
        /// </summary>
        public readonly EventType Type;
        /// <summary>
        /// TCP 客户端套接字事件参数
        /// </summary>
        /// <param name="creator">TCP 服务客户端创建器</param>
        /// <param name="socket">TCP 服务客户端套接字</param>
        /// <param name="type">事件类型</param>
        internal ClientSocketEventParameter(ClientSocketCreator creator, ClientSocketBase socket, EventType type)
        {
            Creator = creator;
            Socket = socket;
            Type = type;
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// 套接字被关闭
            /// </summary>
            SocketDisposed,
            /// <summary>
            /// 设置新的套接字
            /// </summary>
            SetSocket,
            ///// <summary>
            ///// 套接字关闭
            ///// </summary>
            //SocketClosed,
        }
    }
}
