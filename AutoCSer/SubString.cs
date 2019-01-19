using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 字符子串
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe partial struct SubString : IEquatable<SubString>, IEquatable<string>
    {
        /// <summary>
        /// 原字符串
        /// </summary>
        internal string String;
        /// <summary>
        /// 原字符串中的起始位置
        /// </summary>
        internal int Start;
        /// <summary>
        /// 字符子串长度
        /// </summary>
        internal int Length;
        /// <summary>
        /// 获取字符
        /// </summary>
        /// <param name="index">字符位置</param>
        /// <returns></returns>
        public char this[int index]
        {
            get { return String[Start + index]; }
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetNull()
        {
            String = null;
            Start = Length = 0;
        }
        /// <summary>
        /// 设置数据长度
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <param name="startIndex">起始位置,必须合法</param>
        /// <param name="length">长度,必须合法</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(string value, int startIndex, int length)
        {
            String = value;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 字符串隐式转换为子串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符子串</returns>
        public static implicit operator SubString(string value)
        {
            return value != null ? new SubString { String = value, Start = 0, Length = value.Length } : default(SubString);
        }
        /// <summary>
        /// 字符子串隐式转换为字符串
        /// </summary>
        /// <param name="value">字符子串</param>
        /// <returns>字符串</returns>
        public static implicit operator string(SubString value) { return value.ToString(); }
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        public unsafe override int GetHashCode()
        {
            if (Length == 0) return 0;
            fixed (char* valueFixed = String) return Memory.GetHashCode(valueFixed + Start, Length << 1);
        }
        /// <summary>
        /// 判断子串是否相等
        /// </summary>
        /// <param name="obj">待比较子串</param>
        /// <returns>子串是否相等</returns>
        public override bool Equals(object obj)
        {
            SubString other = (SubString)obj;
            return Equals(ref other); 
        }
        /// <summary>
        /// 判断子串是否相等
        /// </summary>
        /// <param name="other">待比较子串</param>
        /// <returns>子串是否相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Equals(SubString other)
        {
            return Equals(ref other);
        }
        /// <summary>
        /// 判断子串是否相等
        /// </summary>
        /// <param name="other">待比较子串</param>
        /// <returns>子串是否相等</returns>
        public bool Equals(ref SubString other)
        {
            if (Length == 0) return other.Length == 0 && !((String == null) ^ (other.String == null));
            if (Length == other.Length)
            {
                if (Object.ReferenceEquals(String, other.String) && Start == other.Start) return true;
                fixed (char* valueFixed = String, otherFixed = other.String)
                {
                    return Memory.EqualNotNull(valueFixed + Start, otherFixed + other.Start, Length << 1);
                }
            }
            return false;
        }
        /// <summary>
        /// 判断子串是否相等
        /// </summary>
        /// <param name="other">待比较子串</param>
        /// <returns>子串是否相等</returns>
        public bool Equals(string other)
        {
            if (Length == 0) return other == null ? String == null : (other.Length == 0 && String != null);
            if (other != null && Length == other.Length)
            {
                if (Object.ReferenceEquals(String, other) && Start == 0) return true;
                fixed (char* valueFixed = String, otherFixed = other)
                {
                    return Memory.EqualNotNull(valueFixed + Start, otherFixed, Length << 1);
                }
            }
            return false;
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public unsafe override string ToString()
        {
            if (String != null)
            {
                if ((Start | (Length ^ String.Length)) == 0) return String;
                fixed (char* valueFixed = String) return new string(valueFixed, Start, Length);
            }
            return null;
        }
        /// <summary>
        /// 获取子串
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>子串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SubString GetSub(int startIndex, int length)
        {
            return new SubString { String = String, Start = Start + startIndex, Length = length };
        }
    }
}
