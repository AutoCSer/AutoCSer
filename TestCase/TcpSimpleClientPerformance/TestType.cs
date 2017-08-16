using System;

namespace AutoCSer.TestCase.TcpInternalSimpleClientPerformance
{
    /// <summary>
    /// 测试类型
    /// </summary>
    internal enum TestType : byte
    {
        /// <summary>
        /// 服务端异步
        /// </summary>
        Asynchronous,
        /// <summary>
        /// 服务端同步
        /// </summary>
        Synchronous,
        /// <summary>
        /// 服务端队列
        /// </summary>
        Queue,
        /// <summary>
        /// 服务端公共任务池超时切换线程
        /// </summary>
        Timeout,
        /// <summary>
        /// 服务端任务池
        /// </summary>
        TcpTask,
        /// <summary>
        /// 系统线程池
        /// </summary>
        ThreadPool,
        /// <summary>
        /// 客户端多线程
        /// </summary>
        Multithreading,
    }
}
