using System;
using System.Net.Sockets;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 套接字数据发送
    /// </summary>
    public abstract class Sender
    {
        /// <summary>
        /// TCP 服务套接字
        /// </summary>
        internal readonly Socket Socket;

        /// <summary>
        /// 是否已经释放缓冲区
        /// </summary>
        protected bool isFree;
        /// <summary>
        /// 是否已经关闭并释放资源
        /// </summary>
        protected bool isClose;
        /// <summary>
        /// TCP 套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 服务套接字</param>
        internal Sender(Socket socket)
        {
            Socket = socket;
        }
    }
}
