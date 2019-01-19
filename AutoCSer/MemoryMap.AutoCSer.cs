using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 指针位图
    /// </summary>
    public unsafe partial struct MemoryMap
    {
        /// <summary>
        /// 指针位图
        /// </summary>
        /// <param name="map">位图指针,不能为null</param>
        /// <param name="count">整数数量,大于0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ulong* map, int count)
        {
            Map = (byte*)map;
            Memory.ClearUnsafe(map, count);
        }
        /// <summary>
        /// 当占位状态为空时设置占位
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否设置成功</returns>
        public bool SetWhenNullUnsafe(int bit)
        {
            int index = bit >> 3, set = 1 << (bit & 7);
            if ((Map[index] & set) == 0)
            {
                Map[index] |= (byte)set;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置占位段
        /// </summary>
        /// <param name="start">位值</param>
        /// <param name="count">段长</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int start, int count)
        {
            if (start < 0)
            {
                count += start;
                start = 0;
            }
            if (count > 0) Memory.FillBits(Map, start, count);
        }
        /// <summary>
        /// 清除占位
        /// </summary>
        /// <param name="bit">位值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear(int bit)
        {
            Map[bit >> 3] &= (byte)((1 << (bit & 7)) ^ byte.MaxValue);
        }
    }
}
