using System;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息分发 读取配置
    /// </summary>
    public sealed class DistributionConsumerConfig : Cache.MessageQueue.DistributionConfig
    {
        /// <summary>
        /// 服务端单次发送数据数量，默认为 8K
        /// </summary>
        public int LoopSendCount = 8 << 10;
    }
}
