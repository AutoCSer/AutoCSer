using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;double,Double;float,Float;DateTime,DateTime
Desc,CompareTo;,CompareFrom*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 快速排序
    /// </summary>
    internal static unsafe partial class FixedArrayQuickRangeSort
    {
        /// <summary>
        /// 范围排序器(一般用于获取分页)
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct ULongRangeSorterDesc
        {
            /// <summary>
            /// 跳过数据指针
            /// </summary>
            public ulong* SkipCount;
            /// <summary>
            /// 最后一条记录指针-1
            /// </summary>
            public ulong* GetEndIndex;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始指针</param>
            /// <param name="endIndex">结束指针-1</param>
            public void Sort(ulong* startIndex, ulong* endIndex)
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
                    ulong* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
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
                        if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                        if (leftIndex > GetEndIndex) break;
                        startIndex = leftIndex;
                    }
                    else
                    {
                        if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                        if (rightIndex < SkipCount) break;
                        endIndex = rightIndex;
                    }
                }
                while (startIndex < endIndex);
            }
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> RangeSortDesc(ulong[] array, int skipCount, int getCount)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] + getCount[" + AutoCSer.Extensions.NumberExtension.toString(getCount) + "] > array.Length[" + AutoCSer.Extensions.NumberExtension.toString(array.Length) + "]");
            return UnsafeRangeSortDesc(array, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        internal static SubArray<ulong> UnsafeRangeSortDesc(ulong[] array, int skipCount, int getCount)
        {
            if (getCount == 0) return new SubArray<ulong>();
            fixed (ulong* valueFixed = array)
            {
                ulong* start = valueFixed + skipCount;
                new ULongRangeSorterDesc
                {
                    SkipCount = start,
                    GetEndIndex = start + getCount - 1
                }.Sort(valueFixed, valueFixed + array.Length - 1);
            }
            return new SubArray<ulong> { Array = array, Start = skipCount, Length = getCount };
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> GetRangeSortDesc(ulong[] array, int skipCount, int getCount)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            return UnsafeGetRangeSortDesc(array, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        internal static SubArray<ulong> UnsafeGetRangeSortDesc(ulong[] array, int skipCount, int getCount)
        {
            if (getCount == 0) return new SubArray<ulong>();
            ulong[] newValues = new ulong[array.Length];
            Buffer.BlockCopy(array, 0, newValues, 0, array.Length * sizeof(ulong));
            fixed (ulong* newValueFixed = newValues)
            {
                ulong* start = newValueFixed + skipCount;
                new ULongRangeSorterDesc
                {
                    SkipCount = start,
                    GetEndIndex = start + skipCount - 1
                }.Sort(newValueFixed, newValueFixed + array.Length - 1);
            }
            return new SubArray<ulong> { Array = newValues, Start = skipCount, Length = getCount };
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> RangeSortDesc(ulong[] array, int startIndex, int count, int skipCount, int getCount)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extensions.NumberExtension.toString(startIndex) + "] < 0");
            if (startIndex + count > array.Length) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extensions.NumberExtension.toString(startIndex) + "] + count[" + AutoCSer.Extensions.NumberExtension.toString(count) + "] > array.Length[" + AutoCSer.Extensions.NumberExtension.toString(array.Length) + "]");
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (skipCount + getCount > count) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] + getCount[" + AutoCSer.Extensions.NumberExtension.toString(getCount) + "] > count[" + AutoCSer.Extensions.NumberExtension.toString(count) + "]");
            return UnsafeRangeSortDesc(array, startIndex, count, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        internal static SubArray<ulong> UnsafeRangeSortDesc(ulong[] array, int startIndex, int count, int skipCount, int getCount)
        {
            if (getCount == 0) return new SubArray<ulong>();
            fixed (ulong* valueFixed = array)
            {
                ulong* skip = valueFixed + (skipCount += startIndex), start = valueFixed + startIndex;
                new ULongRangeSorterDesc
                {
                    SkipCount = skip,
                    GetEndIndex = skip + getCount - 1
                }.Sort(start, start + --count);
            }
            return new SubArray<ulong> { Array = array, Start = skipCount, Length = getCount };
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray<ulong> GetRangeSortDesc(ulong[] array, int startIndex, int count, int skipCount, int getCount)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extensions.NumberExtension.toString(startIndex) + "] < 0");
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (startIndex + count > array.Length) count = array.Length - startIndex;
            if (skipCount + getCount > count) getCount = count - skipCount;
            return UnsafeGetRangeSortDesc(array, startIndex, count, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        internal static SubArray<ulong> UnsafeGetRangeSortDesc(ulong[] array, int startIndex, int count, int skipCount, int getCount)
        {
            if (getCount == 0) return new SubArray<ulong>();
            ulong[] newValues = new ulong[count];
            Buffer.BlockCopy(array, startIndex * sizeof(ulong), newValues, 0, count * sizeof(ulong));
            fixed (ulong* newValueFixed = newValues)
            {
                ulong* start = newValueFixed + skipCount;
                new ULongRangeSorterDesc
                {
                    SkipCount = start,
                    GetEndIndex = start + getCount - 1
                }.Sort(newValueFixed, newValueFixed + count - 1);
            }
            return new SubArray<ulong> { Array = newValues, Start = skipCount, Length = getCount };
        }

        /// <summary>
        /// 索引范围排序器
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct ULongRangeIndexSorterDesc
        {
            /// <summary>
            /// 跳过数据指针
            /// </summary>
            public ULongSortIndex* SkipCount;
            /// <summary>
            /// 最后一条记录指针-1
            /// </summary>
            public ULongSortIndex* GetEndIndex;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始指针</param>
            /// <param name="endIndex">结束指针-1</param>
            public void Sort(ULongSortIndex* startIndex, ULongSortIndex* endIndex)
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
                    ULongSortIndex* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
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
                        if (startIndex < rightIndex && rightIndex >= SkipCount) Sort(startIndex, rightIndex);
                        if (leftIndex > GetEndIndex) break;
                        startIndex = leftIndex;
                    }
                    else
                    {
                        if (leftIndex < endIndex && leftIndex <= GetEndIndex) Sort(leftIndex, endIndex);
                        if (rightIndex < SkipCount) break;
                        endIndex = rightIndex;
                    }
                }
                while (startIndex < endIndex);
            }
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetRangeSortDesc<valueType>(valueType[] array, Func<valueType, ulong> getKey, int skipCount, int getCount)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            return UnsafeGetRangeSortDesc(array, getKey, skipCount, getCount);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        internal static valueType[] UnsafeGetRangeSortDesc<valueType>(valueType[] array, Func<valueType, ulong> getKey, int skipCount, int getCount)
        {
            if (getCount == 0) return EmptyArray<valueType>.Array;
            int size = array.Length * sizeof(ULongSortIndex);
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(size);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(size);
            try
            {
                return getRangeSortDesc(array, getKey, skipCount, getCount, (ULongSortIndex*)data.Data);
            }
            finally { pool.Push(ref data); }
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static valueType[] getRangeSortDesc<valueType>(valueType[] array, Func<valueType, ulong> getKey, int skipCount, int getCount, ULongSortIndex* fixedIndex)
        {
            ULongSortIndex* writeIndex = fixedIndex;
            for (int index = 0; index != array.Length; (*writeIndex++).Set(getKey(array[index]), index++)) ;
            return getRangeSortDesc(array, skipCount, getCount, fixedIndex);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private static valueType[] getRangeSortDesc<valueType>(valueType[] array, int skipCount, int getCount, ULongSortIndex* fixedIndex)
        {
            new ULongRangeIndexSorterDesc
            {
                SkipCount = fixedIndex + skipCount,
                GetEndIndex = fixedIndex + skipCount + getCount - 1
            }.Sort(fixedIndex, fixedIndex + array.Length - 1);
            valueType[] newValues = new valueType[getCount];
            ULongSortIndex* writeIndex = fixedIndex + skipCount;
            for (int index = 0; index != newValues.Length; ++index) newValues[index] = array[(*writeIndex++).Index];
            return newValues;
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="indexs">排序索引</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        public static valueType[] GetRangeSortDesc<valueType>(valueType[] array, ULongSortIndex[] indexs, int skipCount, int getCount)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            if (getCount == 0) return EmptyArray<valueType>.Array;
            if (array.Length != indexs.Length) throw new IndexOutOfRangeException("array.Length[" + AutoCSer.Extensions.NumberExtension.toString(array.Length) + "] != indexs.Length[" + AutoCSer.Extensions.NumberExtension.toString(indexs.Length) + "]");
            fixed (ULongSortIndex* fixedIndex = indexs) return getRangeSortDesc(array, skipCount, getCount, fixedIndex);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetRangeSortDesc<valueType>(valueType[] array, int startIndex, int count, Func<valueType, ulong> getKey, int skipCount, int getCount)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extensions.NumberExtension.toString(startIndex) + "] < 0");
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (startIndex + count > array.Length) count = array.Length - startIndex;
            if (skipCount + getCount > count) getCount = count - skipCount;
            return UnsafeGetRangeSortDesc(array, startIndex, count, getKey, skipCount, getCount);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数组</returns>
        internal static valueType[] UnsafeGetRangeSortDesc<valueType>(valueType[] array, int startIndex, int count, Func<valueType, ulong> getKey, int skipCount, int getCount)
        {
            if (getCount == 0) return EmptyArray<valueType>.Array;
            int size = count * sizeof(ULongSortIndex);
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(size);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(size);
            try
            {
                return getRangeSortDesc(array, startIndex, count, getKey, skipCount, getCount, (ULongSortIndex*)data.Data);
            }
            finally { pool.Push(ref data); }
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private static valueType[] getRangeSortDesc<valueType>(valueType[] array, int startIndex, int count, Func<valueType, ulong> getKey, int skipCount, int getCount, ULongSortIndex* fixedIndex)
        {
            ULongSortIndex* writeIndex = fixedIndex;
            for (int index = startIndex, endIndex = startIndex + count; index != endIndex; (*writeIndex++).Set(getKey(array[index]), index++)) ;
            new ULongRangeIndexSorterDesc
            {
                SkipCount = fixedIndex + skipCount,
                GetEndIndex = fixedIndex + skipCount + getCount - 1
            }.Sort(fixedIndex, fixedIndex + count - 1);
            valueType[] newValues = new valueType[getCount];
            writeIndex = fixedIndex + skipCount;
            for (int index = 0; index != newValues.Length; ++index) newValues[index] = array[(*writeIndex++).Index];
            return newValues;
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static returnType[] GetRangeSortDesc<valueType, returnType>(valueType[] array, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (getValue == null) throw new ArgumentNullException();
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            return UnsafeGetRangeSortDesc(array, getKey, skipCount, getCount, getValue);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>排序后的数组</returns>
        internal static returnType[] UnsafeGetRangeSortDesc<valueType, returnType>(valueType[] array, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (getCount == 0) return EmptyArray<returnType>.Array;
            int size = array.Length * sizeof(ULongSortIndex);
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(size);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(size);
            try
            {
                return getRangeSortDesc(array, getKey, skipCount, getCount, getValue, (ULongSortIndex*)data.Data);
            }
            finally { pool.Push(ref data); }
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="getValue">获取返回数据</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static returnType[] getRangeSortDesc<valueType, returnType>(valueType[] array, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue, ULongSortIndex* fixedIndex)
        {
            ULongSortIndex* writeIndex = fixedIndex;
            for (int index = 0; index != array.Length; (*writeIndex++).Set(getKey(array[index]), index++)) ;
            return getRangeSortDesc(array, skipCount, getCount, getValue, fixedIndex);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="getValue">获取返回数据</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private static returnType[] getRangeSortDesc<valueType, returnType>(valueType[] array, int skipCount, int getCount, Func<valueType, returnType> getValue, ULongSortIndex* fixedIndex)
        {
            new ULongRangeIndexSorterDesc
            {
                SkipCount = fixedIndex + skipCount,
                GetEndIndex = fixedIndex + skipCount + getCount - 1
            }.Sort(fixedIndex, fixedIndex + array.Length - 1);
            returnType[] newValues = new returnType[getCount];
            ULongSortIndex* writeIndex = fixedIndex + skipCount;
            for (int index = 0; index != newValues.Length; ++index) newValues[index] = getValue(array[(*writeIndex++).Index]);
            return newValues;
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="indexs">排序索引</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>排序后的数组</returns>
        public static returnType[] GetRangeSortDesc<valueType, returnType>(valueType[] array, ULongSortIndex[] indexs, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            if (getCount == 0) return EmptyArray<returnType>.Array;
            if (array.Length != indexs.Length) throw new IndexOutOfRangeException("array.Length[" + AutoCSer.Extensions.NumberExtension.toString(array.Length) + "] != indexs.Length[" + AutoCSer.Extensions.NumberExtension.toString(indexs.Length) + "]");
            fixed (ULongSortIndex* fixedIndex = indexs) return getRangeSortDesc(array, skipCount, getCount, getValue, fixedIndex);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>排序后的数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static returnType[] GetRangeSortDesc<valueType, returnType>(valueType[] array, int startIndex, int count, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extensions.NumberExtension.toString(startIndex) + "] < 0");
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extensions.NumberExtension.toString(skipCount) + "] < 0");
            if (getValue == null) throw new ArgumentNullException();
            if (startIndex + count > array.Length) count = array.Length - startIndex;
            if (skipCount + getCount > count) getCount = count - skipCount;
            return UnsafeGetRangeSortDesc(array, startIndex, count, getKey, skipCount, getCount, getValue);
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="getValue">获取返回数据</param>
        /// <returns>排序后的数组</returns>
        internal static returnType[] UnsafeGetRangeSortDesc<valueType, returnType>(valueType[] array, int startIndex, int count, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (getCount == 0) return EmptyArray<returnType>.Array;
            int size = count * sizeof(ULongSortIndex);
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(size);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(size);
            try
            {
                return getRangeSortDesc(array, startIndex, count, getKey, skipCount, getCount, getValue, (ULongSortIndex*)data.Data);
            }
            finally { pool.Push(ref data); }
        }
        /// <summary>
        /// 数组范围排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="returnType">返回数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序范围数据数量</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <param name="getValue">获取返回数据</param>
        /// <param name="fixedIndex">索引位置</param>
        /// <returns>排序后的数组</returns>
        private static returnType[] getRangeSortDesc<valueType, returnType>(valueType[] array, int startIndex, int count, Func<valueType, ulong> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue, ULongSortIndex* fixedIndex)
        {
            ULongSortIndex* writeIndex = fixedIndex;
            for (int index = startIndex, endIndex = startIndex + count; index != endIndex; (*writeIndex++).Set(getKey(array[index]), index++)) ;
            new ULongRangeIndexSorterDesc
            {
                SkipCount = fixedIndex + skipCount,
                GetEndIndex = fixedIndex + skipCount + getCount - 1
            }.Sort(fixedIndex, fixedIndex + count - 1);
            returnType[] newValues = new returnType[getCount];
            writeIndex = fixedIndex + skipCount;
            for (int index = 0; index != newValues.Length; ++index) newValues[index] = getValue(array[(*writeIndex++).Index]);
            return newValues;
        }
    }
}
