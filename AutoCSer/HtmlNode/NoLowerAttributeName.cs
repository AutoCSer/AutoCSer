using System;
using System.Runtime.InteropServices;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// 初始化特殊属性名称唯一哈希(不能用全小写字母表示的属性名称)
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct NoLowerAttributeName : IEquatable<NoLowerAttributeName>
    {
        /// <summary>
        /// 初始化特殊属性名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">初始化特殊属性名称</param>
        /// <returns>初始化特殊属性名称唯一哈希</returns>
        public static implicit operator NoLowerAttributeName(string name) { return new NoLowerAttributeName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            if (Name.Length < 8) return 2;
            return Name[1] & 1;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(NoLowerAttributeName other)
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
            return Equals((NoLowerAttributeName)obj);
        }

        /// <summary>
        /// 初始化特殊属性名称集合(不能用全小写字母表示的属性名称)
        /// </summary>
        public readonly static UniqueHashSet<NoLowerAttributeName> Names = new UniqueHashSet<NoLowerAttributeName>(new NoLowerAttributeName[] { "readOnly", "className" }, 2);
    }
}
