using System;
/*double;float
Desc;*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public unsafe static partial class LeftArrayExtensionSort
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<double> SortDesc(this LeftArray<double> array)
        {
            if (array.Length > 1) AutoCSer.Algorithm.FixedArrayQuickSort.SortDesc(array.Array, 0, array.Length);
            return array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static double[] GetSortDesc(this LeftArray<double> array)
        {
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSortDesc(array.Array, 0, array.Length);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetSortDesc<valueType>(this LeftArray<valueType> array, Func<valueType, double> getKey)
        {
            if (array.Length > 1) return AutoCSer.Algorithm.FixedArrayQuickSort.GetSortDesc(array.Array, getKey, 0, array.Length);
            return array.Length == 0 ? EmptyArray<valueType>.Array : new valueType[] { array.Array[0] };
        }
    }
}
