using System;
/*Type:ulong,ULongRangeSorter,ULongSortIndex,ULongRangeIndexSorter;long,LongRangeSorter,LongSortIndex,LongRangeIndexSorter;uint,UIntRangeSorter,UIntSortIndex,UIntRangeIndexSorter;int,IntRangeSorter,IntSortIndex,IntRangeIndexSorter;double,DoubleRangeSorter,DoubleSortIndex,DoubleRangeIndexSorter;float,FloatRangeSorter,FloatSortIndex,FloatRangeIndexSorter;DateTime,DateTimeRangeSorter,DateTimeSortIndex,DateTimeRangeIndexSorter*/
/*Compare:,>,<;Desc,<,>*/

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public unsafe static partial class LeftArray
    {
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取排序关键字委托</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="count">数据总量</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetPageSort/*Compare[0]*//*Compare[0]*/<valueType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int pageSize, int currentPage, out int count)
        {
            PageCount page = new PageCount(count = array.Length, pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, count, getKey, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取排序关键字委托</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="getValue">获取返回数据</param>
        /// <param name="count">数据总量</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static returnType[] GetPageSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int pageSize, int currentPage, Func<valueType, returnType> getValue, out int count)
        {
            PageCount page = new PageCount(count = array.Length, pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, count, getKey, page.SkipCount, page.CurrentPageSize, getValue);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">数组子串</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> RangeSort/*Compare[0]*//*Compare[0]*/(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, int skipCount, int getCount)
        {
            return array.Length == 0 ? default(SubArray</*Type[0]*/ulong/*Type[0]*/>) : AutoCSer.Algorithm.FixedArrayQuickRangeSort.RangeSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">数组子串</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> GetRangeSort/*Compare[0]*//*Compare[0]*/(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, int skipCount, int getCount)
        {
            return array.Length == 0 ? default(SubArray</*Type[0]*/ulong/*Type[0]*/>) : AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组子串</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            return array.Length == 0 ? NullValue<valueType>.Array : AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length, getKey, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">数组子串</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static returnType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            return array.Length == 0 ? NullValue<returnType>.Array : AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length, getKey, skipCount, getCount, getValue);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">数组子串</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> PageSort/*Compare[0]*//*Compare[0]*/(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, int pageSize, int currentPage)
        {
            PageCount page = new PageCount(array.Length, pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeRangeSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">数组子串</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> GetPageSort/*Compare[0]*//*Compare[0]*/(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, int pageSize, int currentPage)
        {
            PageCount page = new PageCount(array.Length, pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array.Array, 0, array.Length, page.SkipCount, page.CurrentPageSize);
        }
    }
}
