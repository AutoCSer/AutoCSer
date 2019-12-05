using AutoCSer.Extension;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务链表节点
    /// </summary>
    /// <typeparam name="taskType">任务节点类型</typeparam>
    public abstract class TaskLinkNode<taskType> : Link<taskType>
        where taskType : TaskLinkNode<taskType>
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
        internal void RunTask(ref taskType next)
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
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
        }
    }
}
