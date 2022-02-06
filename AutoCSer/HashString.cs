using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 字符串 HASH
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct HashString : IEquatable<HashString>, IEquatable<SubString>, IEquatable<string>
    {
        /// <summary>
        /// 字符子串
        /// </summary>
        internal SubString String;
        /// <summary>
        /// 哈希值
        /// </summary>
        internal int HashCode;
        /// <summary>
        /// 字符串 HASH
        /// </summary>
        /// <param name="value"></param>
        public HashString(SubString value) : this(ref value) { }
        /// <summary>
        /// 字符串 HASH
        /// </summary>
        /// <param name="value"></param>
        public HashString(ref SubString value)
        {
            String = value;
            HashCode = value.Length == 0 ? 0 : (value.GetHashCode() ^ Random.Hash);
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串</returns>
        public static implicit operator HashString(string value) { return new HashString(value); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串</returns>
        public static implicit operator HashString(SubString value) { return new HashString(ref value); }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetEmpty()
        {
            String.SetEmpty();
            HashCode = 0;
        }
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode;
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(HashString other)
        {
            return HashCode == other.HashCode && String.Equals(ref other.String);
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(ref HashString other)
        {
            return HashCode == other.HashCode && String.Equals(ref other.String);
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((HashString)obj);
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(ref SubString other)
        {
            return String.Equals(ref other);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(SubString other)
        {
            return String.Equals(ref other);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(string other)
        {
            return String.Equals(other);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.ToString();
        }

        /// <summary>
        /// 长度为 0 的字符串
        /// </summary>
        public static readonly HashString Empty = new HashString(string.Empty);
    }
}
