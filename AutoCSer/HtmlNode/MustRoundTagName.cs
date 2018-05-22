using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 必须回合的标签名称唯一哈希
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct MustRoundTagName : IEquatable<MustRoundTagName>
    {
        /// <summary>
        /// 必须回合的标签名称
        /// </summary>
        public SubString Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">必须回合的标签名称</param>
        /// <returns>必须回合的标签名称唯一哈希</returns>
        public static implicit operator MustRoundTagName(string name) { return new MustRoundTagName { Name = name }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">必须回合的标签名称</param>
        /// <returns>必须回合的标签名称唯一哈希</returns>
        public static implicit operator MustRoundTagName(SubString name) { return new MustRoundTagName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            if (Name.Length == 0) return 0;
            int code = ((Name[Name.Length >> 1] | 0x20) << 14) + ((Name[0] | 0x20) << 7) + (Name[Name.Length - 1] | 0x20);
            return ((code >> 15) ^ (code >> 13) ^ (code >> 1)) & ((1 << 8) - 1);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(MustRoundTagName other)
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
            return Equals((MustRoundTagName)obj);
        }
        /// <summary>
        /// 必须回合的标签名称集合
        /// </summary>
        public static readonly UniqueHashSet<MustRoundTagName> TagNames = new UniqueHashSet<MustRoundTagName>(new MustRoundTagName[] { "a", "b", "bgsound", "big", "body", "button", "caption", "center", "div", "em", "embed", "font", "form", "h1", "h2", "h3", "h4", "h5", "h6", "hn", "html", "i", "iframe", "map", "marquee", "multicol", "nobr", "ol", "option", "pre", "s", "select", "small", "span", "strike", "strong", "sub", "sup", "table", "tbody", "td", "textarea", "tfoot", "th", "thead", "tr", "u", "ul" }, 239);
    }

}
