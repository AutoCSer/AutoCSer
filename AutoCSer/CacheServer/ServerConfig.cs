using System;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存服务配置
    /// </summary>
    public abstract class ServerConfig
    {
        /// <summary>
        /// 缓存从服务初始化获取缓存数据默认超时时间
        /// </summary>
        public int GetCacheLoadTimeoutSeconds = 14;
        /// <summary>
        /// 短路径数据，默认为 1K
        /// </summary>
        public int ShortPathCount = 1 << 10;
        /// <summary>
        /// 短路径数据
        /// </summary>
        internal int GetShortPathCount
        {
            get { return Math.Max(ShortPathCount, 4); }
        }
    }
}
