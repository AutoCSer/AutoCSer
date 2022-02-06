using System;
using AutoCSer.Extensions;

namespace AutoCSer.Net.RawSocketListener
{
    /// <summary>
    /// 队列线程
    /// </summary>
    internal sealed class QueueTask : AutoCSer.Threading.TaskQueueThreadBase<Buffer>, IDisposable
    {
        /// <summary>
        /// 数据包处理委托
        /// </summary>
        private readonly Action<Buffer> onPacket;
        /// <summary>
        /// 日志处理
        /// </summary>
        private readonly AutoCSer.ILog log;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile bool isDisposed;
        /// <summary>
        /// 队列线程
        /// </summary>
        /// <param name="onPacket">数据包处理委托</param>
        /// <param name="log">日志处理</param>
        internal QueueTask(Action<Buffer> onPacket, AutoCSer.ILog log)
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
            WaitHandle.Set();
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(Buffer value)
        {
            QueueLock.EnterYield();
            if (head == null)
            {
                end = value;
                head = value;
                QueueLock.Exit();
                WaitHandle.Set();
            }
            else
            {
                end.LinkNext = value;
                end = value;
                QueueLock.Exit();
            }
        }
        /// <summary>
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                WaitHandle.Wait();
                QueueLock.EnterYield();
                Buffer value = head;
                end = null;
                head = null;
                QueueLock.Exit();
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            value.OnPacket(ref value, onPacket);
                        }
                        break;
                    }
                    catch (Exception error)
                    {
                        log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                }
                while (true);
            }
            while (!isDisposed);
        }
    }
}
