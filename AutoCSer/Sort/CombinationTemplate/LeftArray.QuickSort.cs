using System;
/*Type:double,DoubleSortIndex;float,FloatSortIndex*/
/*Compare:,>,<;Desc,<,>*/

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public unsafe static partial class LeftArray
    {
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray</*Type[0]*/double/*Type[0]*/> Sort/*Compare[0]*//*Compare[0]*/(this LeftArray</*Type[0]*/double/*Type[0]*/> array)
        {
            if (array.Length > 1) AutoCSer.Algorithm.FixedArrayQuickSort.Sort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length);
            return array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/double/*Type[0]*/[] GetSort/*Compare[0]*//*Compare[0]*/(this LeftArray</*Type[0]*/double/*Type[0]*/> array)
        {
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetSort/*Compare[0]*//*Compare[0]*/<valueType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/double/*Type[0]*/> getKey)
        {
            if (array.Length > 1) return AutoCSer.Algorithm.FixedArrayQuickSort.GetSort/*Compare[0]*//*Compare[0]*/(array.Array, getKey, 0, array.Length);
            return array.Length == 0 ? NullValue<valueType>.Array : new valueType[] { array.Array[0] };
        }
    }
}
