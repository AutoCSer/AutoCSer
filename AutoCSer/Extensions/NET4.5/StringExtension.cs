using AutoCSer.Memory;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 字符串相关操作
    /// </summary>
    internal unsafe static partial class StringExtension
    {
        /// <summary>
        /// 转换成字符子串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static SubString AsSpan(this string value)
        {
            return new SubString(value);
        }
        /// <summary>
        /// 转换成字符子串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static SubString AsSpan(this string value, int start, int count)
        {
            return new SubString(start, count, value);
        }
    }
}
