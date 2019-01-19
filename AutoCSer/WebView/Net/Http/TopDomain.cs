using System;
using AutoCSer.Extension;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 顶级域名唯一哈希
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct TopDomain : IEquatable<TopDomain>
    {
        /// <summary>
        /// 顶级域名
        /// </summary>
        public SubArray<byte> Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">顶级域名</param>
        /// <returns>顶级域名唯一哈希</returns>
        public static implicit operator TopDomain(string name) { return new SubArray<byte> { Array = name.getBytes(), Length = name.Length }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">顶级域名</param>
        /// <returns>顶级域名唯一哈希</returns>
        public static implicit operator TopDomain(SubArray<byte> name) { return new TopDomain { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            if (Name.Length < 3) return 0;
            byte[] key = Name.Array;
            uint code = (uint)(key[Name.Start] << 8) + (uint)key[Name.Start + 2];
            return (int)(((code >> 4) ^ code) & ((1U << 5) - 1));
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public unsafe bool Equals(TopDomain other)
        {
            return Name.equal(ref other.Name);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj">待匹配数据</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((TopDomain)obj);
        }
    }
}
