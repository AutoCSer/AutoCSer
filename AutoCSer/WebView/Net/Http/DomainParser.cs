using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 域名分解器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct DomainParser
    {
        /// <summary>
        /// 顶级域名集合
        /// </summary>
        private static readonly UniqueHashSet<TopDomain> topDomains = new UniqueHashSet<TopDomain>(new TopDomain[] { "arpa", "com", "edu", "gov", "int", "mil", "net", "org", "biz", "name", "info", "pro", "museum", "aero", "coop" }, 30);
        /// <summary>
        /// 域名数据
        /// </summary>
        private byte[] data;
        /// <summary>
        /// 域名数据
        /// </summary>
        private byte* dataFixed;
        /// <summary>
        /// 域名数据结束
        /// </summary>
        private byte* dataEnd;
        /// <summary>
        /// 根据URL地址获取主域名
        /// </summary>
        /// <param name="start">URL起始位置</param>
        /// <returns>主域名</returns>
        public SubArray<byte> GetMainDomainByUrl(byte* start)
        {
            byte* end = dataEnd, domain = Memory.Find(start, end, (byte)':');
            if (domain != null && *(short*)++domain == '/' + ('/' << 8) && (domain += sizeof(short)) < end)
            {
                byte* next = Memory.Find(domain, end, (byte)'/');
                if (next == null) return getMainDomain(domain, end);
                if (domain != next) return getMainDomain(domain, next);

            }
            return default(SubArray<byte>);
        }
        /// <summary>
        /// 获取主域名
        /// </summary>
        /// <param name="start">域名起始位置</param>
        /// <returns>主域名</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public SubArray<byte> GetMainDomain(byte* start)
        {
            return getMainDomain(start, dataEnd);
        }
        /// <summary>
        /// 获取主域名
        /// </summary>
        /// <param name="domain">域名起始位置</param>
        /// <param name="end">域名结束位置</param>
        /// <returns>主域名</returns>
        private SubArray<byte> getMainDomain(byte* domain, byte* end)
        {
            byte* next = Memory.Find(domain, end, (byte)':');
            if (next != null) end = next;
            if (domain != end)
            {
                for (next = domain; next != end; ++next)
                {
                    if (((uint)(*next - '0') >= 10 && *next != '.'))
                    {
                        byte* dot1 = Memory.FindLast(domain, end, (byte)'.');
                        if (dot1 != null && domain != dot1)
                        {
                            byte* dot2 = Memory.FindLast(domain, dot1, (byte)'.');
                            if (dot2 != null)
                            {
                                if (topDomains.Contains(new SubArray<byte> { Array = data, Start = (int)(dot1 - dataFixed) + 1, Length = (int)(end - dot1) - 1 })
                                    || !topDomains.Contains(new SubArray<byte> { Array = data, Start = (int)(dot2 - dataFixed) + 1, Length = (int)(dot1 - dot2) - 1 }))
                                {
                                    domain = dot2 + 1;
                                }
                                else if (domain != dot2 && (dot1 = Memory.FindLast(domain, dot2, (byte)'.')) != null)
                                {
                                    domain = dot1 + 1;
                                }
                            }
                        }
                        break;
                    }
                }
                return new SubArray<byte> { Array = data, Start = (int)(domain - dataFixed), Length = (int)(end - domain) };
            }
            return default(SubArray<byte>);
        }

        /// <summary>
        /// 根据URL地址获取主域名
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <returns>主域名</returns>
        internal unsafe static SubArray<byte> GetMainDomainByUrl(SubArray<byte> url)
        {
            fixed (byte* urlFixed = url.Array)
            {
                byte* urlStart = urlFixed + url.Start;
                return new DomainParser { data = url.Array, dataFixed = urlFixed, dataEnd = urlStart + url.Length }.GetMainDomainByUrl(urlStart);
            }
        }
        /// <summary>
        /// 根据域名获取主域名
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns>主域名</returns>
        internal unsafe static SubArray<byte> GetMainDomain(SubArray<byte> domain)
        {
            fixed (byte* domainFixed = domain.Array)
            {
                byte* domainStart = domainFixed + domain.Start;
                return new DomainParser { data = domain.Array, dataFixed = domainFixed, dataEnd = domainStart + domain.Length }.GetMainDomain(domainStart);
            }
        }
    }
}
