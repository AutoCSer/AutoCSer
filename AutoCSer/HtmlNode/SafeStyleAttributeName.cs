using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 安全样式名称唯一哈希
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct SafeStyleAttributeName : IEquatable<SafeStyleAttributeName>
    {
        /// <summary>
        /// 安全样式名称
        /// </summary>
        public SubString Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">安全样式名称</param>
        /// <returns>安全样式名称唯一哈希</returns>
        public static implicit operator SafeStyleAttributeName(string name) { return new SafeStyleAttributeName { Name = name }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">安全样式名称</param>
        /// <returns>安全样式名称唯一哈希</returns>
        public static implicit operator SafeStyleAttributeName(SubString name) { return new SafeStyleAttributeName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return Name.Length < 4 ? 0 : (Name[Name.Length - 4] | 0x20) & 7;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(SafeStyleAttributeName other)
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
            return Equals((SafeStyleAttributeName)obj);
        }
        /// <summary>
        /// 安全样式名称集合
        /// </summary>
        public static readonly UniqueHashSet<SafeStyleAttributeName> Names = new UniqueHashSet<SafeStyleAttributeName>(new SafeStyleAttributeName[] { "font", "font-family", "font-size", "font-weight", "color", "text-decoration" }, 8);
    }
}
