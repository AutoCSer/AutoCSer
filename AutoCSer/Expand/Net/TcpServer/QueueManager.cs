using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 队列管理器
    /// </summary>
    /// <typeparam name="queueType">队列类型</typeparam>
    public abstract class QueueManager<queueType> : IServerCallQueueSet
        where queueType : ServerCallCanDisposableQueue
    {
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        protected internal readonly queueType[] Queues;
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly AutoCSer.ILog log;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        protected volatile bool isDisposed;
        /// <summary>
        /// 队列管理器
        /// </summary>
        /// <param name="queueCount"></param>
        /// <param name="log"></param>
        protected QueueManager(int queueCount, AutoCSer.ILog log)
        {
            this.log = log ?? AutoCSer.LogHelper.Default;
            Queues = new queueType[queueCount <= 0 ? AutoCSer.Common.ProcessorCount : queueCount];
        }
        /// <summary>
        /// 队列管理器
        /// </summary>
        /// <param name="queueCount">队列数量，默认为 0 标识 CPU 核心数量</param>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="log">日志接口</param>
        public QueueManager(int queueCount = 0, bool isBackground = true, AutoCSer.ILog log = null) : this(queueCount, log)
        {
            for (int index = 0; index != Queues.Length; ++index) Queues[index] = createQueue(isBackground);
        }
        /// <summary>
        /// 创建 TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <returns>TCP 服务器端同步调用队列处理</returns>
        protected abstract queueType createQueue(bool isBackground);
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (!isDisposed)
            {
                if (Queues != null)
                {
                    foreach (ServerCallCanDisposableQueue queue in Queues)
                    {
                        if (queue == null) break;
                        queue.Dispose();
                    }
                    Array.Clear(Queues, 0, Queues.Length);
                }
                isDisposed = true;
            }
        }
#if NOJIT
        /// <summary>
        /// 获取 TCP 服务器端同步调用队列接口
        /// </summary>
        /// <typeparam name="queueKeyType">关键字类型</typeparam>
        /// <returns>TCP 服务器端同步调用队列接口</returns>
        public virtual IServerCallQueue Get<queueKeyType>()
        {
            return new QueueGetter<queueType>(this);
        }
#else
        /// <summary>
        /// 获取 TCP 服务器端同步调用队列接口
        /// </summary>
        /// <typeparam name="queueKeyType">关键字类型</typeparam>
        /// <returns>TCP 服务器端同步调用队列接口</returns>
        public virtual IServerCallQueue<queueKeyType> Get<queueKeyType>()
        {
            return new QueueGetter<queueType, queueKeyType>(this);
        }
#endif
    }
    /// <summary>
    /// 默认队列管理器
    /// </summary>
    public class QueueManager : QueueManager<ServerCallCanDisposableQueue>
    {
        /// <summary>
        /// 队列管理器
        /// </summary>
        /// <param name="queueCount">队列数量，默认为 0 标识 CPU 核心数量</param>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="log">日志接口</param>
        public QueueManager(int queueCount = 0, bool isBackground = true, AutoCSer.ILog log = null) : base(queueCount, isBackground, log) { }
        /// <summary>
        /// 创建 TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <returns>TCP 服务器端同步调用队列处理</returns>
        protected override ServerCallCanDisposableQueue createQueue(bool isBackground)
        {
            return new ServerCallCanDisposableQueue(isBackground, log);
        }
    }
    /// <summary>
    /// 默认 TCP 自定义队列数据
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    public abstract class QueueManager<keyType, valueType> : QueueManager<ServerCallCanDisposableQueue<keyType, valueType>>
        where keyType : IEquatable<keyType>
        where valueType : class
    {
        /// <summary>
        /// 最大数据数量
        /// </summary>
        private readonly int maxDataCount;
        /// <summary>
        /// 队列管理器
        /// </summary>
        /// <param name="maxDataCount">最大数据数量</param>
        /// <param name="queueCount">队列数量，默认为 0 标识 CPU 核心数量</param>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="log">日志接口</param>
        public QueueManager(int maxDataCount, int queueCount = 0, bool isBackground = true, AutoCSer.ILog log = null) : base(queueCount, log)
        {
            if (maxDataCount <= 0) throw new IndexOutOfRangeException();
            this.maxDataCount = maxDataCount;
            for (int index = 0; index != Queues.Length; ++index) Queues[index] = createQueue(isBackground);
        }
    }
}
