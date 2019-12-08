using AutoCSer.Extension;
using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 队列链表任务线程
    /// </summary>
    /// <typeparam name="taskType"></typeparam>
    public class QueueTaskLinkThread<taskType> : QueueTaskThread<taskType>
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
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool CheckAdd(taskType value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (value.LinkNext == null && value != end)
            {
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
                return true;
            }
            System.Threading.Interlocked.Exchange(ref queueLock, 0);
            return false;
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

        /// <summary>
        /// 低优先级任务队列链表
        /// </summary>
        internal sealed class LowPriorityLink
        {
            /// <summary>
            /// 任务队列
            /// </summary>
            private readonly QueueTaskLinkThread<taskType> queue;
            /// <summary>
            /// 首节点
            /// </summary>
            private taskType head;
            /// <summary>
            /// 尾节点
            /// </summary>
            private taskType end;
            /// <summary>
            /// 弹出节点访问锁
            /// </summary>
            private int queueLock;
            /// <summary>
            /// 任务队列链表节点
            /// </summary>
            /// <param name="queue">任务队列</param>
            internal LowPriorityLink(QueueTaskLinkThread<taskType> queue)
            {
                this.queue = queue;
            }
            /// <summary>
            /// 添加任务
            /// </summary>
            /// <param name="node"></param>
            internal void Add(taskType node)
            {
                if (node != null)
                {
                    while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                    if (head == null)
                    {
                        head = end = node;
                        System.Threading.Interlocked.Exchange(ref queueLock, 0);
                        queue.Add(this);
                    }
                    else
                    {
                        end.LinkNext = node;
                        end = node;
                        System.Threading.Interlocked.Exchange(ref queueLock, 0);
                    }
                }
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            internal void RunTask()
            {
                taskType node = head, next = head.LinkNext;
                if (next == null)
                {
                    while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                    head = next = head.LinkNext;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                }
                else head = next;
                try
                {
                    node.RunTask();
                }
                finally
                {
                    if (next != null) queue.Add(this);
                }
            }
        }
        /// <summary>
        /// 添加低优先级任务队列链表
        /// </summary>
        /// <param name="link"></param>
        internal virtual void Add(LowPriorityLink link)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 创建低优先级任务队列链表
        /// </summary>
        /// <returns></returns>
        internal LowPriorityLink CreateLink()
        {
            return new LowPriorityLink(this);
        }
    }
}
