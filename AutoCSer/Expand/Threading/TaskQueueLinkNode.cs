using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务队列链表节点
    /// </summary>
    internal sealed class TaskQueueLinkNode : TaskQueueNode
    {
        /// <summary>
        /// 任务队列链表
        /// </summary>
        private TaskQueueLink link;
        /// <summary>
        /// 任务队列链表节点
        /// </summary>
        /// <param name="link">任务队列链表</param>
        internal TaskQueueLinkNode(TaskQueueLink link)
        {
            this.link = link;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask()
        {
            link.RunTask();
        }
    }
}
