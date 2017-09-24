using System;

namespace AutoCSer.Net.HtmlTitle
{
    /// <summary>
    /// 域名 IP 转换配置
    /// </summary>
    public class DomainIPAddressConfig
    {
        /// <summary>
        /// 域名转IP地址缓存时间分钟数默认为 60
        /// </summary>
        public int TimeoutMinutes = 60;
        /// <summary>
        /// 缓存数量默认为 1024（小于等于0表示不限）
        /// </summary>
        public int CacheCount = 1 << 10;
        /// <summary>
        /// 初始化
        /// </summary>
        private void set()
        {
            if (TimeoutMinutes <= 1) TimeoutMinutes = 1;
        }

        /// <summary>
        /// 域名 IP 转换配置
        /// </summary>
        internal static readonly DomainIPAddressConfig Default;
        static DomainIPAddressConfig()
        {
            (Default = ConfigLoader.GetUnion(typeof(DomainIPAddressConfig)).DomainIPAddressConfig ?? new DomainIPAddressConfig()).set();
        }
    }
}
