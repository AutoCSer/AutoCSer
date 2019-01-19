using System;
using System.Net;

namespace fastCSharp.Net
{
    /// <summary>
    /// IPv6地址哈希
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Ipv6Hash : IEquatable<Ipv6Hash>
    {
        /// <summary>
        /// IPv6地址
        /// </summary>
        private static readonly Func<IPAddress, ushort[]> ipAddress = fastCSharp.Emit.Pub.GetField<IPAddress, ushort[]>("m_Numbers");
        /// <summary>
        /// IP地址
        /// </summary>
        internal ushort[] Ip;
        /// <summary>
        /// 哈希值
        /// </summary>
        internal int HashCode;
        /// <summary>
        /// IPv6地址哈希
        /// </summary>
        /// <param name="ip"></param>
        public unsafe Ipv6Hash(IPAddress ip)
        {
            if ((Ip = ipAddress(ip)) == null) HashCode = 0;
            else
            {
                fixed (ushort* ipFixed = Ip)
                {
                    HashCode = * (int*)ipFixed ^ *(int*)(ipFixed + 2) ^ *(int*)(ipFixed + 4) ^ *(int*)(ipFixed + 6) ^ Random.Hash;
                }
            }
        }
        /// <summary>
        /// IPv6地址哈希隐式转换
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>IPv6地址哈希</returns>
        public static implicit operator Ipv6Hash(IPAddress ip) { return new Ipv6Hash(ip); }
        /// <summary>
        /// IPv6地址哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override unsafe int GetHashCode() { return HashCode; }
        /// <summary>
        /// IPv6地址哈希是否相等
        /// </summary>
        /// <param name="obj">IPv6地址哈希</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj) { return Equals((Ipv6Hash)obj); }
        /// <summary>
        /// IPv6地址哈希是否相等
        /// </summary>
        /// <param name="other">IPv6地址哈希</param>
        /// <returns>是否相等</returns>
        public unsafe bool Equals(Ipv6Hash other)
        {
            fixed (ushort* ipFixed = Ip, otherFixed = other.Ip)
            {
                if (*(int*)ipFixed == *(int*)otherFixed)
                {
                    return ((*(int*)(ipFixed + 2) ^ *(int*)(otherFixed + 2))
                        | (*(int*)(ipFixed + 4) ^ *(int*)(otherFixed + 4))
                        | (*(int*)(ipFixed + 6) ^ *(int*)(otherFixed + 6))) == 0;
                }
            }
            return false;
        }
    }
}
