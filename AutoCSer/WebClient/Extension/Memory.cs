using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal unsafe static class Memory_WebClient
    {
        /// <summary>
        /// 根据原始字节流生成字符串
        /// </summary>
        /// <param name="data">原始字节流</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static string BytesToStringNotEmpty(this byte[] data)
        {
            fixed (byte* dataFixed = data) return BytesToStringNotEmpty(dataFixed, data.Length);
        }
        /// <summary>
        /// 根据原始字节流生成字符串
        /// </summary>
        /// <param name="data">原始字节流</param>
        /// <param name="length">字符串长度</param>
        /// <returns>字符串</returns>
        internal unsafe static string BytesToStringNotEmpty(byte* data, int length)
        {
            string value = AutoCSer.Extension.StringExtension.FastAllocateString(length);
            fixed (char* valueFixed = value)
            {
                char* start = valueFixed;
                for (byte* end = data + length; data != end; *start++ = (char)*data++) ;
            }
            return value;
        }
    }
}
