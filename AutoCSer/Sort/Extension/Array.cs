using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static partial class Array_Sort
    {
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
