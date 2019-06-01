using System;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 内部服务客户端套接字数据发送
    /// </summary>
    public sealed class ClientSocketSender : TcpServer.ClientSocketSender<ServerAttribute>
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
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(this, AutoCSer.Threading.Thread.CallType.TcpOpenClientSocketSenderBuildOutput);
            //BuildOutputMainWaitHandle.Set(0);
            //BuildOutputOtherWaitHandle.Set(0);
            //SendLock = new object();
            //AutoCSer.Threading.ThreadPool.TinyBackground.FastStart((Action)BuildOutputMain, Threading.Thread.CallType.Action);
            //AutoCSer.Threading.ThreadPool.TinyBackground.FastStart((Action)BuildOutputOther, Threading.Thread.CallType.Action);
        }
    }
}
