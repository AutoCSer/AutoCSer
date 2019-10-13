using System;

namespace AutoCSer.Net.TcpServer
{
#if NOJIT
    /// <summary>
    /// 队列获取
    /// </summary>
    /// <typeparam name="queueType">队列类型</typeparam>
    public class QueueGetter<queueType> : IServerCallQueue
#else
    /// <summary>
    /// 队列获取
    /// </summary>
    /// <typeparam name="queueType">队列类型</typeparam>
    /// <typeparam name="queueKeyType">关键字类型</typeparam>
    public class QueueGetter<queueType, queueKeyType> : IServerCallQueue<queueKeyType>
#endif
        where queueType : ServerCallCanDisposableQueue
    {
        /// <summary>
        /// 队列管理器
        /// </summary>
        private readonly QueueManager<queueType> manager;
        /// <summary>
        /// 队列获取
        /// </summary>
        /// <param name="manager">队列管理器</param>
        public QueueGetter(QueueManager<queueType> manager)
        {
            this.manager = manager;
        }
#if NOJIT
        /// <summary>
        /// 获取 TCP 服务器端同步调用队列
        /// </summary>
        /// <typeparam name="queueKeyType">关键字类型</typeparam>
        /// <param name="sender">TCP 服务套接字数据发送</param>
        /// <param name="queueKey">关键字</param>
        /// <returns>TCP 服务器端同步调用队列</returns>
        public virtual ServerCallQueue Get<queueKeyType>(ServerSocketSenderBase sender, ref queueKeyType queueKey)
#else
        /// <summary>
        /// 获取 TCP 服务器端同步调用队列
        /// </summary>
        /// <param name="sender">TCP 服务套接字数据发送</param>
        /// <param name="queueKey">关键字</param>
        /// <returns>TCP 服务器端同步调用队列</returns>
        public virtual ServerCallQueue Get(ServerSocketSenderBase sender, ref queueKeyType queueKey)
#endif
        {
            if (queueKey == null) return manager.Queues[0];
            return manager.Queues[(uint)queueKey.GetHashCode() % manager.Queues.Length];
        }
    }
}
