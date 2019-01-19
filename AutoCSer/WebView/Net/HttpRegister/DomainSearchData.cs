using System;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// 域名搜索数据
    /// </summary>
    internal unsafe sealed class DomainSearchData : IDisposable
    {
        /// <summary>
        /// 域名信息集合
        /// </summary>
        private byte[][] domains;
        /// <summary>
        /// 域名服务信息集合
        /// </summary>
        private DomainServer[] servers;
        /// <summary>
        /// 域名搜索数据
        /// </summary>
        private Pointer.Size data;
        /// <summary>
        /// 字节数组搜索器
        /// </summary>
        private DomainSearcher searcher;
        /// <summary>
        /// 最后一次查询域名
        /// </summary>
        private Pointer.Size lastDomain;
        /// <summary>
        /// 最后一次查询域名服务信息
        /// </summary>
        private DomainServer lastServer;
        /// <summary>
        /// 最后一次查询域名长度
        /// </summary>
        private int lastDomainSize;
        /// <summary>
        /// 最后一次查询访问锁
        /// </summary>
        private readonly object lastLock = new object();
        /// <summary>
        /// 域名搜索
        /// </summary>
        internal DomainSearchData()
        {
            servers = NullValue<DomainServer>.Array;
        }
        /// <summary>
        /// 域名搜索
        /// </summary>
        /// <param name="domains">域名信息集合</param>
        /// <param name="servers">域名服务信息集合</param>
        private DomainSearchData(byte[][] domains, DomainServer[] servers)
        {
            this.domains = domains;
            this.servers = servers;
            data = AutoCSer.StateSearcher.ByteBuilder.Create(domains, false);
            searcher = new DomainSearcher(ref data);
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~DomainSearchData()
        {
            Unmanaged.Free(ref data);
            Unmanaged.Free(ref lastDomain);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Dispose()
        {
            searcher.State = null;
            Unmanaged.Free(ref data);
            Monitor.Enter(lastLock);
            Unmanaged.Free(ref lastDomain);
            Monitor.Exit(lastLock);
        }
        /// <summary>
        /// 关闭所有域名服务
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Close()
        {
            foreach (DomainServer domain in servers)
            {
                if (domain != null) domain.Dispose();
            }
        }
        /// <summary>
        /// 获取域名服务信息
        /// </summary>
        /// <param name="domain">域名</param>
        /// <param name="size">域名长度</param>
        /// <returns>域名服务信息</returns>
        internal DomainServer Get(byte* domain, int size)
        {
            if (searcher.State != null)
            {
                if (lastDomainSize == size && Monitor.TryEnter(lastLock))
                {
                    if (lastDomainSize == size && Memory.SimpleEqualNotNull(lastDomain.Byte, domain, lastDomainSize))
                    {
                        DomainServer server = lastServer;
                        Monitor.Exit(lastLock);
                        return server;
                    }
                    Monitor.Exit(lastLock);
                }
                int index = searcher.Search(domain, domain + size);
                if (index >= 0)
                {
                    DomainServer server = servers[index];
                    if (server != null)
                    {
                        if (Monitor.TryEnter(lastLock))
                        {
                            if (lastDomain.Byte == null)
                            {
                                try
                                {
                                    lastDomain = Unmanaged.Get(Server.MaxDomainSize, false, false);
                                    Memory.SimpleCopyNotNull(domain, lastDomain.Byte, lastDomainSize = size);
                                    lastServer = server;
                                }
                                finally { Monitor.Exit(lastLock); }

                            }
                            else
                            {
                                Memory.SimpleCopyNotNull(domain, lastDomain.Byte, lastDomainSize = size);
                                lastServer = server;
                                Monitor.Exit(lastLock);
                            }
                        }
                        return server;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 添加域名服务信息
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="server"></param>
        /// <param name="removeDomains">域名搜索</param>
        /// <returns></returns>
        internal DomainSearchData Add(byte[] domain, DomainServer server, out DomainSearchData removeDomains)
        {
            byte[][] domains = this.domains;
            DomainServer[] servers = this.servers;
            if (domain.Length != 0)
            {
                fixed (byte* domainFixed = domain)
                {
                    byte* domainEnd = domainFixed + domain.Length;
                    int index = searcher.State != null ? searcher.Search(domainFixed, domainEnd): -1;
                    if (index < 0 || servers[index] == null)
                    {
                        int count = index = 0;
                        foreach (DomainServer nextServer in servers)
                        {
                            if (nextServer != null) ++count;
                        }
                        byte[][] newDomains = new byte[++count][];
                        fixed (byte* reverseDomainFixed = (newDomains[0] = new byte[domain.Length]))
                        {
                            for (byte* start = domainFixed, write = reverseDomainFixed + domain.Length; start != domainEnd; *--write = *start++) ;
                        }
                        DomainServer[] newServers = new DomainServer[count];
                        newServers[0] = server;
                        count = 1;
                        foreach (DomainServer nextServer in servers)
                        {
                            if (nextServer != null)
                            {
                                newDomains[count] = domains[index];
                                newServers[count] = servers[index];
                                ++count;
                            }
                            ++index;
                        }
                        if (newServers.Length != count)
                        {
                            Array.Resize(ref newDomains, count);
                            Array.Resize(ref newServers, count);
                        }
                        DomainSearchData newSearchData = new DomainSearchData(newDomains, newServers);
                        removeDomains = this;
                        return newSearchData;
                    }
                }
            }
            removeDomains = null;
            return this;
        }
        /// <summary>
        /// 删除域名服务信息
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal DomainServer Remove(byte[] domain)
        {
            if (domain.Length != 0)
            {
                byte[][] domains = this.domains;
                DomainServer[] servers = this.servers;
                if (searcher.State != null)
                {
                    int index = searcher.Search(domain);
                    if (index >= 0)
                    {
                        DomainServer removeDomainServer = servers[index];
                        servers[index] = null;
                        return removeDomainServer;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取域名注册信息集合
        /// </summary>
        /// <returns></returns>
        internal Cache[] GetSaveCache()
        {
            int count = 0;
            foreach (DomainServer server in servers)
            {
                if (server != null) ++count;
            }
            if (count == 0) return null;
            Cache[] registers = new Cache[count];
            count = 0;
            foreach (DomainServer server in servers)
            {
                if ((registers[count] = server.GetSaveCache()).Domains.Length != 0) ++count;
            }
            if (count == 0) return null;
            if (count != registers.Length) Array.Resize(ref registers, count);
            return registers;
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void StopListen()
        {
            foreach (DomainServer domain in servers) domain.StopListen();
        }

        /// <summary>
        /// 默认空域名搜索
        /// </summary>
        internal static readonly DomainSearchData Default = new DomainSearchData();
    }
}
