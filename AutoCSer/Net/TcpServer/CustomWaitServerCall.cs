using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 自定义 TCP 服务器端同步调用任务
    /// </summary>
    internal sealed class CustomWaitServerCall : ServerCall
    {
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        internal ServerSocketSenderBase Sender;
        /// <summary>
        /// 自定义任务
        /// </summary>
        internal Action Task;
        /// <summary>
        /// 等待锁
        /// </summary>
        internal AutoCSer.Threading.AutoWaitHandle Wait;
        /// <summary>
        /// 调用处理
        /// </summary>
        public override void Call()
        {
            try
            {
                Task();
            }
            catch (Exception error)
            {
                Sender.VirtualAddLog(error);
            }
            finally { Wait.Set(); }
        }
    }
}
