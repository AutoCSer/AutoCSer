using System;
using AutoCSer.Extension;

namespace AutoCSer.Sql.MySql
{
    /// <summary>
    /// 数据类型名称唯一哈希
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct TypeName : IEquatable<TypeName>
    {
        /// <summary>
        /// 数据类型名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 数据类型长度
        /// </summary>
        public int Length;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">数据类型名称</param>
        /// <returns>数据类型名称唯一哈希</returns>
        public static implicit operator TypeName(string name) { return new TypeName { Name = name, Length = name.Length }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public unsafe override int GetHashCode()
        {
            if (Name.Length <= 2) return 1;
            fixed (char* nameFixed = Name)
            {
                return ((nameFixed[2] << 2) ^ ((nameFixed[Length >> 2] >> 2) | 0x20)) & (int)((1U << 4) - 1);
            }
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public bool Equals(TypeName other)
        {
            return Length == other.Length && Name.equalCaseNotNull(other.Name, Length);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj">待匹配数据</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((TypeName)obj);
        }
    }
}
