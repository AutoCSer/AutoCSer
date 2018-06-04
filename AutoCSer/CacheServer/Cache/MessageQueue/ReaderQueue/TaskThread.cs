using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue.ReaderQueue
{
    /// <summary>
    /// 消息队列读取操作队列线程
    /// </summary>
    internal sealed class TaskThread : AutoCSer.Threading.QueueTaskThread<Node>
    {
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(Node value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (head == null)
            {
                end = value;
                head = value;
                queueLock = 0;
                waitHandle.Set();
            }
            else
            {
                end.LinkNext = value;
                end = value;
                queueLock = 0;
            }
        }
        /// <summary>
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                waitHandle.Wait();
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                Node value = head;
                end = null;
                head = null;
                queueLock = 0;
                do
                {
                    try
                    {
                        do
                        {
                            value = value.RunTask();
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        value.MessageQueue.Cache.TcpServer.AddLog(error);
                    }
                    value = value.LinkNext;
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// 消息队列读取操作队列线程
        /// </summary>
        internal static readonly TaskThread Default = new TaskThread();
    }
}
