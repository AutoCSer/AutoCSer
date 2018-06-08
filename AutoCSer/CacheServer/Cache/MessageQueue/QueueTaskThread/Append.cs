using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 添加数据任务
    /// </summary>
    internal sealed class Append : Node
    {
        /// <summary>
        /// 数据缓冲区首节点
        /// </summary>
        private readonly Buffer head;
        /// <summary>
        /// 添加数据任务
        /// </summary>
        /// <param name="head">数据缓冲区首节点</param>
        internal Append(Buffer head) : base(head.Node)
        {
            this.head = head;
        }
        /// <summary>
        /// 读取任务操作
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            Buffer value = head;
            do
            {
                try
                {
                    do
                    {
                        value = value.Append();
                    }
                    while (value != null);
                    break;
                }
                catch (Exception error)
                {
                    MessageQueue.Cache.TcpServer.AddLog(error);
                }
                value = value.LinkNext;
            }
            while (value != null);
            return LinkNext;
        }
    }
}
