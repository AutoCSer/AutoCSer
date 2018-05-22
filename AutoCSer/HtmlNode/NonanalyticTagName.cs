using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 非解析标签名称唯一哈希
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct NonanalyticTagName : IEquatable<NonanalyticTagName>
    {
        /// <summary>
        /// 非解析标签名称
        /// </summary>
        public SubString Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">非解析标签名称</param>
        /// <returns>非解析标签名称唯一哈希</returns>
        public static implicit operator NonanalyticTagName(string name) { return new NonanalyticTagName { Name = name }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">非解析标签名称</param>
        /// <returns>非解析标签名称唯一哈希</returns>
        public static implicit operator NonanalyticTagName(SubString name) { return new NonanalyticTagName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            switch (Name.Length)
            {
                case 0: return 2;
                case 1: return (Name[0] >> 2) & ((1 << 3) - 1);
                default: return ((Name[Name.Length - 1] | 0x20) >> 2) & ((1 << 3) - 1);
            }
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(NonanalyticTagName other)
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
            return Equals((NonanalyticTagName)obj);
        }
        /// <summary>
        /// 非解析标签名称集合
        /// </summary>
        public static readonly UniqueHashSet<NonanalyticTagName> TagNames = new UniqueHashSet<NonanalyticTagName>(new NonanalyticTagName[] { "script", "style", "!", "/" }, 6);
    }
}
