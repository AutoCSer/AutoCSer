using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 分发消息状态
    /// </summary>
    internal enum DistributionMessageState : byte
    {
        /// <summary>
        /// 等待发送
        /// </summary>
        None,
        /// <summary>
        /// 已发送
        /// </summary>
        Sended,
        /// <summary>
        /// 超时追加到文件
        /// </summary>
        AppendFile,
        /// <summary>
        /// 已消费
        /// </summary>
        Consumed,
    }
}
