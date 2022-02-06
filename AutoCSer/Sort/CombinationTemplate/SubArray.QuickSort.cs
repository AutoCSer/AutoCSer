using System;
/*double;float
Desc;*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public unsafe static partial class SubArrayExtension
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<double> SortDesc(this SubArray<double> array)
        {
            if (array.Length > 1) AutoCSer.Algorithm.FixedArrayQuickSort.SortDesc(array.Array, array.Start, array.Length);
            return array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static double[] GetSortDesc(this SubArray<double> array)
        {
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSortDesc(array.Array, array.Start, array.Length);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetSortDesc<valueType>(this SubArray<valueType> array, Func<valueType, double> getKey)
        {
            if (array.Length > 1) return AutoCSer.Algorithm.FixedArrayQuickSort.GetSortDesc(array.Array, getKey, array.Start, array.Length);
            return array.Length == 0 ? EmptyArray<valueType>.Array : new valueType[] { array.Array[array.Start] };
        }
    }
}
