using System;

namespace AutoCSer.TestCase.TcpInternalStreamClientPerformance
{
    /// <summary>
    /// 测试类型
    /// </summary>
    internal enum ClientTestType : byte
    {
        /// <summary>
        /// 客户端异步
        /// </summary>
        Asynchronous,
        /// <summary>
        /// 客户端同步
        /// </summary>
        Synchronous,
        /// <summary>
        /// 客户端异步任务
        /// </summary>
        TaskAsync,
        /// <summary>
        /// 客户端 await
        /// </summary>
        Awaiter,
    }
}
