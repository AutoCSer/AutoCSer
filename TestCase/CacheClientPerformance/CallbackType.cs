using System;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// 测试回调类型
    /// </summary>
    internal enum CallbackType : byte
    {
        /// <summary>
        /// 异步
        /// </summary>
        Asynchronous,
        /// <summary>
        /// 同步
        /// </summary>
        Synchronous,
        /// <summary>
        /// await
        /// </summary>
        Awaiter,

        /// <summary>
        /// 线程池
        /// </summary>
        ThreadPool,
        /// <summary>
        /// IO 线程同步
        /// </summary>
        SynchronousStream,
    }
}
