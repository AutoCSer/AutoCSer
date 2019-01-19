using System;
using AutoCSer.Extension;

namespace AutoCSer.Net.RawSocketListener
{
    /// <summary>
    /// 队列线程
    /// </summary>
    internal sealed class QueueTask : AutoCSer.Threading.QueueTaskThread<Buffer>, IDisposable
    {
        /// <summary>
        /// 数据包处理委托
        /// </summary>
        private readonly Action<Buffer> onPacket;
        /// <summary>
        /// 日志处理
        /// </summary>
        private readonly AutoCSer.Log.ILog log;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile bool isDisposed;
        /// <summary>
        /// 队列线程
        /// </summary>
        /// <param name="onPacket">数据包处理委托</param>
        /// <param name="log">日志处理</param>
        internal QueueTask(Action<Buffer> onPacket, AutoCSer.Log.ILog log)
        {
            this.onPacket = onPacket;
            this.log = log;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            waitHandle.Set();
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(Buffer value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (head == null)
            {
                end = value;
                head = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                waitHandle.Set();
            }
            else
            {
                end.LinkNext = value;
                end = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
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
                Buffer value = head;
                end = null;
                head = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            onPacket(value);
                            value = value.LinkNext;
                        }
                        break;
                    }
                    catch (Exception error)
                    {
                        log.Add(Log.LogType.Error, error);
                    }
                    value = value.LinkNext;
                }
                while (true);
            }
            while (!isDisposed);
        }
    }
}
