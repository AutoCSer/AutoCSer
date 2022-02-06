using System;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字节数组相关扩展操作
    /// </summary>
    internal static class MemoryExtensionCacheServer
    {
        /// <summary>
        /// 转换为 16 进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static unsafe string toHex(this byte[] data)
        {
            string value = AutoCSer.Extensions.StringExtension.FastAllocateString(data.Length << 1);
            fixed (char* valueFixed = value)
            {
                char* write = valueFixed;
                foreach (byte code in data)
                {
                    uint code32 = code;
                    *write = (char)AutoCSer.Extensions.NumberExtension.ToHex(code32 >> 4);
                    *(write + 1) = (char)AutoCSer.Extensions.NumberExtension.ToHex(code32 & 0xf);
                    write += 2;
                }
            }
            return value;
        }
    }
}
