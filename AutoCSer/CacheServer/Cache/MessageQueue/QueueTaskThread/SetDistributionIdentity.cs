using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 设置当前读取数据标识
    /// </summary>
    internal sealed class SetDistributionIdentity : Node
    {
        /// <summary>
        /// 消息分发 读文件
        /// </summary>
        private readonly DistributionFileReader reader;
        /// <summary>
        /// 确认已完成消息标识
        /// </summary>
        private readonly ulong identity;
        /// <summary>
        /// 设置当前读取数据标识
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="identity">确认已完成消息标识</param>
        internal SetDistributionIdentity(DistributionFileReader reader, ulong identity) : base(reader.Node)
        {
            this.reader = reader;
            this.identity = identity;
        }
        /// <summary>
        /// 消息超时追加到文件完毕
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool OnAppendFile(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            if (value.Type == Net.TcpServer.ReturnType.Success) reader.SetIdentity(identity);
            return true;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            reader.SetIdentity(identity);
            return LinkNext;
        }
    }
}
