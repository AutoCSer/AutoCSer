using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Lock.QueueTaskThread
{
    /// <summary>
    /// 锁操作任务
    /// </summary>
    internal abstract class Node : AutoCSer.Threading.TaskLinkNode
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
        /// 添加到消息队列读取操作队列线程
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddQueueTaskLinkThread()
        {
            queueTaskLinkThread.Add(this);
        }

        /// <summary>
        /// 锁操作队列线程
        /// </summary>
        private static readonly AutoCSer.Threading.TaskQueueThread queueTaskLinkThread = new AutoCSer.Threading.TaskQueueThread();
    }
}
