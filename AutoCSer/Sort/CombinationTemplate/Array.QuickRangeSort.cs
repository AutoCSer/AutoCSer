using System;
/*ulong;long;uint;int;double;float;DateTime
Desc;*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public unsafe static partial class FixedArrayQuickRangeSort
    {
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> rangeSortDesc(this ulong[] array, int skipCount, int getCount)
        {
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.RangeSortDesc(array, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> getRangeSortDesc(this ulong[] array, int skipCount, int getCount)
        {
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getRangeSortDesc<valueType>(this valueType[] array, Func<valueType, ulong> getKey, int skipCount, int getCount)
        {
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array, getKey, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数,小于0表示所有</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static returnType[] getRangeSortDesc<valueType, returnType>(this valueType[] array, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array, getKey, skipCount, getCount, getValue);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> rangeSortDesc(this ulong[] array, int startIndex, int count, int skipCount, int getCount)
        {
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.RangeSortDesc(array, startIndex, count, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> getRangeSortDesc(this ulong[] array, int startIndex, int count, int skipCount, int getCount)
        {
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array, startIndex, count, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getRangeSortDesc<valueType>(this valueType[] array, int startIndex, int count, Func<valueType, ulong> getKey, int skipCount, int getCount)
        {
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array, startIndex, count, getKey, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">结束位置</param>
        /// <param name="getKey">排序键</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>排序范围数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static returnType[] getRangeSortDesc<valueType, returnType>(this valueType[] array, int startIndex, int count, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.GetRangeSortDesc(array, startIndex, count, getKey, skipCount, getCount, getValue);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">分页号,从 1 开始</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> pageSortDesc(this ulong[] array, int pageSize, int currentPage)
        {
            PageCount page = new PageCount(array.length(), pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeRangeSortDesc(array, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取排序关键字委托</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">分页号,从 1 开始</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getPageSortDesc<valueType>(this valueType[] array, Func<valueType, ulong> getKey, int pageSize, int currentPage)
        {
            PageCount page = new PageCount(array.length(), pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSortDesc(array, getKey, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取排序关键字委托</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">分页号,从 1 开始</param>
        /// <param name="count">数据总量</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getPageSortDesc<valueType>(this valueType[] array, Func<valueType, ulong> getKey, int pageSize, int currentPage, out int count)
        {
            PageCount page = new PageCount(count = array.length(), pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSortDesc(array, getKey, page.SkipCount, page.CurrentPageSize);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取排序关键字委托</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">分页号,从 1 开始</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static returnType[] getPageSortDesc<valueType, returnType>(this valueType[] array, Func<valueType, ulong> getKey, int pageSize, int currentPage, Func<valueType, returnType> getValue)
        {
            PageCount page = new PageCount(array.length(), pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSortDesc(array, getKey, page.SkipCount, page.CurrentPageSize, getValue);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">获取排序关键字委托</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">分页号,从 1 开始</param>
        /// <param name="getValue">获取返回数据</param>
        /// <param name="count">数据总量</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static returnType[] getPageSortDesc<valueType, returnType>(this valueType[] array, Func<valueType, ulong> getKey, int pageSize, int currentPage, Func<valueType, returnType> getValue, out int count)
        {
            PageCount page = new PageCount(count = array.length(), pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSortDesc(array, getKey, page.SkipCount, page.CurrentPageSize, getValue);
        }
        /// <summary>
        /// 分页排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">分页号,从 1 开始</param>
        /// <returns>分页排序数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> getPageSortDesc(this ulong[] array, int pageSize, int currentPage)
        {
            PageCount page = new PageCount(array.length(), pageSize, currentPage);
            return AutoCSer.Algorithm.FixedArrayQuickRangeSort.UnsafeGetRangeSortDesc(array, page.SkipCount, page.CurrentPageSize);
        }
    }
}
