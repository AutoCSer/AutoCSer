using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 数组位图
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct DeSerializeArrayMap
    {
        /// <summary>
        /// 当前位
        /// </summary>
        public uint Bit;
        /// <summary>
        /// 当前位图
        /// </summary>
        public uint Map;
        /// <summary>
        /// 当前读取位置
        /// </summary>
        public byte* Read;
        /// <summary>
        /// 数组位图
        /// </summary>
        /// <param name="read">当前读取位置</param>
        public DeSerializeArrayMap(byte* read)
        {
            Read = read;
            Bit = 1;
            Map = 0;
        }
        /// <summary>
        /// 数组位图
        /// </summary>
        /// <param name="read">当前读取位置</param>
        /// <param name="bit">当前位</param>
        public DeSerializeArrayMap(byte* read, uint bit)
        {
            Read = read;
            Bit = bit;
            Map = 0;
        }
        /// <summary>
        /// 获取位图数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public uint Next()
        {
            if (Bit == 1)
            {
                Map = *(uint*)Read;
                Bit = 1U << 31;
                Read += sizeof(uint);
            }
            else Bit >>= 1;
            return Map & Bit;
        }
        /// <summary>
        /// 获取位图数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool? NextBool()
        {
            if (Bit == 2)
            {
                Map = *(uint*)Read;
                Bit = 1U << 31;
                Read += sizeof(uint);
            }
            else Bit >>= 2;
            if ((Map & Bit) == 0) return null;
            return (Map & (Bit >> 1)) != 0;
        }
    }
}
