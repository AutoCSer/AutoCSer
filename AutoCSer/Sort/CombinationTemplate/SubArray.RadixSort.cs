using System;
/*ulong;long;uint;int;DateTime
Desc;*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展操作
    /// </summary>
    public static partial class SubArrayExtension
    {
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <returns>排序后的数组子串</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> SortDesc(this SubArray<ulong> array)
        {
            if (array.Length > 1) FixedArraySort.SortDesc(array.Array, array.Start, array.Length);
            return array;
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] GetSortDesc(this SubArray<ulong> array)
        {
            return array.Length > 1 ? FixedArraySort.GetSortDesc(array.Array, array.Start, array.Length) : array.GetArray();
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组子串</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetSortDesc<valueType>(this SubArray<valueType> array, Func<valueType, ulong> getKey)
        {
            return array.Length > 1 ? FixedArraySort.GetSortDesc(array.Array, getKey, array.Start, array.Length) : array.GetArray();
        }
    }
}
