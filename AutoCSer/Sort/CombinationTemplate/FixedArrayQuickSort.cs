using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;double,Double;float,Float;DateTime,DateTime
Desc,CompareTo;,CompareFrom*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 快速排序
    /// </summary>
    internal static unsafe partial class FixedArrayQuickSort
    {
        /// <summary>
        /// 快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        private static void sortDesc(ulong* startIndex, ulong* endIndex)
        {
            do
            {
                ulong leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.CompareTo(rightValue) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                
                ulong* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                ulong value = *averageIndex;
                if (leftValue.CompareTo(value) < 0)
                {
                    if (leftValue.CompareTo(rightValue) < 0)
                    {
                        *rightIndex = leftValue;
                        if (value.CompareTo(rightValue) < 0) *leftIndex = rightValue;
                        else
                        {
                            *leftIndex = value;
                            *averageIndex = value = rightValue;
                        }
                    }
                    else
                    {
                        *leftIndex = value;
                        *averageIndex = value = leftValue;
                    }
                }
                else
                {
                    if (value.CompareTo(rightValue) < 0)
                    {
                        *rightIndex = value;
                        if (leftValue.CompareTo(rightValue) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = value = leftValue;
                        }
                        else *averageIndex = value = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                do
                {
                    while ((*leftIndex).CompareTo(value) > 0) ++leftIndex;
                    while (value.CompareTo(*rightIndex) > 0) --rightIndex;
                    if (leftIndex < rightIndex)
                    {
                        leftValue = *leftIndex;
                        *leftIndex = *rightIndex;
                        *rightIndex = leftValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            ++leftIndex;
                            --rightIndex;
                        }
                        break;
                    }
                }
                while (++leftIndex <= --rightIndex);
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) sortDesc(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) sortDesc(leftIndex, endIndex);
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void SortDesc(ulong[] values)
        {
            if (values.Length > 1)
            {
                fixed (ulong* valueFixed = values) sortDesc(valueFixed, valueFixed + values.Length - 1);
            }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <returns>排序后的新数组</returns>
        public static ulong[] GetSortDesc(ulong[] values)
        {
            if (values.Length == 0) return values;
            ulong[] newValue = new ulong[values.Length];
            Buffer.BlockCopy(values, 0, newValue, 0, values.Length * sizeof(ulong));
            fixed (ulong* newValueFixed = newValue, valueFixed = values) sortDesc(newValueFixed, newValueFixed + values.Length - 1);
            return newValue;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void SortDesc(ulong[] values, int startIndex, int count)
        {
            fixed (ulong* valueFixed = values)
            {
                ulong* start = valueFixed + startIndex;
                sortDesc(start, start + count - 1);
            }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        public static ulong[] GetSortDesc(ulong[] values, int startIndex, int count)
        {
            if (count == 0) return EmptyArray<ulong>.Array;
            ulong[] newValues = new ulong[count];
            Buffer.BlockCopy(values, startIndex * sizeof(ulong), newValues, 0, count * sizeof(ulong));
            fixed (ulong* newValueFixed = newValues, valueFixed = values)
            {
                if (--count > 0) sortDesc(newValueFixed, newValueFixed + count);
            }
            return newValues;
        }
        /// <summary>
        /// 索引快速排序子过程
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置-1</param>
        internal static void sortDesc(ULongSortIndex* startIndex, ULongSortIndex* endIndex)
        {
            do
            {
                ULongSortIndex leftValue = *startIndex, rightValue = *endIndex;
                int average = (int)(endIndex - startIndex) >> 1;
                if (average == 0)
                {
                    if (leftValue.Value.CompareTo(rightValue.Value) < 0)
                    {
                        *startIndex = rightValue;
                        *endIndex = leftValue;
                    }
                    break;
                }
                ULongSortIndex* leftIndex = startIndex, rightIndex = endIndex, averageIndex = startIndex + average;
                ULongSortIndex indexValue = *averageIndex;
                if (leftValue.Value.CompareTo(indexValue.Value) < 0)
                {
                    if (leftValue.Value.CompareTo(rightValue.Value) < 0)
                    {
                        *rightIndex = leftValue;
                        if (indexValue.Value.CompareTo(rightValue.Value) < 0) *leftIndex = rightValue;
                        else
                        {
                            *leftIndex = indexValue;
                            *averageIndex = indexValue = rightValue;
                        }
                    }
                    else
                    {
                        *leftIndex = indexValue;
                        *averageIndex = indexValue = leftValue;
                    }
                }
                else
                {
                    if (indexValue.Value.CompareTo(rightValue.Value) < 0)
                    {
                        *rightIndex = indexValue;
                        if (leftValue.Value.CompareTo(rightValue.Value) < 0)
                        {
                            *leftIndex = rightValue;
                            *averageIndex = indexValue = leftValue;
                        }
                        else *averageIndex = indexValue = rightValue;
                    }
                }
                ++leftIndex;
                --rightIndex;
                ulong value = indexValue.Value;
                do
                {
                    while ((*leftIndex).Value.CompareTo(value) > 0) ++leftIndex;
                    while (value.CompareTo((*rightIndex).Value) > 0) --rightIndex;
                    if (leftIndex < rightIndex)
                    {
                        leftValue = *leftIndex;
                        *leftIndex = *rightIndex;
                        *rightIndex = leftValue;
                    }
                    else
                    {
                        if (leftIndex == rightIndex)
                        {
                            ++leftIndex;
                            --rightIndex;
                        }
                        break;
                    }
                }
                while (++leftIndex <= --rightIndex);
                if (rightIndex - startIndex <= endIndex - leftIndex)
                {
                    if (startIndex < rightIndex) sortDesc(startIndex, rightIndex);
                    startIndex = leftIndex;
                }
                else
                {
                    if (leftIndex < endIndex) sortDesc(leftIndex, endIndex);
                    endIndex = rightIndex;
                }
            }
            while (startIndex < endIndex);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <returns>排序后的数组</returns>
        public static valueType[] GetSortDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey)
        {
            int length = values.Length * sizeof(ULongSortIndex);
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(length);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(length);
            try
            {
                return getSortDesc(values, getKey, (ULongSortIndex*)data.Data);
            }
            finally { pool.Push(ref data); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private static valueType[] getSortDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, ULongSortIndex* fixedIndex)
        {
            ULongSortIndex.Create(fixedIndex, values, getKey);
            sortDesc(fixedIndex, fixedIndex + values.Length - 1);
            return ULongSortIndex.Create(fixedIndex, values, values.Length);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        public static valueType[] GetSortDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, int startIndex, int count)
        {
            int length = count * sizeof(ULongSortIndex);
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(length);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(length);
            try
            {
                return getSortDesc(values, getKey, startIndex, count, (ULongSortIndex*)data.Data);
            }
            finally { pool.Push(ref data); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private static valueType[] getSortDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, int startIndex, int count, ULongSortIndex* fixedIndex)
        {
            ULongSortIndex.Create(fixedIndex, values, getKey, startIndex, count);
            sortDesc(fixedIndex, fixedIndex + count - 1);
            return ULongSortIndex.Create(fixedIndex, values, count);
        }
    }
}
