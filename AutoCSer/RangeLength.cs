using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// 范围
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(int) * 2)]
    internal partial struct RangeLength
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        [FieldOffset(0)]
        public int Length;
        /// <summary>
        /// 数量
        /// </summary>
        [FieldOffset(sizeof(uint))]
        public int Start;
        /// <summary>
        /// 范围
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="length">数量</param>
        public RangeLength(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}
