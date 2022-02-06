using System;
/*ulong;long;uint;int;double;float;DateTime*/

namespace AutoCSer
{
    /// <summary>
    /// 数据反向比较
    /// </summary>
    internal static partial class CompareFromExtensions
    {
        /// <summary>
        /// 数据反向比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int CompareFrom(this ulong left, ulong right)
        {
            return right.CompareTo(left);
        }
    }
}
