using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务链表节点
    /// </summary>
    /// <typeparam name="T">任务节点类型</typeparam>
    public abstract class TaskLinkNode<T> : Link<T>
        where T : TaskLinkNode<T>
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        public abstract void RunTask();
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RunTask(ref T next)
        {
            next = LinkNext;
            LinkNext = null;
            RunTask();
        }
        /// <summary>
        /// 系统线程池调用处理
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ThreadPoolCall(object state)
        {
            try
            {
                RunTask();
            }
            catch (Exception error)
            {
                AutoCSer.LogHelper.Exception(error);
            }
        }
    }
    /// <summary>
    /// 任务链表节点
    /// </summary>
    public abstract class TaskLinkNode : TaskLinkNode<TaskLinkNode>
    {
    }
}
