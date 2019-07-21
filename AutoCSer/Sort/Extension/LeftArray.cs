using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static partial class LeftArray
    {
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="values">数组子串</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public static int GetCount<valueType>(this LeftArray<valueType> values, Func<valueType, bool> isValue)
        {
            if (values.Length != 0)
            {
                int index = values.Length, count = 0;
                foreach (valueType value in values.Array)
                {
                    if (isValue(value)) ++count;
                    if (--index == 0) break;
                }
                return count;
            }
            return 0;
        }
        /// <summary>
        /// 获取分页字段数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="values">数组子串</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="count"></param>
        /// <returns>分页字段数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetPage<valueType>(this LeftArray<valueType> values, int pageSize, int currentPage, out int count)
        {
            PageCount page = new PageCount(count = values.Length, pageSize, currentPage);
            return new SubArray<valueType>(page.SkipCount, page.CurrentPageSize, values.Array).GetArray();
        }
        /// <summary>
        /// 获取分页字段数组
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="values">数组子串</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        /// <param name="count"></param>
        /// <returns>分页字段数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetPageDesc<valueType>(this LeftArray<valueType> values, int pageSize, int currentPage, out int count)
        {
            PageCount page = new PageCount(count = values.Length, pageSize, currentPage);
            return new SubArray<valueType>(page.DescSkipCount, page.CurrentPageSize, values.Array).GetReverse();
        }

        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<valueType> RangeSort<valueType>
            (this LeftArray<valueType> values, Func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            return Array_Sort.RangeSort(values.Array, 0, values.Length, comparer, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<valueType> GetRangeSort<valueType>
            (this LeftArray<valueType> values, Func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            return Array_Sort.GetRangeSort(values.Array, 0, values.Length, comparer, skipCount, getCount);
        }
    }
}
