using System;
using AutoCSer.Extension;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenStreamServer
{
    /// <summary>
    /// 套接字链表
    /// </summary>
    internal sealed class SocketLink : AutoCSer.Threading.Link<SocketLink>
    {
        /// <summary>
        /// 套接字
        /// </summary>
        internal Socket Socket;
        /// <summary>
        /// 释放套接字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void DisposeSocket()
        {
#if !DotNetStandard
            Socket.Dispose();
#endif
            Socket = null;
        }
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
        /// 创建 TCP 服务端套接字
        /// </summary>
        /// <param name="server"></param>
        /// <param name="serverSocket"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SocketLink Start(Server server, ref ServerSocket serverSocket)
        {
            serverSocket = new ServerSocket(server, ref Socket);
            serverSocket.Start();
            serverSocket = null;
            return LinkNext;
        }
    }
}
