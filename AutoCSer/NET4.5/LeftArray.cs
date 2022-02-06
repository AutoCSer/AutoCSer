using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组子串
    /// </summary>
    public partial struct LeftArray<T>
    {
        /// <summary>
        /// 转换为数组子串
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SubArray<T> AsSpan()
        {
            return new SubArray<T>(Array, 0, Length);
        }
    }
}
