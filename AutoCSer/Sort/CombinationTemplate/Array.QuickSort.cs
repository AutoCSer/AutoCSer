using System;
/*Type:double,DoubleSortIndex;float,FloatSortIndex*/
/*Compare:,>,<;Desc,<,>*/

namespace AutoCSer.Extension
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
        public static /*Type[0]*/double/*Type[0]*/[] sort/*Compare[0]*//*Compare[0]*/(this /*Type[0]*/double/*Type[0]*/[] array)
        {
            if (array == null) return NullValue</*Type[0]*/double/*Type[0]*/>.Array;
            AutoCSer.Algorithm.FixedArrayQuickSort.Sort/*Compare[0]*//*Compare[0]*/(array);
            return array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/double/*Type[0]*/[] getSort/*Compare[0]*//*Compare[0]*/(this /*Type[0]*/double/*Type[0]*/[] array)
        {
            if (array == null) return NullValue</*Type[0]*/double/*Type[0]*/>.Array;
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSort/*Compare[0]*//*Compare[0]*/(array);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getSort/*Compare[0]*//*Compare[0]*/<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/double/*Type[0]*/> getKey)
        {
            if (array != null)
            {
                if (array.Length > 1) return AutoCSer.Algorithm.FixedArrayQuickSort.GetSort/*Compare[0]*//*Compare[0]*/(array, getKey);
                if (array.Length != 0) return new valueType[] { array[0] };
            }
            return NullValue<valueType>.Array;
        }
    }
}
