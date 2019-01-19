using System;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// SQL 队列处理
    /// </summary>
    internal sealed class TaskQueue : AutoCSer.Threading.QueueTaskThread<QueueTask>
    {
        /// <summary>
        /// SQL 队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        private TaskQueue(bool isBackground) : base(isBackground) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(QueueTask value)
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
                QueueTask value = head;
                end = null;
                head = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                do
                {
                    value = value.RunTask();
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// SQL 队列处理
        /// </summary>
        internal static readonly TaskQueue Default = new TaskQueue(false);
        /// <summary>
        /// SQL 队列处理
        /// </summary>
        internal static readonly TaskQueue Background = new TaskQueue(true);
    }
}
