using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息分发 读取配置
    /// </summary>
    public class DistributionConfig : ReaderConfig
    {
        /// <summary>
        /// 处理超时秒数，默认为 60 秒
        /// </summary>
        public int TimeoutSeconds = 60;
    }
}
