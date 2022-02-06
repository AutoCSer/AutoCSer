using System;
/*ulong;long;uint;int;double;float;DateTime
Desc;*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public unsafe static partial class LeftArrayExtensionSort
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
        public static valueType[] GetPageSortDesc<valueType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey, int pageSize, int currentPage, out int count)
        {
            PageCount page = new PageCount(count = array.Length, pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSortDesc(array.Array, 0, count, getKey, page.SkipCount, page.CurrentPageSize);
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
        public static returnType[] GetPageSortDesc<valueType, returnType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey, int pageSize, int currentPage, Func<valueType, returnType> getValue, out int count)
        {
            PageCount page = new PageCount(count = array.Length, pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSortDesc(array.Array, 0, count, getKey, page.SkipCount, page.CurrentPageSize, getValue);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">数组子串</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> RangeSortDesc(this LeftArray<ulong> array, int skipCount, int getCount)
        {
            return array.Length == 0 ? new SubArray<ulong>() : AutoCSer.Algorithm.FixedArrayQuickRangeSort.RangeSortDesc(array.Array, 0, array.Length, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">数组子串</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> GetRangeSortDesc(this LeftArray<ulong> array, int skipCount, int getCount)
        {
            return array.Length == 0 ? new SubArray<ulong>() : AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array.Array, 0, array.Length, skipCount, getCount);
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
        public static valueType[] GetRangeSortDesc<valueType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey, int skipCount, int getCount)
        {
            return array.Length == 0 ? EmptyArray<valueType>.Array : AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array.Array, 0, array.Length, getKey, skipCount, getCount);
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
        public static returnType[] GetRangeSortDesc<valueType, returnType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            return array.Length == 0 ? EmptyArray<returnType>.Array : AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array.Array, 0, array.Length, getKey, skipCount, getCount, getValue);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">数组子串</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> PageSortDesc(this LeftArray<ulong> array, int pageSize, int currentPage)
        {
            PageCount page = new PageCount(array.Length, pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeRangeSortDesc(array.Array, 0, array.Length, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">数组子串</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> GetPageSortDesc(this LeftArray<ulong> array, int pageSize, int currentPage)
        {
            PageCount page = new PageCount(array.Length, pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSortDesc(array.Array, 0, array.Length, page.SkipCount, page.CurrentPageSize);
        }
    }
}
