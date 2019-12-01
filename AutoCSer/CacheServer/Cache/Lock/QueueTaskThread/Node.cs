using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Lock.QueueTaskThread
{
    /// <summary>
    /// 锁操作任务
    /// </summary>
    internal abstract class Node : AutoCSer.Threading.Link<Node>
    {
        /// <summary>
        /// 锁节点
        /// </summary>
        internal readonly Lock.Node Lock;
        /// <summary>
        /// 锁操作任务
        /// </summary>
        /// <param name="node">锁节点</param>
        protected Node(Lock.Node node)
        {
            Lock = node;
        }
        /// <summary>
        /// 锁任务操作
        /// </summary>
        internal abstract void RunTask();
        /// <summary>
        /// 锁任务操作
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RunTask(ref Node next)
        {
            next = LinkNext;
            LinkNext = null;
            RunTask();
        }
    }
}
