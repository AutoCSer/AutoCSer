using AutoCSer.Extension;
using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 队列链表任务线程
    /// </summary>
    /// <typeparam name="taskType"></typeparam>
    internal class QueueTaskLinkThread<taskType> : QueueTaskThread<taskType>
        where taskType : TaskLinkNode<taskType>
    {
        /// <summary>
        /// 队列任务线程
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="isStart">是否启动线程</param>
        internal QueueTaskLinkThread(bool isBackground = true, bool isStart = true) : base(isBackground, isStart) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(taskType value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (head == null)
            {
                end = value;
                head = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                WaitHandle.Set();
            }
            else
            {
                end.LinkNext = value;
                end = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        internal void Add(taskType head, taskType end)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (this.head == null)
            {
                this.end = end;
                this.head = head;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                WaitHandle.Set();
            }
            else
            {
                this.end.LinkNext = head;
                this.end = end;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
        }
        /// <summary>
        /// 消息队列读取操作任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                WaitHandle.Wait();
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                taskType value = head;
                end = null;
                head = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                do
                {
                    try
                    {
                        do
                        {
                            value.RunTask(ref value);
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                    }
                }
                while (value != null);
            }
            while (true);
        }
    }
}
