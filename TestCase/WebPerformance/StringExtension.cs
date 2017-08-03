using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 字符串相关操作
    /// </summary>
    internal unsafe static partial class StringExtension
    {
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
    }
}
