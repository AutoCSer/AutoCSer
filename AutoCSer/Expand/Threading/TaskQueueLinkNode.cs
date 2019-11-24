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
        private TaskQueueLink like;
        /// <summary>
        /// 任务队列链表节点
        /// </summary>
        /// <param name="like">任务队列链表</param>
        internal TaskQueueLinkNode(TaskQueueLink like)
        {
            this.like = like;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask()
        {
            like.RunTask();
        }
    }
}
