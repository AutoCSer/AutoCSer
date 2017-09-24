using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal unsafe static class Memory_RawSocketListener
    {
        /// <summary>
        /// 字节流转32位无符号整数
        /// </summary>
        /// <param name="values">字节数组,不能为null</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>无符号整数值</returns>
        public unsafe static uint GetUIntBigEndian(this byte[] values, int startIndex)
        {
            fixed (byte* value = values)
            {
                byte* start = value + startIndex;
                return ((uint)*start << 24) + ((uint)*(start + 1) << 16) + ((uint)*(start + 2) << 8) + (uint)*(start + 3);
            }
        }
    }
}
