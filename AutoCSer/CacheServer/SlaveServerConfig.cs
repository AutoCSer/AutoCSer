using System;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存从服务配置
    /// </summary>
    public sealed class SlaveServerConfig : ServerConfig
    {
        /// <summary>
        /// 缓存源服务 TCP 内部服务配置
        /// </summary>
        public AutoCSer.Net.TcpInternalServer.ServerAttribute CacheServerAttribute;
        /// <summary>
        /// 默认为 true 表示缓存源服务为主缓存服务
        /// </summary>
        public bool IsMasterCacheServer = true;
    }
}
