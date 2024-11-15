﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 消息队列读取操作任务
    /// </summary>
    internal abstract class Node : AutoCSer.Threading.TaskLinkNode
    {
        /// <summary>
        /// 消息队列节点
        /// </summary>
        internal readonly MessageQueue.Node MessageQueue;
        /// <summary>
        /// 消息队列读取操作任务
        /// </summary>
        /// <param name="messageQueue">消息队列节点</param>
        protected Node(MessageQueue.Node messageQueue)
        {
            MessageQueue = messageQueue;
        }
        /// <summary>
        /// 添加到消息队列读取操作队列线程
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddQueueTaskLinkThread()
        {
            QueueTaskLinkThread.Add(this);
        }
        ///// <summary>
        ///// 添加到消息队列读取操作队列线程
        ///// </summary>
        ///// <param name="end"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void AddQueueTaskLinkThread(Node end)
        //{
        //    QueueTaskLinkThread.Add(this, end);
        //}

        /// <summary>
        /// 消息队列读取操作队列线程
        /// </summary>
        internal static readonly AutoCSer.Threading.TaskQueueThread QueueTaskLinkThread = new AutoCSer.Threading.TaskQueueThread();
    }
}
