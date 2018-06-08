using System;

namespace AutoCSer.CacheServer.Cache.Lock.QueueTaskThread
{
    /// <summary>
    /// 申请锁任务
    /// </summary>
    internal sealed class TryEnter : Node
    {
        /// <summary>
        /// 锁链表节点
        /// </summary>
        internal LinkNode LinkNode;
        /// <summary>
        /// 申请锁任务
        /// </summary>
        /// <param name="node"></param>
        internal TryEnter(Lock.Node node) : base(node) { }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            Lock.TryEnter(LinkNode);
            return LinkNext;
        }
    }
}
