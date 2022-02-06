using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 低优先级任务队列链表
    /// </summary>
    public sealed class TaskQueueThreadLowPriorityLink : TaskLinkNode
    {
        /// <summary>
        /// 任务队列
        /// </summary>
        private readonly TaskQueueThread<TaskLinkNode> queue;
        /// <summary>
        /// 首节点
        /// </summary>
        private TaskLinkNode head;
        /// <summary>
        /// 尾节点
        /// </summary>
        private TaskLinkNode end;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock queueLock;
        /// <summary>
        /// 任务队列链表节点
        /// </summary>
        /// <param name="queue">任务队列</param>
        internal TaskQueueThreadLowPriorityLink(TaskQueueThread queue)
        {
            this.queue = queue;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Add(TaskLinkNode node)
        {
            if (node != null)
            {
                queueLock.EnterYield();
                add(node);
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        private void add(TaskLinkNode node)
        {
            if (head == null)
            {
                head = end = node;
                queueLock.Exit();
                queue.Add(this);
            }
            else
            {
                end.LinkNext = node;
                end = node;
                queueLock.Exit();
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool CheckAdd(TaskLinkNode node)
        {
            if (node != null)
            {
                queueLock.EnterYield();
                if (node.LinkNext == null)
                {
                    if(node != end)
                    {
                        add(node);
                        return true;
                    }
                    if (head == null)
                    {
                        head = end = node;
                        queueLock.Exit();
                        queue.Add(this);
                        return true;
                    }
                }
                queueLock.Exit();
            }
            return false;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask()
        {
            TaskLinkNode node = head, next = head.LinkNext;
            if (next == null)
            {
                queueLock.EnterYield();
                head = next = head.LinkNext;
                queueLock.Exit();
                node.LinkNext = null;
                try
                {
                    node.RunTask();
                }
                finally
                {
                    if (next != null) queue.Add(this);
                }
            }
            else
            {
                head = next;
                node.LinkNext = null;
                try
                {
                    node.RunTask();
                }
                finally { queue.Add(this); }
            }
        }
    }
}
