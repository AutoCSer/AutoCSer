using System;
using AutoCSer.Extension;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public sealed class TaskQueue : IDisposable
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        private AutoCSer.Threading.Thread.AutoWaitHandle waitHandle;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 队列头部
        /// </summary>
        private TaskQueueNode head;
        /// <summary>
        /// 队列尾部
        /// </summary>
        private TaskQueueNode end;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        private int queueLock;
        /// <summary>
        /// 是否已经释放
        /// </summary>
        private volatile bool isDisposed;
        /// <summary>
        /// SQL 队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        public TaskQueue(bool isBackground = true)
        {
            waitHandle.Set(0);
            threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
            if (isBackground) threadHandle.IsBackground = true;
            threadHandle.Start();
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
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        public void Add(TaskQueueNode value)
        {
            if (value.LinkNext != null) throw new InvalidOperationException();

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
        private void run()
        {
            do
            {
                waitHandle.Wait();
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                TaskQueueNode value = head;
                end = null;
                head = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                if (isDisposed) return;

                while (value != null)
                {
                    try
                    {
                        do
                        {
                            value.RunTask();
                            if (isDisposed) return;
                            value = value.LinkNext;
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                    }
                    if (isDisposed) return;
                    value = value.LinkNext;
                }
            }
            while (!isDisposed);
        }
    }
}
