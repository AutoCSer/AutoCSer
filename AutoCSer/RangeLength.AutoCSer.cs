using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// 范围
    /// </summary>
    internal partial struct RangeLength
    {
        /// <summary>
        /// 设置范围
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="length">数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}
