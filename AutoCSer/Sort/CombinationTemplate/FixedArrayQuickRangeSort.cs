using System;
/*Type:ulong,ULongRangeSorter,ULongSortIndex,ULongRangeIndexSorter;long,LongRangeSorter,LongSortIndex,LongRangeIndexSorter;uint,UIntRangeSorter,UIntSortIndex,UIntRangeIndexSorter;int,IntRangeSorter,IntSortIndex,IntRangeIndexSorter;double,DoubleRangeSorter,DoubleSortIndex,DoubleRangeIndexSorter;float,FloatRangeSorter,FloatSortIndex,FloatRangeIndexSorter;DateTime,DateTimeRangeSorter,DateTimeSortIndex,DateTimeRangeIndexSorter*/
/*Compare:,>,<;Desc,<,>*/

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
        internal struct /*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
        {
            /// <summary>
            /// 跳过数据指针
            /// </summary>
            public /*Type[0]*/ulong/*Type[0]*/* SkipCount;
            /// <summary>
            /// 最后一条记录指针-1
            /// </summary>
            public /*Type[0]*/ulong/*Type[0]*/* GetEndIndex;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始指针</param>
            /// <param name="endIndex">结束指针-1</param>
            public void Sort(/*Type[0]*/ulong/*Type[0]*/* startIndex, /*Type[0]*/ulong/*Type[0]*/* endIndex)
            {
                do
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ leftValue = *startIndex, rightValue = *endIndex;
                    int average = (int)(endIndex - startIndex) >> 1;
                    if (average == 0)
                    {
                        if (leftValue /*Compare[1]*/>/*Compare[1]*/ rightValue)
                        {
                            *startIndex = rightValue;
                            *endIndex = leftValue;
                        }
                        break;
                    }
                    /*Type[0]*/
                    ulong/*Type[0]*/* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                    /*Type[0]*/
                    ulong/*Type[0]*/ value = *averageIndex;
                    if (leftValue /*Compare[1]*/>/*Compare[1]*/ value)
                    {
                        if (leftValue /*Compare[1]*/>/*Compare[1]*/ rightValue)
                        {
                            *rightIndex = leftValue;
                            if (value /*Compare[1]*/>/*Compare[1]*/ rightValue) *leftIndex = rightValue;
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
                        if (value /*Compare[1]*/>/*Compare[1]*/ rightValue)
                        {
                            *rightIndex = value;
                            if (leftValue /*Compare[1]*/>/*Compare[1]*/ rightValue)
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
                        while (*leftIndex /*Compare[2]*/</*Compare[2]*/ value) ++leftIndex;
                        while (value /*Compare[2]*/</*Compare[2]*/ *rightIndex) --rightIndex;
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
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> RangeSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int skipCount, int getCount)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] + getCount[" + AutoCSer.Extension.Number.toString(getCount) + "] > array.Length[" + AutoCSer.Extension.Number.toString(array.Length) + "]");
            return UnsafeRangeSort/*Compare[0]*//*Compare[0]*/(array, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        internal static SubArray</*Type[0]*/ulong/*Type[0]*/> UnsafeRangeSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int skipCount, int getCount)
        {
            if (getCount == 0) return default(SubArray</*Type[0]*/ulong/*Type[0]*/>);
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* start = valueFixed + skipCount;
                new /*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
                {
                    SkipCount = start,
                    GetEndIndex = start + getCount - 1
                }.Sort(valueFixed, valueFixed + array.Length - 1);
            }
            return new SubArray</*Type[0]*/ulong/*Type[0]*/> { Array = array, Start = skipCount, Length = getCount };
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> GetRangeSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int skipCount, int getCount)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            return UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array, skipCount, getCount);
        }
        /// <summary>
        /// 范围排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="skipCount">跳过数据数量</param>
        /// <param name="getCount">排序数据数量</param>
        /// <returns>排序后的新数据</returns>
        internal static SubArray</*Type[0]*/ulong/*Type[0]*/> UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int skipCount, int getCount)
        {
            if (getCount == 0) return default(SubArray</*Type[0]*/ulong/*Type[0]*/>);
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[array.Length];
            Buffer.BlockCopy(array, 0, newValues, 0, array.Length * sizeof(/*Type[0]*/ulong/*Type[0]*/));
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* start = newValueFixed + skipCount;
                new /*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
                {
                    SkipCount = start,
                    GetEndIndex = start + skipCount - 1
                }.Sort(newValueFixed, newValueFixed + array.Length - 1);
            }
            return new SubArray</*Type[0]*/ulong/*Type[0]*/> { Array = newValues, Start = skipCount, Length = getCount };
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
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> RangeSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int startIndex, int count, int skipCount, int getCount)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extension.Number.toString(startIndex) + "] < 0");
            if (startIndex + count > array.Length) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extension.Number.toString(startIndex) + "] + count[" + AutoCSer.Extension.Number.toString(count) + "] > array.Length[" + AutoCSer.Extension.Number.toString(array.Length) + "]");
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (skipCount + getCount > count) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] + getCount[" + AutoCSer.Extension.Number.toString(getCount) + "] > count[" + AutoCSer.Extension.Number.toString(count) + "]");
            return UnsafeRangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, skipCount, getCount);
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
        internal static SubArray</*Type[0]*/ulong/*Type[0]*/> UnsafeRangeSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int startIndex, int count, int skipCount, int getCount)
        {
            if (getCount == 0) return default(SubArray</*Type[0]*/ulong/*Type[0]*/>);
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* skip = valueFixed + (skipCount += startIndex), start = valueFixed + startIndex;
                new /*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
                {
                    SkipCount = skip,
                    GetEndIndex = skip + getCount - 1
                }.Sort(start, start + --count);
            }
            return new SubArray</*Type[0]*/ulong/*Type[0]*/> { Array = array, Start = skipCount, Length = getCount };
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
        public static SubArray</*Type[0]*/ulong/*Type[0]*/> GetRangeSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int startIndex, int count, int skipCount, int getCount)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extension.Number.toString(startIndex) + "] < 0");
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (startIndex + count > array.Length) count = array.Length - startIndex;
            if (skipCount + getCount > count) getCount = count - skipCount;
            return UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, skipCount, getCount);
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
        internal static SubArray</*Type[0]*/ulong/*Type[0]*/> UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] array, int startIndex, int count, int skipCount, int getCount)
        {
            if (getCount == 0) return default(SubArray</*Type[0]*/ulong/*Type[0]*/>);
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[count];
            Buffer.BlockCopy(array, startIndex * sizeof(/*Type[0]*/ulong/*Type[0]*/), newValues, 0, count * sizeof(/*Type[0]*/ulong/*Type[0]*/));
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* start = newValueFixed + skipCount;
                new /*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/
                {
                    SkipCount = start,
                    GetEndIndex = start + getCount - 1
                }.Sort(newValueFixed, newValueFixed + count - 1);
            }
            return new SubArray</*Type[0]*/ulong/*Type[0]*/> { Array = newValues, Start = skipCount, Length = getCount };
        }

        /// <summary>
        /// 索引范围排序器
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct /*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/
        {
            /// <summary>
            /// 跳过数据指针
            /// </summary>
            public /*Type[2]*/ULongSortIndex/*Type[2]*/* SkipCount;
            /// <summary>
            /// 最后一条记录指针-1
            /// </summary>
            public /*Type[2]*/ULongSortIndex/*Type[2]*/* GetEndIndex;
            /// <summary>
            /// 范围排序
            /// </summary>
            /// <param name="startIndex">起始指针</param>
            /// <param name="endIndex">结束指针-1</param>
            public void Sort(/*Type[2]*/ULongSortIndex/*Type[2]*/* startIndex, /*Type[2]*/ULongSortIndex/*Type[2]*/* endIndex)
            {
                do
                {
                    /*Type[2]*/
                    ULongSortIndex/*Type[2]*/ leftValue = *startIndex, rightValue = *endIndex;
                    int average = (int)(endIndex - startIndex) >> 1;
                    if (average == 0)
                    {
                        if (leftValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value)
                        {
                            *startIndex = rightValue;
                            *endIndex = leftValue;
                        }
                        break;
                    }
                    /*Type[2]*/
                    ULongSortIndex/*Type[2]*/* averageIndex = startIndex + average, leftIndex = startIndex, rightIndex = endIndex;
                    /*Type[2]*/
                    ULongSortIndex/*Type[2]*/ indexValue = *averageIndex;
                    if (leftValue.Value /*Compare[1]*/>/*Compare[1]*/ indexValue.Value)
                    {
                        if (leftValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value)
                        {
                            *rightIndex = leftValue;
                            if (indexValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value) *leftIndex = rightValue;
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
                        if (indexValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value)
                        {
                            *rightIndex = indexValue;
                            if (leftValue.Value /*Compare[1]*/>/*Compare[1]*/ rightValue.Value)
                            {
                                *leftIndex = rightValue;
                                *averageIndex = indexValue = leftValue;
                            }
                            else *averageIndex = indexValue = rightValue;
                        }
                    }
                    ++leftIndex;
                    --rightIndex;
                    /*Type[0]*/
                    ulong/*Type[0]*/ value = indexValue.Value;
                    do
                    {
                        while ((*leftIndex).Value /*Compare[2]*/</*Compare[2]*/ value) ++leftIndex;
                        while (value /*Compare[2]*/</*Compare[2]*/ (*rightIndex).Value) --rightIndex;
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
        public static valueType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            return UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array, getKey, skipCount, getCount);
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
        internal static valueType[] UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            if (getCount == 0) return NullValue<valueType>.Array;
            int size = array.Length * sizeof(/*Type[2]*/ULongSortIndex/*Type[2]*/);
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(size);
            Pointer.Size data = pool.GetSize(size);
            try
            {
                return getRangeSort/*Compare[0]*//*Compare[0]*/(array, getKey, skipCount, getCount, (/*Type[2]*/ULongSortIndex/*Type[2]*/*)data.Data);
            }
            finally { pool.PushOnly(ref data); }
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
        private static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, /*Type[2]*/ULongSortIndex/*Type[2]*/* fixedIndex)
        {
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* writeIndex = fixedIndex;
            for (int index = 0; index != array.Length; (*writeIndex++).Set(getKey(array[index]), index++)) ;
            return getRangeSort/*Compare[0]*//*Compare[0]*/(array, skipCount, getCount, fixedIndex);
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
        private static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, int skipCount, int getCount, /*Type[2]*/ULongSortIndex/*Type[2]*/* fixedIndex)
        {
            new /*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/
            {
                SkipCount = fixedIndex + skipCount,
                GetEndIndex = fixedIndex + skipCount + getCount - 1
            }.Sort(fixedIndex, fixedIndex + array.Length - 1);
            valueType[] newValues = new valueType[getCount];
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* writeIndex = fixedIndex + skipCount;
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
        public static valueType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, /*Type[2]*/ULongSortIndex/*Type[2]*/[] indexs, int skipCount, int getCount)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            if (getCount == 0) return NullValue<valueType>.Array;
            if (array.Length != indexs.Length) throw new IndexOutOfRangeException("array.Length[" + AutoCSer.Extension.Number.toString(array.Length) + "] != indexs.Length[" + AutoCSer.Extension.Number.toString(indexs.Length) + "]");
            fixed (/*Type[2]*/ULongSortIndex/*Type[2]*/* fixedIndex = indexs) return getRangeSort/*Compare[0]*//*Compare[0]*/(array, skipCount, getCount, fixedIndex);
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
        public static valueType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, int startIndex, int count, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extension.Number.toString(startIndex) + "] < 0");
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (startIndex + count > array.Length) count = array.Length - startIndex;
            if (skipCount + getCount > count) getCount = count - skipCount;
            return UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, getKey, skipCount, getCount);
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
        internal static valueType[] UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, int startIndex, int count, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount)
        {
            if (getCount == 0) return NullValue<valueType>.Array;
            int size = count * sizeof(/*Type[2]*/ULongSortIndex/*Type[2]*/);
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(size);
            Pointer.Size data = pool.GetSize(size);
            try
            {
                return getRangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, getKey, skipCount, getCount, (/*Type[2]*/ULongSortIndex/*Type[2]*/*)data.Data);
            }
            finally { pool.PushOnly(ref data); }
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
        private static valueType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] array, int startIndex, int count, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, /*Type[2]*/ULongSortIndex/*Type[2]*/* fixedIndex)
        {
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* writeIndex = fixedIndex;
            for (int index = startIndex, endIndex = startIndex + count; index != endIndex; (*writeIndex++).Set(getKey(array[index]), index++)) ;
            new /*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/
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
        public static returnType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (getValue == null) throw new ArgumentNullException();
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            return UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array, getKey, skipCount, getCount, getValue);
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
        internal static returnType[] UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (getCount == 0) return NullValue<returnType>.Array;
            int size = array.Length * sizeof(/*Type[2]*/ULongSortIndex/*Type[2]*/);
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(size);
            Pointer.Size data = pool.GetSize(size);
            try
            {
                return getRangeSort/*Compare[0]*//*Compare[0]*/(array, getKey, skipCount, getCount, getValue, (/*Type[2]*/ULongSortIndex/*Type[2]*/*)data.Data);
            }
            finally { pool.PushOnly(ref data); }
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
        private static returnType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue, /*Type[2]*/ULongSortIndex/*Type[2]*/* fixedIndex)
        {
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* writeIndex = fixedIndex;
            for (int index = 0; index != array.Length; (*writeIndex++).Set(getKey(array[index]), index++)) ;
            return getRangeSort/*Compare[0]*//*Compare[0]*/(array, skipCount, getCount, getValue, fixedIndex);
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
        private static returnType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(valueType[] array, int skipCount, int getCount, Func<valueType, returnType> getValue, /*Type[2]*/ULongSortIndex/*Type[2]*/* fixedIndex)
        {
            new /*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/
            {
                SkipCount = fixedIndex + skipCount,
                GetEndIndex = fixedIndex + skipCount + getCount - 1
            }.Sort(fixedIndex, fixedIndex + array.Length - 1);
            returnType[] newValues = new returnType[getCount];
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* writeIndex = fixedIndex + skipCount;
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
        public static returnType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(valueType[] array, /*Type[2]*/ULongSortIndex/*Type[2]*/[] indexs, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (skipCount + getCount > array.Length) getCount = array.Length - skipCount;
            if (getCount == 0) return NullValue<returnType>.Array;
            if (array.Length != indexs.Length) throw new IndexOutOfRangeException("array.Length[" + AutoCSer.Extension.Number.toString(array.Length) + "] != indexs.Length[" + AutoCSer.Extension.Number.toString(indexs.Length) + "]");
            fixed (/*Type[2]*/ULongSortIndex/*Type[2]*/* fixedIndex = indexs) return getRangeSort/*Compare[0]*//*Compare[0]*/(array, skipCount, getCount, getValue, fixedIndex);
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
        public static returnType[] GetRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(valueType[] array, int startIndex, int count, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException("startIndex[" + AutoCSer.Extension.Number.toString(startIndex) + "] < 0");
            if (skipCount < 0) throw new IndexOutOfRangeException("skipCount[" + AutoCSer.Extension.Number.toString(skipCount) + "] < 0");
            if (getValue == null) throw new ArgumentNullException();
            if (startIndex + count > array.Length) count = array.Length - startIndex;
            if (skipCount + getCount > count) getCount = count - skipCount;
            return UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, getKey, skipCount, getCount, getValue);
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
        internal static returnType[] UnsafeGetRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(valueType[] array, int startIndex, int count, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue)
        {
            if (getCount == 0) return NullValue<returnType>.Array;
            int size = count * sizeof(/*Type[2]*/ULongSortIndex/*Type[2]*/);
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(size);
            Pointer.Size data = pool.GetSize(size);
            try
            {
                return getRangeSort/*Compare[0]*//*Compare[0]*/(array, startIndex, count, getKey, skipCount, getCount, getValue, (/*Type[2]*/ULongSortIndex/*Type[2]*/*)data.Data);
            }
            finally { pool.PushOnly(ref data); }
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
        private static returnType[] getRangeSort/*Compare[0]*//*Compare[0]*/<valueType, returnType>(valueType[] array, int startIndex, int count, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int skipCount, int getCount, Func<valueType, returnType> getValue, /*Type[2]*/ULongSortIndex/*Type[2]*/* fixedIndex)
        {
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* writeIndex = fixedIndex;
            for (int index = startIndex, endIndex = startIndex + count; index != endIndex; (*writeIndex++).Set(getKey(array[index]), index++)) ;
            new /*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/
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
