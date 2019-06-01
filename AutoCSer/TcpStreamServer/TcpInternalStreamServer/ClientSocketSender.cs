using System;

namespace AutoCSer.Net.TcpInternalStreamServer
{
    /// <summary>
    /// TCP 内部服务客户端套接字数据发送
    /// </summary>
    public sealed class ClientSocketSender : TcpStreamServer.ClientSocketSender<ServerAttribute>
    {
#if !NOJIT
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        internal ClientSocketSender() : base() { }
#endif
        /// <summary>
        /// TCP 服务客户端套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 服务客户端套接字</param>
        internal ClientSocketSender(ClientSocket socket)
            : base(socket)
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, AutoCSer.Threading.Thread.CallType.TcpClientSocketSenderVirtualBuildOutput);
        }
    }
}
