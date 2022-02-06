using System;
/*double;float
Desc;*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public unsafe static partial class FixedArrayQuickSort
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static double[] sortDesc(this double[] array)
        {
            if (array == null) return EmptyArray<double>.Array;
            AutoCSer.Algorithm.FixedArrayQuickSort.SortDesc(array);
            return array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static double[] getSortDesc(this double[] array)
        {
            if (array == null) return EmptyArray<double>.Array;
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSortDesc(array);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getSortDesc<valueType>(this valueType[] array, Func<valueType, double> getKey)
        {
            if (array != null)
            {
                if (array.Length > 1) return AutoCSer.Algorithm.FixedArrayQuickSort.GetSortDesc(array, getKey);
                if (array.Length != 0) return new valueType[] { array[0] };
            }
            return EmptyArray<valueType>.Array;
        }
    }
}
