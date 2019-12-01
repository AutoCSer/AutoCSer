using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务队列节点
    /// </summary>
    public abstract class TaskQueueNode
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal TaskQueueNode LinkNext;
        /// <summary>
        /// 执行任务
        /// </summary>
        public abstract void RunTask();
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RunTask(ref TaskQueueNode next)
        {
            next = LinkNext;
            LinkNext = null;
            RunTask();
        }
    }
}
