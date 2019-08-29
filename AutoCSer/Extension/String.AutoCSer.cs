using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 字符串相关操作
    /// </summary>
    internal unsafe static partial class StringExtension
    {
        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>null为0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int length(this string value)
        {
            return value != null ? value.Length : 0;
        }
        /// <summary>
        /// 获取Ascii字符串原始字节流
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字节流</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte[] getBytes(this string value)
        {
            if (value != null)
            {
                fixed (char* valueFixed = value) return GetBytes(valueFixed, value.Length);
            }
            return null;
        }
        /// <summary>
        /// 获取Ascii字符串原始字节流
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <param name="length">字符串长度</param>
        /// <returns>字节流</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte[] GetBytes(char* value, int length)
        {
            byte[] data = new byte[length];
            fixed (byte* dataFixed = data) WriteBytes(value, length, dataFixed);
            return data;
        }
        /// <summary>
        /// 获取Ascii字符串原始字节流
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <param name="length">字符串长度</param>
        /// <param name="write">写入位置,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void WriteBytes(char* value, int length, byte* write)
        {
            for (char* start = value, end = value + length; start != end; ++start) *write++ = *(byte*)start;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="value">大写字符串</param>
        /// <returns>小写字符串(原引用)</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string toLower(this string value)
        {
            return !string.IsNullOrEmpty(value) ? toLowerNotEmpty(value) : value;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="value">大写字符串</param>
        /// <returns>小写字符串(原引用)</returns>
        internal static string toLowerNotEmpty(this string value)
        {
            fixed (char* valueFixed = value)
            {
                ToLower(valueFixed, valueFixed + value.Length);
            }
            return value;
        }
        /// <summary>
        /// 大写转小写
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end">长度必须大于0</param>
        internal static void ToLower(char* start, char* end)
        {
            do
            {
                if ((uint)(*start - 'A') < 26) *start |= (char)0x20;
            }
            while (++start != end);
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null,长度必须大于0</param>
        /// <param name="value">查找值</param>
        /// <returns>字符位置,失败为null</returns>
        internal static char* FindNotNull(char* start, char* end, char value)
        {
            if (*--end == value)
            {
                while (*start != value) ++start;
                return start;
            }
            while (start != end)
            {
                if (*start == value) return start;
                ++start;
            }
            return null;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="value">原字符串</param>
        /// <param name="split">分割符</param>
        /// <returns>字符子串集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static LeftArray<SubString> split(this string value, char split)
        {
            return new SubString(value).Split(split);
        }
        /// <summary>
        /// 字符替换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="oldChar">原字符</param>
        /// <param name="newChar">目标字符</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string replaceNotNull(this string value, char oldChar, char newChar)
        {
            fixed (char* valueFixed = value)
            {
                for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                {
                    if (*start == oldChar) *start = newChar;
                }
            }
            return value;
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <returns>是否相等</returns>
        internal static bool equalCase(this string left, string right)
        {
            if (left == null) return right == null;
            if (right != null && left.Length == right.Length)
            {
                if (left.Length == 0) return true;
                fixed (char* leftFixed = left, rightFixed = right) return equalCaseNotNull(leftFixed, rightFixed, left.Length);
            }
            return false;
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <returns>是否相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool equalCaseNotNull(this string left, string right)
        {
            if (left.Length == right.Length)
            {
                fixed (char* leftFixed = left, rightFixed = right) return equalCaseNotNull(leftFixed, rightFixed, left.Length);
            }
            return false;
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="count">比较字符数量</param>
        /// <returns>是否相等</returns>
        internal static bool equalCase(this string left, string right, int count)
        {
            if (left == null) return right == null;
            if (right != null)
            {
                if (count == 0) return true;
                int leftLength = Math.Min(left.Length, count), rightLength = Math.Min(right.Length, count);
                return leftLength == rightLength && count > 0 && equalCaseNotNull(left, right, count);
            }
            return false;
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">比较字符数量</param>
        /// <returns>是否相等</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool equalCaseNotNull(this string left, string right, int count)
        {
            fixed (char* leftFixed = left, rightFixed = right) return equalCaseNotNull(leftFixed, rightFixed, count);
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left">不能为null</param>
        /// <param name="right">不能为null</param>
        /// <param name="count">字符数量,大于等于0</param>
        /// <returns>是否相等</returns>
        internal static bool equalCaseNotNull(char* left, char* right, int count)
        {
            for (char* end = left + count; left != end; ++left, ++right)
            {
                char value = *left;
                if (value != *right)
                {
                    if ((value |= (char)0x20) != (*right | (char)0x20) || (uint)(value - 'a') >= 26) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <returns>字符位置,失败为null</returns>
        internal static char* trimStartNotEmpty(char* start, char* end)
        {
            do
            {
                switch (*start & 3)
                {
                    case 0:
                        if (*start != 0x20) return start;
                        break;
                    case 1:
                        if (*start != 0x0d) return start;
                        break;
                    case 2:
                        if (*start != 0x0a) return start;
                        break;
                    case 3:
                        if (*start != 7) return start;
                        break;
                }
            }
            while (++start != end);
            return null;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <returns>字符位置,失败为null</returns>
        internal static char* trimEndNotEmpty(char* start, char* end)
        {
            do
            {
                switch (*--end & 3)
                {
                    case 0:
                        if (*end != 0x20) return end + 1;
                        break;
                    case 1:
                        if (*end != 0x0d) return end + 1;
                        break;
                    case 2:
                        if (*end != 0x0a) return end + 1;
                        break;
                    case 3:
                        if (*end != 7) return end + 1;
                        break;
                }
            }
            while (start != end);
            return null;
        }
    }
}
