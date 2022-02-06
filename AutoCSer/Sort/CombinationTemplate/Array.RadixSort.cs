using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;DateTime,DateTime
Desc;*/

namespace AutoCSer.Extensions
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
        internal static void SortDesc(ulong[] array, int index, int count)
        {
            if (array.Length >= AutoCSer.Algorithm.RadixSort.SortSize64) AutoCSer.Algorithm.RadixSort.SortDesc(array, index, count);
            else AutoCSer.Algorithm.FixedArrayQuickSort.SortDesc(array, index, count);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] sortDesc(this ulong[] array)
        {
            if (array == null) return EmptyArray<ulong>.Array;
            if (array.Length > 1) SortDesc(array, 0, array.Length);
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
        public static SubArray<ulong> sortDesc(this ulong[] array, int index, int count)
        {
            if (array == null) return new SubArray<ulong>();
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            if (index + count > array.Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > array.Length[" + array.Length.toString() + "]");
            if (count > 1) SortDesc(array, index, count);
            return new SubArray<ulong> { Array = array, Start = index, Length = count };
        }
        /// <summary>
        /// 数组子串排序
        /// </summary>
        /// <param name="array">待排序数组子串</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ulong[] GetSortDesc(ulong[] array, int index, int count)
        {
            if (count >= AutoCSer.Algorithm.RadixSort.SortSize64) return AutoCSer.Algorithm.RadixSort.GetSortDesc(array, index, count);
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSortDesc(array, index, count);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] getSortDesc(this ulong[] array)
        {
            if (array == null) return EmptyArray<ulong>.Array;
            return array.Length > 1 ? GetSortDesc(array, 0, array.Length) : array.copy();
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] getSortDesc(this ulong[] array, int index, int count)
        {
            if (array == null || count == 0) return EmptyArray<ulong>.Array;
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            if (index + count > array.Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > array.Length[" + array.Length.toString() + "]");
            if (count == 1) return new ulong[] { array[index] };
            return GetSortDesc(array, index, count);
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
        internal static valueType[] GetSortDesc<valueType>(valueType[] array, Func<valueType, ulong> getKey, int index, int count)
        {
            if (count >= AutoCSer.Algorithm.RadixSort.SortSize64)
            {
                int size = (count << 1) * sizeof(AutoCSer.Algorithm.ULongSortIndex);
                AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(size);
                AutoCSer.Memory.Pointer data = pool.GetMinSize(size);
                try
                {
                    AutoCSer.Algorithm.ULongSortIndex* indexFixed = (AutoCSer.Algorithm.ULongSortIndex*)data.Data;
                    AutoCSer.Algorithm.ULongSortIndex.Create(indexFixed, array, getKey, index, count);
                    AutoCSer.Algorithm.RadixSort.SortDesc(indexFixed, indexFixed + count, count);
                    return AutoCSer.Algorithm.ULongSortIndex.Create(indexFixed, array, count);
                }
                finally { pool.Push(ref data); }
            }
            return AutoCSer.Algorithm.FixedArrayQuickSort.GetSortDesc(array, getKey, index, count);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getSortDesc<valueType>(this valueType[] array, Func<valueType, ulong> getKey)
        {
            if (array == null) return EmptyArray<valueType>.Array;
            return array.Length > 1 ? GetSortDesc(array, getKey, 0, array.Length) : array.copy();
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
        public static valueType[] getSortDesc<valueType>(this valueType[] array, Func<valueType, ulong> getKey, int index, int count)
        {
            if (array == null || count == 0) return EmptyArray<valueType>.Array;
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            if (index + count > array.Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > array.Length[" + array.Length.toString() + "]");
            if (count == 1) return new valueType[] { array[index] };
            return GetSortDesc(array, getKey, index, count);
        }
    }
}
