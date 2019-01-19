using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 套接字链表
    /// </summary>
    internal sealed class SocketLink : AutoCSer.Threading.Link<SocketLink>
    {
        /// <summary>
        /// 套接字
        /// </summary>
        internal System.Net.Sockets.Socket Socket;
        /// <summary>
        /// 释放套接字
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SocketLink Cancel()
        {
#if !DotNetStandard
            if (Socket != null) Socket.Dispose();
#endif
            return LinkNext;
        }
        /// <summary>
        /// 创建 HTTP 服务端套接字
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SocketLink Start(AutoCSer.Net.Http.SslServer server)
        {
            (Http.SslSocket.Pool.Pop() ?? new Http.SslSocket()).Start(server, ref Socket);
            return LinkNext;
        }
        /// <summary>
        /// 创建 HTTP 服务端套接字
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SocketLink Start(AutoCSer.Net.Http.Server server)
        {
            (Http.Socket.Pool.Pop() ?? new Http.Socket()).Start(server, ref Socket);
            return LinkNext;
        }
    }
}
