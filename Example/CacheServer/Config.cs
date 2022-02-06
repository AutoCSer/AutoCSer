using System;
using System.Collections.Generic;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 项目配置
    /// </summary>
    internal sealed class Config : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// 缓存主服务配置
        /// </summary>
        [AutoCSer.Configuration.Member]
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
        [AutoCSer.Configuration.Member(AutoCSer.CacheServer.MasterServer.ServerName)]
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
        [AutoCSer.Configuration.Member]
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
        [AutoCSer.Configuration.Member(AutoCSer.CacheServer.SlaveServer.ServerName)]
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
