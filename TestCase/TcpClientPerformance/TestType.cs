using System;

namespace AutoCSer.TestCase.TcpInternalClientPerformance
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
        /// 混合测试 [ Asynchronous + Synchronous + Queue + TcpTask + Timeout]
        /// </summary>
        Mixing,
        /// <summary>
        /// 客户端同步
        /// </summary>
        ClientSynchronous,
        /// <summary>
        /// 客户端异步任务
        /// </summary>
        ClientTaskAsync,
        /// <summary>
        /// 客户端 await
        /// </summary>
        ClientAwaiter,
        /// <summary>
        /// 无限制线程池
        /// </summary>
        NewThread,
        /// <summary>
        /// 系统线程池
        /// </summary>
        ThreadPool,
        /// <summary>
        /// 服务端公共任务池轮询
        /// </summary>
        Poll,
        /// <summary>
        /// 保持回调（注册模式 / 消息模式）
        /// </summary>
        Register,
        /// <summary>
        /// 自定义序列化（作弊模式：非函数调用模式）
        /// </summary>
        CustomSerialize,
    }
}
