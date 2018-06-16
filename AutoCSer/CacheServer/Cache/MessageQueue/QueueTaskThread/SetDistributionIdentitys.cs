using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 设置当前读取数据标识
    /// </summary>
    internal sealed class SetDistributionIdentitys : Node
    {
        /// <summary>
        /// 消息分发 读文件
        /// </summary>
        private readonly DistributionFileReader reader;
        /// <summary>
        /// 确认已完成消息标识
        /// </summary>
        private readonly ulong[] identitys;
        /// <summary>
        /// 设置当前读取数据标识
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="identitys">确认已完成消息标识</param>
        internal SetDistributionIdentitys(DistributionFileReader reader, ulong[] identitys) : base(reader.Node)
        {
            this.reader = reader;
            this.identitys = identitys;
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            reader.SetIdentity(identitys);
            return LinkNext;
        }
    }
}
