using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// 枚举位图
    /// </summary>
    /// <typeparam name="enumType">枚举类型</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct BitMap<enumType> where enumType : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// 最大值
        /// </summary>
        private static readonly uint size = (uint)AutoCSer.EnumAttribute<enumType>.GetMaxValue(-1) + 1;
        /// <summary>
        /// 位图字节数组
        /// </summary>
        private byte[] map;
        /// <summary>
        /// 枚举位图
        /// </summary>
        /// <param name="map">位图字节数组</param>
        public BitMap(byte[] map)
        {
            this.map = map ?? new byte[(size + 7) >> 3];
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="bit">位值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(int bit)
        {
            map[bit >> 3] |= (byte)(1 << (int)(bit &= 7));
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="value">位值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(enumType value)
        {
            Set(value.ToInt32(null));
        }
        /// <summary>
        /// 清除占位
        /// </summary>
        /// <param name="bit">位值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear(int bit)
        {
            map[bit >> 3] &= (byte)(0xff - (1 << (int)(bit &= 7)));
        }
        /// <summary>
        /// 设置占位
        /// </summary>
        /// <param name="value">位值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear(enumType value)
        {
            Clear(value.ToInt32(null));
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="bit">位值</param>
        /// <returns>是否已占位</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Get(int bit)
        {
            return (map[bit >> 3] & (byte)(1 << (int)(bit &= 7))) != 0;
        }
        /// <summary>
        /// 获取占位状态
        /// </summary>
        /// <param name="value">位值</param>
        /// <returns>是否已占位</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Get(enumType value)
        {
            return Get(value.ToInt32(null));
        }
    }
}
