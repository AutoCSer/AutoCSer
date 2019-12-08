using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务队列链表，用于低优先级任务
    /// </summary>
    /// <typeparam name="taskType">任务节点类型</typeparam>
    public sealed class TaskQueueLink<taskType>
        where taskType : TaskLinkNode<taskType>
    {
        /// <summary>
        /// 任务队列
        /// </summary>
        private readonly AutoCSer.Threading.TaskQueue<taskType> queue;
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
        internal TaskQueueLink(AutoCSer.Threading.TaskQueue<taskType> queue)
        {
            this.queue = queue;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        public void Add(taskType node)
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
        public void RunTask()
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
}
