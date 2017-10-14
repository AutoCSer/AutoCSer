using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 字符串扩展操作
    /// </summary>
    internal static class String_Weixin
    {
        /// <summary>
        /// 获取Ascii字符串原始字节流
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        internal unsafe static byte[] ConcatBytes(params string[] values)
        {
            int length = 0;
            foreach (string value in values) length += value.Length;
            byte[] data = new byte[length];
            fixed (byte* dataFixed = data)
            {
                byte* write = dataFixed;
                foreach (string value in values)
                {
                    fixed (char* valueFixed = value) StringExtension.WriteBytesNotNull(valueFixed, value.Length, write);
                    write += value.Length;
                }
            }
            return data;
        }
        /// <summary>
        /// 字符串比较
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static bool SimpleEqual(string source, void* destination)
        {
            fixed (char* sourceFixed = source) return AutoCSer.Memory.SimpleEqualNotNull((byte*)sourceFixed, (byte*)destination, source.Length << 1);
        }
    }
}
