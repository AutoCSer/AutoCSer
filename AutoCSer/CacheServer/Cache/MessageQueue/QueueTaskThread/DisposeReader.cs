using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 释放队列数据 读文件
    /// </summary>
    internal sealed class DisposeReader : Node
    {
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        private readonly IDisposable reader;
        /// <summary>
        /// 释放队列数据 读文件
        /// </summary>
        /// <param name="reader"></param>
        internal DisposeReader(FileReader reader) : base(reader.Node)
        {
            this.reader = reader;
        }
        /// <summary>
        /// 释放消息分发 读文件
        /// </summary>
        /// <param name="reader"></param>
        internal DisposeReader(DistributionFileReader reader) : base(reader.Node)
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
