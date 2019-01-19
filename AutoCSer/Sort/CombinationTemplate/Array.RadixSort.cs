using System;
/*Type:ulong,ULongSortIndex;long,LongSortIndex;uint,UIntSortIndex;int,IntSortIndex;DateTime,DateTimeSortIndex*/
/*Compare:;Desc*/

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static unsafe partial class FixedArraySort
    {
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Sort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int index, int count)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize64) AutoCSer.Algorithm.RadixSort.Sort/*Compare[0]*//*Compare[0]*/(array, index, count);
            else AutoCSer.Algorithm.FixedArrayQuickSort.Sort/*Compare[0]*//*Compare[0]*/(array, index, count);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/ulong/*Type[0]*/[] sort/*Compare[0]*//*Compare[0]*/(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            if (array == null) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            if (array.Length > 1) Sort/*Compare[0]*//*Compare[0]*/(array, 0, array.Length);
            return array;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> sort/*Compare[0]*//*Compare[0]*/(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count)
        {
            if (array == null) return default(SubArray</*Type[0]*/ulong/*Type[0]*/>);
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            if (index + count > array.Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > array.Length[" + array.Length.toString() + "]");
            if (count > 1) Sort/*Compare[0]*//*Compare[0]*/(array, index, count);
            return new SubArray</*Type[0]*/ulong/*Type[0]*/> { Array = array, Start = index, Length = count };
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static /*Type[0]*/ulong/*Type[0]*/[] GetSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int index, int count)
        {
            if (count >= AutoCSer.Algorithm.RadixSort.SortSize64) return AutoCSer.Algorithm.RadixSort.GetSort/*Compare[0]*//*Compare[0]*/(array, index, count);
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSort/*Compare[0]*//*Compare[0]*/(array, index, count);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/ulong/*Type[0]*/[] getSort/*Compare[0]*//*Compare[0]*/(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            if (array == null) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            return array.Length > 1 ? GetSort/*Compare[0]*//*Compare[0]*/(array, 0, array.Length) : array.copy();
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/ulong/*Type[0]*/[] getSort/*Compare[0]*//*Compare[0]*/(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int count)
        {
            if (array == null || count == 0) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            if (index + count > array.Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > array.Length[" + array.Length.toString() + "]");
            if (count == 1) return new /*Type[0]*/ulong/*Type[0]*/[] { array[index] };
            return GetSort/*Compare[0]*//*Compare[0]*/(array, index, count);
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组子串</param>
        /// <param name="getKey">排序键</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的数组</returns>
        internal static valueType[] GetSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int index, int count)
        {
            if (count >= AutoCSer.Algorithm.RadixSort.SortSize64)
            {
                int size = (count << 1) * sizeof(AutoCSer.Algorithm./*Type[1]*/ULongSortIndex/*Type[1]*/);
                UnmanagedPool pool = AutoCSer.Algorithm.RadixSort.GetUnmanagedPool(size);
                Pointer.Size data = pool.GetSize(size);
                try
                {
                    AutoCSer.Algorithm./*Type[1]*/ULongSortIndex/*Type[1]*/* indexFixed = (AutoCSer.Algorithm./*Type[1]*/ULongSortIndex/*Type[1]*/*)data.Data;
                    AutoCSer.Algorithm./*Type[1]*/ULongSortIndex/*Type[1]*/.Create(indexFixed, array, getKey, index, count);
                    AutoCSer.Algorithm.RadixSort.Sort/*Compare[0]*//*Compare[0]*/(indexFixed, indexFixed + count, count);
                    return AutoCSer.Algorithm./*Type[1]*/ULongSortIndex/*Type[1]*/.Create(indexFixed, array, count);
                }
                finally { pool.PushOnly(ref data); }
            }
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSort/*Compare[0]*//*Compare[0]*/(array, getKey, index, count);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getSort/*Compare[0]*//*Compare[0]*/<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey)
        {
            if (array == null) return NullValue<valueType>.Array;
            return array.Length > 1 ? GetSort/*Compare[0]*//*Compare[0]*/(array, getKey, 0, array.Length) : array.copy();
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getSort/*Compare[0]*//*Compare[0]*/<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int index, int count)
        {
            if (array == null || count == 0) return NullValue<valueType>.Array;
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            if (index + count > array.Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > array.Length[" + array.Length.toString() + "]");
            if (count == 1) return new valueType[] { array[index] };
            return GetSort/*Compare[0]*//*Compare[0]*/(array, getKey, index, count);
        }
    }
}
