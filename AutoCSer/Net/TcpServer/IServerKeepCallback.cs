using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端异步保持调用
    /// </summary>
    internal interface IServerKeepCallback
    {
        /// <summary>
        /// 取消保持调用
        /// </summary>
        void CancelKeep();
    }
}
