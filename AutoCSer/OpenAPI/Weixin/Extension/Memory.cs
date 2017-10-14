using System;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal unsafe static class Memory_Weixin
    {
        /// <summary>
        /// 16进制字符串比较(小写字母)
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="hex"></param>
        /// <returns>16进制字符串</returns>
        internal static bool checkLowerHexNotNull(this byte[] data, string hex)
        {
            fixed (byte* dataFixed = data)
            fixed (char* hexFixed = hex)
            {
                char* hexChar = hexFixed;
                for (byte* start = dataFixed, end = dataFixed + data.Length; start != end; ++start)
                {
                    int code = *start >> 4;
                    if (*hexChar++ != (char)(code < 10 ? code + '0' : (code + ('0' + 'a' - '9' - 1)))) return false;
                    code = *start & 0xf;
                    if (*hexChar++ != (char)(code < 10 ? code + '0' : (code + ('0' + 'a' - '9' - 1)))) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 转16进制字符串(小写字母)
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>16进制字符串</returns>
        internal static string toUpperHex(this byte[] data)
        {
            if (data == null) return null;
            if (data.Length == 0) return string.Empty;
            string hex = AutoCSer.Extension.StringExtension.FastAllocateString(data.Length << 1);
            fixed (byte* dataFixed = data)
            fixed (char* hexFixed = hex)
            {
                char* write = hexFixed;
                for (byte* start = dataFixed, end = dataFixed + data.Length; start != end; ++start)
                {
                    *write++ = (char)AutoCSer.Extension.Number.ToHex((uint)*start >> 4);
                    *write++ = (char)AutoCSer.Extension.Number.ToHex((uint)*start & 0xf);
                }
            }
            return hex;
        }
        /// <summary>
        /// 16进制字符串比较(大写字母)
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="hex"></param>
        /// <returns>16进制字符串</returns>
        internal static bool checkUpperHexNotNull(this byte[] data, string hex)
        {
            fixed (byte* dataFixed = data)
            fixed (char* hexFixed = hex)
            {
                char* hexChar = hexFixed;
                for (byte* start = dataFixed, end = dataFixed + data.Length; start != end; ++start)
                {
                    if (*hexChar++ != AutoCSer.Extension.Number.ToHex((uint)*start >> 4)) return false;
                    if (*hexChar++ != AutoCSer.Extension.Number.ToHex((uint)*start & 0xf)) return false;
                }
            }
            return true;
        }
    }
}
