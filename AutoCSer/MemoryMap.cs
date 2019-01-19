using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 指针位图
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe partial struct MemoryMap
    {
        /// <summary>
        /// 位图指针
        /// </summary>
        internal byte* Map;
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        public MemoryMap(void* map)
        {
            Map = (byte*)map;
        }
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        /// <param name="count">整数数量,大于0</param>
        internal MemoryMap(ulong* map, int count)
        {
            Map = (byte*)map;
            Memory.ClearUnsafe(map, count);
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否已占位</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int Get(int bit)
        {
            return Map[bit >> 3] & (1 << (bit & 7));
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否已占位</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int Get(char bit)
        {
            return Map[bit >> 3] & (1 << (bit & 7));
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="bit">位值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int bit)
        {
            Map[bit >> 3] |= (byte)(1 << (bit & 7));
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="bit">位值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(char bit)
        {
            Map[bit >> 3] |= (byte)(1 << (bit & 7));
        }
    }
}
