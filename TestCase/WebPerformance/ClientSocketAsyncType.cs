using System;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 客户端异步操作类型
    /// </summary>
    internal enum ClientSocketAsyncType : byte
    {
        /// <summary>
        /// 建立连接
        /// </summary>
        Connect,
        /// <summary>
        /// 发送数据
        /// </summary>
        Send,
        /// <summary>
        /// 接收数据
        /// </summary>
        Receive,
    }
}
