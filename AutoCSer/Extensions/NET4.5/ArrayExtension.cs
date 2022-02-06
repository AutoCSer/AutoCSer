using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static unsafe partial class ArrayExtension
    {
        /// <summary>
        /// 转换成数组字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static SubArray<T> AsSpan<T>(this T[] array)
        {
            return new SubArray<T>(array);
        }
        /// <summary>
        /// 转换成数组字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static SubArray<T> AsSpan<T>(this T[] array, int start, int count)
        {
            //if (start < 0) throw new IndexOutOfRangeException(start.toString() + " < 0");
            //if (start + count > array.Length) throw new IndexOutOfRangeException(start.toString() + " + " + count.toString() + " > " + array.Length.toString());
            return new SubArray<T>(start, count, array);
        }
    }
}
