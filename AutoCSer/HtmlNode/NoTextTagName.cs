using System;
using System.Runtime.InteropServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 非文本标签名称唯一哈希
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct NoTextTagName : IEquatable<NoTextTagName>
    {
        /// <summary>
        /// 非文本标签名称
        /// </summary>
        public SubString Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">非文本标签名称</param>
        /// <returns>非文本标签名称唯一哈希</returns>
        public static implicit operator NoTextTagName(string name) { return new NoTextTagName { Name = name }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">非文本标签名称</param>
        /// <returns>非文本标签名称唯一哈希</returns>
        public static implicit operator NoTextTagName(SubString name) { return new NoTextTagName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            switch (Name.Length)
            {
                case 0: return 5;
                case 1:
                    int code1 = (Name[0] << 7) + Name[0];
                    return ((code1 >> 7) ^ (code1 >> 2)) & ((1 << 4) - 1);
                default:
                    int code = ((Name[0] | 0x20) << 7) + (Name[Name.Length >> 2] | 0x20);
                    return ((code >> 7) ^ (code >> 2)) & ((1 << 4) - 1);
            }
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(NoTextTagName other)
        {
            return Name == other.Name;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj">待匹配数据</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((NoTextTagName)obj);
        }
        /// <summary>
        /// 非文本标签名称集合
        /// </summary>
        public static readonly UniqueHashSet<NoTextTagName> TagNames = new UniqueHashSet<NoTextTagName>(new NoTextTagName[] { "script", "style", "pre", "areatext", "!", "/", "input", "iframe", "img", "link", "head" }, 15);
    }
}
