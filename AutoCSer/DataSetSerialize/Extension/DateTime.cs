using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 时间信息扩展
    /// </summary>
    internal static class DateTime_DataSetSerialize
    {
        /// <summary>
        /// 时间转换成时钟周期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>时钟周期</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ulong toKindTicks(this DateTime date)
        {
            return (ulong)date.Ticks + ((ulong)(int)date.Kind << 0x3e);
        }
        /// <summary>
        /// 时钟周期转换时间
        /// </summary>
        /// <param name="value">时钟周期</param>
        /// <returns>时间</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DateTime FromKindTicks(ulong value)
        {
            return new DateTime((long)(value & 0x3fffffffffffffffL), (DateTimeKind)(int)(value >> 0x3e));
        }
    }
}
