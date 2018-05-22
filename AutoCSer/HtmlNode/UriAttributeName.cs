using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// URI 属性名称唯一哈希
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct UriAttributeName : IEquatable<UriAttributeName>
    {
        /// <summary>
        /// URI属性名称
        /// </summary>
        public SubString Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">URI属性名称</param>
        /// <returns>URI属性名称唯一哈希</returns>
        public static implicit operator UriAttributeName(string name) { return new UriAttributeName { Name = name }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">URI属性名称</param>
        /// <returns>URI属性名称唯一哈希</returns>
        public static implicit operator UriAttributeName(SubString name) { return new UriAttributeName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return (Name[0] | 0x20) & 7;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(UriAttributeName other)
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
            return Equals((UriAttributeName)obj);
        }
        /// <summary>
        /// URI 属性名称集合
        /// </summary>
        public static readonly UniqueHashSet<UriAttributeName> Names = new UniqueHashSet<UriAttributeName>(new UriAttributeName[] { "background", "dynsrc", "href", "src" }, 5);
    }
}
