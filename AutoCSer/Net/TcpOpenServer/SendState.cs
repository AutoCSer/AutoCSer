using System;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// 发送数据状态
    /// </summary>
    internal enum SendState : byte
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 同步
        /// </summary>
        Synchronize,
        /// <summary>
        /// 异步
        /// </summary>
        Asynchronous,
    }
}
