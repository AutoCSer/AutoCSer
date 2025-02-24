﻿using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
{
    /// <summary>
    /// 链表任务队列节点
    /// </summary>
    public abstract class LinkQueueTaskNode : AutoCSer.Threading.Link<LinkQueueTaskNode>
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="connection"></param>
        internal abstract void RunLinkQueueTask(ref DbConnection connection);
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RunLinkQueueTask(ref DbConnection connection, ref LinkQueueTaskNode next)
        {
            next = LinkNext;
            LinkNext = null;
            RunLinkQueueTask(ref connection);
        }
    }
    /// <summary>
    /// 链表任务队列节点
    /// </summary>
    internal abstract class LinkQueueTaskNode<nodeType> : LinkQueueTaskNode
    {
        ///// <summary>
        ///// 链表节点池
        ///// </summary>
        //internal new static class YieldPool
        //{
        //    /// <summary>
        //    /// 链表节点池
        //    /// </summary>
        //    internal static LinkQueueTaskNode.YieldPool Default;
        //    /// <summary>
        //    /// 清除缓存数据
        //    /// </summary>
        //    /// <param name="count">保留缓存数据数量</param>
        //    private static void clearCache(int count)
        //    {
        //        Default.ClearCache(count);
        //    }

        //    static YieldPool()
        //    {
        //        AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(YieldPool));
        //    }
        //}
    }
}
