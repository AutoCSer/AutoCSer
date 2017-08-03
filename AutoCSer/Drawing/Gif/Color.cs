using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 24位色彩
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color : IEquatable<Color>
    {
        /// <summary>
        /// 整数值
        /// </summary>
        [FieldOffset(0)]
        internal int Value;
        /// <summary>
        /// 红色
        /// </summary>
        [FieldOffset(0)]
        public byte Red;
        /// <summary>
        /// 绿色
        /// </summary>
        [FieldOffset(1)]
        public byte Green;
        /// <summary>
        /// 蓝色
        /// </summary>
        [FieldOffset(2)]
        public byte Blue;
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        public override int GetHashCode()
        {
            return Value;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns>是否相等</returns>
        public bool Equals(Color other)
        {
            return Value == other.Value;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((Color)obj);
        }
    }
}
