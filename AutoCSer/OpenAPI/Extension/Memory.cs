using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal unsafe static class Memory_OpenAPI
    {
        /// <summary>
        /// 复制字节数组
        /// </summary>
        /// <param name="source">原字节数组,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="length">字节长度,大于等于0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyNotNull(byte[] source, void* destination, int length)
        {
            fixed (byte* data = source) AutoCSer.Memory.CopyNotNull((void*)data, destination, length);
        }
        /// <summary>
        /// 转16进制字符串(小写字母)
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>16进制字符串</returns>
        internal unsafe static string toLowerHex(this byte[] data)
        {
            if (data.length() != 0)
            {
                string hex = AutoCSer.Extension.StringExtension.FastAllocateString(data.Length << 1);
                fixed (byte* dataFixed = data)
                fixed (char* hexFixed = hex)
                {
                    char* write = hexFixed;
                    for (byte* start = dataFixed, end = dataFixed + data.Length; start != end; ++start)
                    {
                        int code = *start >> 4;
                        *write++ = (char)(code < 10 ? code + '0' : (code + ('0' + 'a' - '9' - 1)));
                        code = *start & 0xf;
                        *write++ = (char)(code < 10 ? code + '0' : (code + ('0' + 'a' - '9' - 1)));
                    }
                }
                return hex;
            }
            return data != null ? string.Empty : null;
        }
    }
}
