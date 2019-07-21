using System;
using System.Net;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.HtmlTitle
{
    /// <summary>
    /// IP 地址信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DomainIPAddress
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        public DateTime Timeout;
        /// <summary>
        /// IP地址
        /// </summary>
        public IPAddress[] Ips;
        /// <summary>
        /// 域名字符串
        /// </summary>
        public string Domain;

        /// <summary>
        /// 域名转换IP地址集合
        /// </summary>
        private static FifoPriorityQueue<HashBytes, DomainIPAddress> domainIps = new FifoPriorityQueue<HashBytes, DomainIPAddress>();
        /// <summary>
        /// 域名转换IP地址访问锁
        /// </summary>
        private static readonly object domainIpLock = new object();
        /// <summary>
        /// 域名转IP地址缓存超时时钟周期
        /// </summary>
        private static readonly long domainIpTimeoutTicks = new TimeSpan(0, DomainIPAddressConfig.Default.TimeoutMinutes, 0).Ticks;
        /// <summary>
        /// 根据域名获取IP地址
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns>IP地址,失败返回null</returns>
        internal unsafe static IPAddress[] Get(ref SubArray<byte> domain)
        {
            try
            {
                fixed (byte* domainFixed = domain.Array)
                {
                    byte* domainStart = domainFixed + domain.StartIndex;
                    AutoCSer.Memory.ToLowerNotNull(domainStart, domainStart + domain.Length);
                    HashBytes key = domain;
                    DomainIPAddress value;
                    Monitor.Enter(domainIpLock);
                    try
                    {
                        value = domainIps.Get(ref key, default(DomainIPAddress));
                        if (value.Ips != null && value.Timeout < AutoCSer.Date.NowTime.Now)
                        {
                            domainIps.Remove(ref key, out value);
                            value.Ips = null;
                        }
                    }
                    finally { Monitor.Exit(domainIpLock); }
                    if (value.Ips == null)
                    {
                        if (value.Domain == null) value.Domain = Memory_WebClient.BytesToStringNotEmpty(domainStart, domain.Length);
                        IPAddress ip;
                        if (IPAddress.TryParse(value.Domain, out ip))
                        {
                            value.Timeout = DateTime.MaxValue;
                            value.Domain = null;
                            setDomainIp(key.Copy(), ref value);
                            return value.Ips = new IPAddress[] { ip };
                        }
                        value.Ips = Dns.GetHostEntry(value.Domain).AddressList;
                        if (value.Ips.Length != 0)
                        {
                            value.Timeout = AutoCSer.Date.NowTime.Now.AddTicks(domainIpTimeoutTicks);
                            setDomainIp(key.Copy(), ref value);
                            return value.Ips;
                        }
                    }
                    else return value.Ips;
                }
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            return null;
        }
        /// <summary>
        /// 设置域名转换IP地址
        /// </summary>
        /// <param name="key">域名</param>
        /// <param name="ipAddress">IP地址</param>
        private static void setDomainIp(HashBytes key, ref DomainIPAddress ipAddress)
        {
            Monitor.Enter(domainIpLock);
            try
            {
                domainIps.Set(ref key, ipAddress);
                if (domainIps.Count == DomainIPAddressConfig.Default.CacheCount) domainIps.UnsafePopNode();
            }
            finally { Monitor.Exit(domainIpLock); }
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            Monitor.Enter(domainIpLock);
            try
            {
                if (domainIps.Count != 0) domainIps = new FifoPriorityQueue<HashBytes, DomainIPAddress>();
            }
            finally { Monitor.Exit(domainIpLock); }
        }
        static DomainIPAddress()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
