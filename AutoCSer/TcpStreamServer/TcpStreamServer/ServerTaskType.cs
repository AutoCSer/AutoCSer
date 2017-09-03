using System;

namespace AutoCSer.Net.TcpStreamServer
{
    /// <summary>
    /// 服务端任务类型
    /// </summary>
    public enum ServerTaskType : byte
    {
        /// <summary>
        /// 非任务同步阻塞模式，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据。
        /// </summary>
        Synchronous,
        /// <summary>
        /// TCP 任务队列，适用于无阻塞的快速任务处理。
        /// </summary>
        TcpQueue,
        /// <summary>
        /// 服务独占任务队列，适用于无阻塞的快速任务处理。
        /// </summary>
        Queue,
    }
}
