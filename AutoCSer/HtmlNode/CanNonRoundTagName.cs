using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 允许不回合的标签名称唯一哈希
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct CanNonRoundTagName : IEquatable<CanNonRoundTagName>
    {
        /// <summary>
        /// 允许不回合的标签名称
        /// </summary>
        public SubString Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">允许不回合的标签名称</param>
        /// <returns>允许不回合的标签名称唯一哈希</returns>
        public static implicit operator CanNonRoundTagName(string name) { return new CanNonRoundTagName { Name = name }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">允许不回合的标签名称</param>
        /// <returns>允许不回合的标签名称唯一哈希</returns>
        public static implicit operator CanNonRoundTagName(SubString name) { return new CanNonRoundTagName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            if (Name.Length == 0) return 1;
            int code = ((Name[Name.Length - 1] | 0x20) << 7) + (Name[0] | 0x20);
            return ((code >> 5) ^ code) & ((1 << 5) - 1);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(CanNonRoundTagName other)
        {
            return Name.EqualCase(ref other.Name);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj">待匹配数据</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((CanNonRoundTagName)obj);
        }
        /// <summary>
        /// 允许不回合的标签名称集合
        /// </summary>
        public static readonly UniqueHashSet<CanNonRoundTagName> TagNames = new UniqueHashSet<CanNonRoundTagName>(new CanNonRoundTagName[] { "area", "areatext", "basefont", "br", "col", "colgroup", "hr", "img", "input", "li", "p", "spacer" }, 27);
    }
}
