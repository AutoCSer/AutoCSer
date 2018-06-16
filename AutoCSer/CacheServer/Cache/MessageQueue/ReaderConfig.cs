using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 队列数据 读取配置
    /// </summary>
    public class ReaderConfig
    {
        /// <summary>
        /// 单个消息队列最大读取数量
        /// </summary>
        internal const uint MaxReaderCount = 65536;
        /// <summary>
        /// 最大内存缓存节点数量
        /// </summary>
        private const int maxMemoryCacheNodeCount = 64 << 20;

        /// <summary>
        /// 服务端内存缓存节点数量，默认为 32KB
        /// </summary>
        public uint MemoryCacheNodeCount = 32 << 10;
        /// <summary>
        /// 可发送的客户端未处理消息数量，默认为 16K
        /// </summary>
        public uint SendClientCount = 16 << 10;
        /// <summary>
        /// 格式化配置数据
        /// </summary>
        internal void Format()
        {
            if (MemoryCacheNodeCount > maxMemoryCacheNodeCount) MemoryCacheNodeCount = maxMemoryCacheNodeCount;
            else if (MemoryCacheNodeCount <= FileWriter.MaxPacketCount) MemoryCacheNodeCount = FileWriter.MaxPacketCount + 1;
            if (SendClientCount > MemoryCacheNodeCount) SendClientCount = MemoryCacheNodeCount;
            else if (MemoryCacheNodeCount == 0) MemoryCacheNodeCount = 1;
        }
    }
}
