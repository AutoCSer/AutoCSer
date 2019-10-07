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
        /// 客户端多线程
        /// </summary>
        Multithreading,
    }
}
