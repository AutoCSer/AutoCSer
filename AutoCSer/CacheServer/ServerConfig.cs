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
    }
}
