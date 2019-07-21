using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static partial class Array_Sort
    {
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static SubArray<valueType> RangeSort<valueType>
            (this valueType[] values, Func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            FormatRange range = new FormatRange(values.length(), skipCount, getCount);
            if ((getCount = range.GetCount) != 0)
            {
                if (comparer == null) throw new ArgumentNullException();
                new AutoCSer.Algorithm.RangeQuickSort.Sorter<valueType>
                {
                    Array = values,
                    Comparer = comparer,
                    SkipCount = range.SkipCount,
                    GetEndIndex = range.EndIndex - 1
                }.Sort(0, values.Length - 1);
                return new SubArray<valueType>(range.SkipCount, getCount, values);
            }
            return default(SubArray<valueType>);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        public static SubArray<valueType> GetRangeSort<valueType>
            (this valueType[] values, Func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            FormatRange range = new FormatRange(values.length(), skipCount, getCount);
            if ((getCount = range.GetCount) != 0)
            {
                if (comparer == null) throw new ArgumentNullException();
                AutoCSer.Algorithm.RangeQuickSort.Sorter<valueType> sorter = new AutoCSer.Algorithm.RangeQuickSort.Sorter<valueType>
                {
                    Array = values.copy(),
                    Comparer = comparer,
                    SkipCount = range.SkipCount,
                    GetEndIndex = range.EndIndex - 1
                };
                sorter.Sort(0, values.Length - 1);
                return new SubArray<valueType>(range.SkipCount, getCount, sorter.Array);
            }
            return default(SubArray<valueType>);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static SubArray<valueType> RangeSort<valueType>
            (this valueType[] values, int startIndex, int count, Func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            FormatRange range = new FormatRange(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                FormatRange getRange = new FormatRange(count, skipCount, getCount);
                if ((getCount = getRange.GetCount) != 0)
                {
                    if (comparer == null) throw new ArgumentNullException();
                    skipCount = range.SkipCount + getRange.SkipCount;
                    new AutoCSer.Algorithm.RangeQuickSort.Sorter<valueType>
                    {
                        Array = values,
                        Comparer = comparer,
                        SkipCount = skipCount,
                        GetEndIndex = skipCount + getCount - 1
                    }.Sort(range.SkipCount, range.SkipCount + --count);
                    return new SubArray<valueType>(skipCount, getCount, values);
                }
            }
            return default(SubArray<valueType>);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static SubArray<valueType> GetRangeSort<valueType>
            (this valueType[] values, int startIndex, int count, Func<valueType, valueType, int> comparer, int skipCount, int getCount)
        {
            FormatRange range = new FormatRange(values.length(), startIndex, count);
            if ((count = range.GetCount) != 0)
            {
                FormatRange getRange = new FormatRange(count, skipCount, getCount);
                if ((getCount = getRange.GetCount) != 0)
                {
                    valueType[] newValues = new valueType[count];
                    Array.Copy(values, range.SkipCount, newValues, 0, count);
                    return RangeSort(newValues, comparer, getRange.SkipCount, getCount);
                }
            }
            return default(SubArray<valueType>);
        }


        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static SubArray<valueType> GetTop<valueType>(this valueType[] values, Func<valueType, valueType, int> comparer, int count)
        {
            if (values == null) return default(SubArray<valueType>);
            if (comparer == null) throw new ArgumentNullException();
            if (count > 0)
            {
                if (count < values.Length)
                {
                    if (count <= values.Length >> 1) return getTop(values, comparer, count);
                    values = getRemoveTop(values, comparer, count);
                }
                else
                {
                    valueType[] newValues = new valueType[values.Length];
                    Array.Copy(values, 0, newValues, 0, values.Length);
                    values = newValues;
                }
                return new SubArray<valueType>(0, values.Length, values);
            }
            return default(SubArray<valueType>);
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static SubArray<valueType> Top<valueType>(this valueType[] values, Func<valueType, valueType, int> comparer, int count)
        {
            if (values == null) return default(SubArray<valueType>);
            if (comparer == null) throw new ArgumentNullException();
            if (count > 0)
            {
                if (count < values.Length)
                {
                    if (count <= values.Length >> 1) return getTop(values, comparer, count);
                    values = getRemoveTop(values, comparer, count);
                }
                return new SubArray<valueType>(0, values.Length, values);
            }
            return default(SubArray<valueType>);
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static SubArray<valueType> getTop<valueType>(this valueType[] values, Func<valueType, valueType, int> comparer, int count)
        {
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)Number.sqrt((uint)values.Length, out sqrtMod)), values.Length);
            valueType[] newValues = new valueType[length];
            int readIndex = values.Length - length, writeIndex = count;
            Array.Copy(values, readIndex, newValues, 0, length);
            AutoCSer.Algorithm.RangeQuickSort.Sorter<valueType> sort = new AutoCSer.Algorithm.RangeQuickSort.Sorter<valueType> { Array = newValues, Comparer = comparer, SkipCount = count - 1, GetEndIndex = count - 1 };
            sort.Sort(0, --length);
            for (valueType maxValue = newValues[sort.GetEndIndex]; readIndex != 0; )
            {
                if (comparer(values[--readIndex], maxValue) < 0)
                {
                    newValues[writeIndex] = values[readIndex];
                    if (writeIndex == length)
                    {
                        sort.Sort(0, length);
                        writeIndex = count;
                        maxValue = newValues[sort.GetEndIndex];
                    }
                    else ++writeIndex;
                }
            }
            if (writeIndex != count) sort.Sort(0, writeIndex - 1);
            Array.Clear(newValues, count, newValues.Length - count);
            return new SubArray<valueType>(0, count, newValues);
        }
        /// <summary>
        /// 排序去除Top N
        /// </summary>
        /// <typeparam name="valueType">排序数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="comparer">排序比较器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static valueType[] getRemoveTop<valueType>(this valueType[] values, Func<valueType, valueType, int> comparer, int count)
        {
            valueType[] newValues = new valueType[count];
            count = values.Length - count;
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)Number.sqrt((uint)values.Length, out sqrtMod)), values.Length);
            valueType[] removeValues = new valueType[length];
            int readIndex = values.Length - length, copyCount = length - count, removeIndex = copyCount, writeIndex = copyCount;
            Array.Copy(values, readIndex, removeValues, 0, length);
            AutoCSer.Algorithm.RangeQuickSort.Sorter<valueType> sort = new AutoCSer.Algorithm.RangeQuickSort.Sorter<valueType> { Array = removeValues, Comparer = comparer, SkipCount = copyCount, GetEndIndex = copyCount };
            sort.Sort(0, --length);
            Array.Copy(removeValues, 0, newValues, 0, copyCount);
            for (valueType maxValue = removeValues[copyCount]; readIndex != 0; )
            {
                if (comparer(values[--readIndex], maxValue) <= 0) newValues[writeIndex++] = values[readIndex];
                else
                {
                    removeValues[--removeIndex] = values[readIndex];
                    if (removeIndex == 0)
                    {
                        sort.Sort(0, length);
                        removeIndex = copyCount;
                        maxValue = removeValues[copyCount];
                        Array.Copy(removeValues, 0, newValues, writeIndex, copyCount);
                        writeIndex += copyCount;
                    }
                }
            }
            if (removeIndex != copyCount)
            {
                sort.Sort(removeIndex, length);
                Array.Copy(removeValues, removeIndex, newValues, writeIndex, copyCount - removeIndex);
            }
            return newValues;
        }

        #region 二分查找第一个匹配值位置
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryIndexOf<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            int index = binaryIndexOfLess(values, value, IComparableExtension<valueType>.CompareToHandle);
            return index != values.Length && IComparableExtension<valueType>.CompareToHandle(value, values[index]) == 0 ? index : -1;
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryIndexOf<valueType>(this valueType[] values, valueType value
            , Func<valueType, valueType, int> comparer)
        {
            int index = binaryIndexOfLess(values, value, comparer);
            return index != values.Length && comparer(value, values[index]) == 0 ? index : -1;
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , Func<keyType, valueType, int> comparer, bool isAscending)
        {
            int index = binaryIndexOfLess(values, value, comparer, isAscending ? SortOrder<valueType>.Ascending : SortOrder<valueType>.Descending);
            return index != values.Length && comparer(value, values[index]) == 0 ? index : -1;
        }
        /// <summary>
        /// 二分查找第一个匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , Func<keyType, valueType, int> comparer, Func<valueType, valueType, int> orderComparer)
        {
            int index = binaryIndexOfLess(values, value, comparer, orderComparer);
            return index != values.Length && comparer(value, values[index]) == 0 ? index : -1;
        }
        #endregion

        #region 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值之前的位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryIndexOfLess<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            return binaryIndexOfLess(values, value, IComparableExtension<valueType>.CompareToHandle);
        }
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值之前的位置,失败返回-1</returns>
        public static int binaryIndexOfLess<valueType>(this valueType[] values, valueType value, Func<valueType, valueType, int> comparer)
        {
            if (values != null && values.Length != 0)
            {
                int start = 0, length = values.Length, average;
                if (comparer(values[0], values[length - 1]) <= 0)
                {
                    do
                    {
                        if (comparer(value, values[average = start + ((length - start) >> 1)]) > 0) start = average + 1;
                        else length = average;
                    }
                    while (start != length);
                }
                else
                {
                    do
                    {
                        if (comparer(value, values[average = start + ((length - start) >> 1)]) < 0) start = average + 1;
                        else length = average;
                    }
                    while (start != length);
                }
                return start;
            }
            return 0;
        }
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值之前的位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryIndexOfLess<keyType, valueType>(this valueType[] values, keyType value
            , Func<keyType, valueType, int> comparer
            , bool isAscending)
        {
            return binaryIndexOfLess(values, value, comparer, isAscending ? SortOrder<valueType>.Ascending : SortOrder<valueType>.Descending);
        }
        /// <summary>
        /// 二分查找匹配值之前的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值之前的位置,失败返回-1</returns>
        public static int binaryIndexOfLess<keyType, valueType>(this valueType[] values, keyType value
            , Func<keyType, valueType, int> comparer
            , Func<valueType, valueType, int> orderComparer)
        {
            if (values != null && values.Length != 0)
            {
                int start = 0, length = values.Length, average;
                if (orderComparer(values[0], values[length - 1]) <= 0)
                {
                    do
                    {
                        if (comparer(value, values[average = start + ((length - start) >> 1)]) > 0) start = average + 1;
                        else length = average;
                    }
                    while (start != length);
                }
                else
                {
                    do
                    {
                        if (comparer(value, values[average = start + ((length - start) >> 1)]) < 0) start = average + 1;
                        else length = average;
                    }
                    while (start != length);
                }
                return start;
            }
            return 0;
        }
        #endregion

        #region 二分查找最后一个匹配值位置
        /// <summary>
        /// 二分查找最后一个匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryLastIndexOf<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            int index = binaryIndexOfThan(values, value, IComparableExtension<valueType>.CompareToHandle);
            return index != 0 && IComparableExtension<valueType>.CompareToHandle(value, values[--index]) == 0 ? index : -1;
        }
        /// <summary>
        /// 二分查找最后一个匹配值位置
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryLastIndexOf<valueType>(this valueType[] values, valueType value, Func<valueType, valueType, int> comparer)
        {
            int index = binaryIndexOfThan(values, value, comparer);
            return index != 0 && comparer(value, values[--index]) == 0 ? index : -1;
        }
        /// <summary>
        /// 二分查找最后一个匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryLastIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , Func<keyType, valueType, int> comparer
            , bool isAscending)
        {
            int index = binaryIndexOfThan(values, value, comparer, isAscending ? SortOrder<valueType>.Ascending : SortOrder<valueType>.Descending);
            return index != 0 && comparer(value, values[--index]) == 0 ? index : -1;
        }
        /// <summary>
        /// 二分查找最后一个匹配值位置
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryLastIndexOf<keyType, valueType>(this valueType[] values, keyType value
            , Func<keyType, valueType, int> comparer
            , Func<valueType, valueType, int> orderComparer)
        {
            int index = binaryIndexOfThan(values, value, comparer, orderComparer);
            return index != 0 && comparer(value, values[--index]) == 0 ? index : -1;
        }
        #endregion

        #region 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// <summary>
        /// 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <returns>匹配值之后的位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryIndexOfThan<valueType>(this valueType[] values, valueType value)
            where valueType : IComparable<valueType>
        {
            return binaryIndexOfThan(values, value, IComparableExtension<valueType>.CompareToHandle);
        }
        /// <summary>
        /// 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">数组值比较器</param>
        /// <returns>匹配值之后的位置,失败返回-1</returns>
        public static int binaryIndexOfThan<valueType>(this valueType[] values, valueType value, Func<valueType, valueType, int> comparer)
        {
            if (values != null && values.Length != 0)
            {
                int start = 0, length = values.Length, average;
                if (comparer(values[0], values[length - 1]) <= 0)
                {
                    do
                    {
                        if (comparer(value, values[average = start + ((length - start) >> 1)]) < 0) length = average;
                        else start = average + 1;
                    }
                    while (start != length);
                }
                else
                {
                    do
                    {
                        if (comparer(value, values[average = start + ((length - start) >> 1)]) > 0) length = average;
                        else start = average + 1;
                    }
                    while (start != length);
                }
                return start;
            }
            return 0;
        }
        /// <summary>
        /// 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>匹配值之后的位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int binaryIndexOfThan<keyType, valueType>(this valueType[] values, keyType value
            , Func<keyType, valueType, int> comparer
            , bool isAscending)
        {
            return binaryIndexOfThan(values, value, comparer, isAscending ? SortOrder<valueType>.Ascending : SortOrder<valueType>.Descending);
        }
        /// <summary>
        /// 二分查找匹配值之后的位置(用于查找插入值的位置)
        /// </summary>
        /// <typeparam name="keyType">查找值类型</typeparam>
        /// <typeparam name="valueType">数组值类型</typeparam>
        /// <param name="value">匹配值</param>
        /// <param name="values">匹配数组</param>
        /// <param name="comparer">查找值比较器</param>
        /// <param name="orderComparer">数组值比较器</param>
        /// <returns>匹配值之后的位置,失败返回-1</returns>
        public static int binaryIndexOfThan<keyType, valueType>(this valueType[] values, keyType value
            , Func<keyType, valueType, int> comparer
            , Func<valueType, valueType, int> orderComparer)
        {
            if (values != null && values.Length != 0)
            {
                int start = 0, length = values.Length, average;
                if (orderComparer(values[0], values[length - 1]) <= 0)
                {
                    do
                    {
                        if (comparer(value, values[average = start + ((length - start) >> 1)]) < 0) length = average;
                        else start = average + 1;
                    }
                    while (start != length);
                }
                else
                {
                    do
                    {
                        if (comparer(value, values[average = start + ((length - start) >> 1)]) > 0) length = average;
                        else start = average + 1;
                    }
                    while (start != length);
                }
                return start;
            }
            return 0;
        }
        #endregion

        /// <summary>
        /// 随机排序
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">排序数组</param>
        /// <param name="count">排序数量</param>
        internal static void Random<valueType>(valueType[] values, int count)
        {
            AutoCSer.Random random = AutoCSer.Random.Default;
            valueType value;
            int index;
            while (count > 1)
            {
                index = (int)((uint)random.Next() % (uint)count);
                value = values[--count];
                values[count] = values[index];
                values[index] = value;
            }
        }
        /// <summary>
        /// 随机排序
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">排序数组</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Random<valueType>(this valueType[] values)
        {
            if (values.length() > 1) Random(values, values.Length);
        }
    }
}
