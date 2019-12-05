using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务链表节点，带线程切换检测时间
    /// </summary>
    /// <typeparam name="taskType">任务节点类型</typeparam>
    public abstract class TimestampTaskLinkNode<taskType> : TaskLinkNode<taskType>, ILinkTask
        where taskType : TimestampTaskLinkNode<taskType>
    {
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        internal long TaskTimestamp;
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        long ILinkTask.LinkTaskTimestamp
        {
            get { return TaskTimestamp; }
            set { TaskTimestamp = value; }
        }
        /// <summary>
        /// 下一个任务节点
        /// </summary>
        ILinkTask ILinkTask.NextLinkTask
        {
            get { return LinkNext; }
            set { LinkNext = (taskType)value; }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="next">下一个任务节点</param>
        void ILinkTask.RunTask(ref ILinkTask next)
        {
            next = LinkNext;
            LinkNext = null;
            RunTask();
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="next"></param>
        /// <param name="currentTaskTimestamp"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RunTask(ref taskType next, ref long currentTaskTimestamp)
        {
            next = LinkNext;
            currentTaskTimestamp = TaskTimestamp;
            LinkNext = null;
            RunTask();
        }
    }
}
