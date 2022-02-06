using System;
/*ulong;long;uint;int;DateTime
Desc;*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展操作
    /// </summary>
    public static partial class LeftArrayExtensionSort
    {
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <returns>排序后的数组子串</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<ulong> SortDesc(this LeftArray<ulong> array)
        {
            if (array.Length > 1) FixedArraySort.SortDesc(array.Array, 0, array.Length);
            return array;
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] GetSortDesc(this LeftArray<ulong> array)
        {
            return array.Length > 1 ? FixedArraySort.GetSortDesc(array.Array, 0, array.Length) : array.GetArray();
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组子串</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetSortDesc<valueType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey)
        {
            return array.Length > 1 ? FixedArraySort.GetSortDesc(array.Array, getKey, 0, array.Length) : array.GetArray();
        }
    }
}
