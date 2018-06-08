using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 消息队列读取操作任务
    /// </summary>
    internal abstract class Node : AutoCSer.Threading.Link<Node>
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
        /// 读取任务操作
        /// </summary>
        /// <returns></returns>
        internal abstract Node RunTask();
    }
}
