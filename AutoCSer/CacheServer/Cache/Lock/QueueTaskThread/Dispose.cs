using System;

namespace AutoCSer.CacheServer.Cache.Lock.QueueTaskThread
{
    /// <summary>
    /// 释放锁节点
    /// </summary>
    internal sealed class Dispose : Node
    {
        /// <summary>
        /// 释放锁节点
        /// </summary>
        /// <param name="node"></param>
        internal Dispose(Lock.Node node) : base(node) { }
        /// <summary>
        /// 释放锁节点
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            Lock.Dispose();
            return LinkNext;
        }
    }
}