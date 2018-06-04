using System;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 项目配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// 缓存主服务配置
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.CacheServer.MasterServerConfig CacheMasterServerConfig
        {
            get
            {
                return new AutoCSer.CacheServer.MasterServerConfig { FileName = "test", IsIgnoreFileEndError = true };
            }
        }
        /// <summary>
        /// 缓存主服务 TCP 静态服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.CacheServer.MasterServer.ServerName)]
        public static AutoCSer.Net.TcpInternalServer.ServerAttribute CacheMasterConfig
        {
            get
            {
                AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.CacheServer.MasterServer), false);
                attribute.VerifyString = "!2#4%6&8QwErTyAsDfZx";
                return attribute;
            }
        }
        /// <summary>
        /// 缓存从服务配置
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.CacheServer.SlaveServerConfig CacheSlaveServerConfig
        {
            get
            {
                return new AutoCSer.CacheServer.SlaveServerConfig { CacheServerAttribute = CacheMasterConfig };
            }
        }
        /// <summary>
        /// 缓存从服务 TCP 静态服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.CacheServer.SlaveServer.ServerName)]
        public static AutoCSer.Net.TcpInternalServer.ServerAttribute CacheSlaveConfig
        {
            get
            {
                AutoCSer.Net.TcpInternalServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalServer.ServerAttribute>(typeof(AutoCSer.CacheServer.SlaveServer), false);
                attribute.VerifyString = "!2#4%6&8QwErTyAsDfZx";
                return attribute;
            }
        }
    }
}
