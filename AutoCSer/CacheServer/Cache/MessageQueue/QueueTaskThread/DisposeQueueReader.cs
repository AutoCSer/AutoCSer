using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 释放队列数据 读文件
    /// </summary>
    internal sealed class DisposeQueueReader : Node
    {
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        private readonly File.QueueReader reader;
        /// <summary>
        /// 释放队列数据 读文件
        /// </summary>
        /// <param name="reader"></param>
        internal DisposeQueueReader(File.QueueReader reader) : base(reader.Node)
        {
            this.reader = reader;
        }
        /// <summary>
        /// 释放队列数据 读文件
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            reader.Dispose();
            return LinkNext;
        }
    }
}
