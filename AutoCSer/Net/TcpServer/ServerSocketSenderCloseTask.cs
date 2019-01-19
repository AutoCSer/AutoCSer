using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务套接字数据发送关闭任务
    /// </summary>
    internal sealed class ServerSocketSenderCloseTask : ServerCall
    {
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        internal ServerSocketSenderBase Sender;
        /// <summary>
        /// 调用处理
        /// </summary>
        public override void Call()
        {
            Sender.CallOnCloseTask();
        }
    }
}
