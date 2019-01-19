using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 客户端任务类型
    /// </summary>
    public enum ClientTaskType : byte
    {
        /// <summary>
        /// 非任务同步阻塞模式，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据。
        /// 如果不小心在回调处理事件中又使用同步的方式调用了服务端，那么客户端死锁是必然的。
        /// </summary>
        Synchronous,
        /// <summary>
        /// 系统线程池
        /// </summary>
        ThreadPool,
        /// <summary>
        /// 公用任务池，默认最大并行数量为 AutoCSer.Threading.Pub.CpuCount * 32，定时检测任务超时调度线程（默认为 10 毫秒）
        /// </summary>
        Timeout,
        /// <summary>
        /// TCP 任务池，适用于 CPU 计算类型任务，默认最大并行数量为 AutoCSer.Threading.Pub.CpuCount，定时检测任务超时调度线程（默认为 10 毫秒）
        /// </summary>
        TcpTask,
        /// <summary>
        /// TCP 任务队列，适应于无阻塞的快速任务处理。
        /// </summary>
        TcpQueue,
    }
}
