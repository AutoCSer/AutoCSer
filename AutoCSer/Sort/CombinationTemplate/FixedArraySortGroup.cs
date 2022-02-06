using System;
/*ulong;long;uint;int;DateTime*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static unsafe partial class FixedArraySortGroup
    {
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<ulong> distinct(this ulong[] array)
        {
            if (array == null) return new LeftArray<ulong>(0);
            if (array.Length <= 1) return new LeftArray<ulong> { Array = array, Length = array.Length };
            return new LeftArray<ulong> { Array = array, Length = Distinct(array, 0, array.Length) };
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <returns>结束位置</returns>
        internal static int Distinct(ulong[] array, int startIndex, int count)
        {
            array.sort(startIndex, count);
            fixed (ulong* valueFixed = array)
            {
                ulong* write = valueFixed + startIndex, start = write + 1, end = write + count;
                do
                {
                    if (*start != *write) *++write = *start;
                }
                while (++start != end);
                return (int)(write - valueFixed) + 1;
            }
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] distinct<valueType>(this ulong[] array, Func<ulong, valueType> getValue)
        {
            return array.isEmpty() ? EmptyArray<valueType>.Array : Distinct(array, getValue, 0, array.Length);
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <returns>目标数据集合</returns>
        internal static valueType[] Distinct<valueType>(ulong[] array, Func<ulong, valueType> getValue, int startIndex, int count)
        {
            array.sort(startIndex, count);
            fixed (ulong* valueFixed = array)
            {
                ulong* start = valueFixed + startIndex, end = start + count;
                ulong value = *start;
                int valueCount = 1;
                while (++start != end)
                {
                    if (*start != value)
                    {
                        ++valueCount;
                        value = *start;
                    }
                }
                valueType[] values = new valueType[valueCount];
                values[0] = getValue(value = *(start = valueFixed + startIndex));
                valueCount = 1;
                while (++start != end)
                {
                    if (*start != value) values[valueCount++] = getValue(value = *start);
                }
                return values;
            }
        }
        /// <summary>
        /// 数据去重
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<ulong> distinct<valueType>(this valueType[] array, Func<valueType, ulong> getValue)
        {
            ulong[] newValues = array.getArray(getValue);
            newValues.sort(0, newValues.Length);
            return newValues.distinct();
        }
        /// <summary>
        /// 数据排序分组数量
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <returns>分组数量</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static KeyValue<ulong, int>[] sortGroupCount(this ulong[] array)
        {
            return array.isEmpty() ? EmptyArray<KeyValue<ulong, int>>.Array : SortGroupCount(array, 0, array.Length);
        }
        /// <summary>
        /// 数据排序分组数量
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <returns>分组数量</returns>
        internal static KeyValue<ulong, int>[] SortGroupCount(ulong[] array, int startIndex, int count)
        {
            array.sort(startIndex, count);
            fixed (ulong* valueFixed = array)
            {
                ulong* start = valueFixed + startIndex, lastStart = start, end = start + count;
                ulong value = *start;
                int valueCount = 1;
                while (++start != end)
                {
                    if (*start != value)
                    {
                        ++valueCount;
                        value = *start;
                    }
                }
                KeyValue<ulong, int>[] values = new KeyValue<ulong, int>[valueCount];
                value = *(start = lastStart);
                valueCount = 0;
                while (++start != end)
                {
                    if (*start != value)
                    {
                        values[valueCount++].Set(value, (int)(start - lastStart));
                        value = *start;
                        lastStart = start;
                    }
                }
                values[valueCount].Set(value, (int)(start - lastStart));
                return values;
            }
        }
        /// <summary>
        /// 数据排序分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数据集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<SubArray<valueType>> sortGroup<valueType>(this valueType[] array, Func<valueType, ulong> getValue)
        {
            return array.isEmpty() ? new LeftArray<SubArray<valueType>>(0) : SortGroup(array, getValue, 0, array.Length);
        }
        /// <summary>
        /// 数据排序分组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <returns>目标数据集合</returns>
        internal static LeftArray<SubArray<valueType>> SortGroup<valueType>(this valueType[] array, Func<valueType, ulong> getValue, int startIndex, int count)
        {
            valueType[] sortArray = array.getSort(getValue, startIndex, count);
            SubArray<valueType>[] values = new SubArray<valueType>[sortArray.Length];
            ulong key = getValue(sortArray[0]);
            int valueStartIndex = 0, valueIndex = 0;
            for (int index = 1; index != sortArray.Length; ++index)
            {
                ulong nextKey = getValue(sortArray[index]);
                if (key != nextKey)
                {
                    values[valueIndex++].Set(sortArray, valueStartIndex, index - valueStartIndex);
                    key = nextKey;
                    valueStartIndex = index;
                }
            }
            values[valueIndex++].Set(sortArray, valueStartIndex, sortArray.Length - valueStartIndex);
            return new LeftArray<SubArray<valueType>> { Array = values, Length = valueIndex };
        }
        /// <summary>
        /// 数据排序分组数量
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>数据排序分组数量</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int sortGroupCount<valueType>(this valueType[] array, Func<valueType, ulong> getValue)
        {
            return array.isEmpty() ? 0 : SortGroupCount(array, getValue, 0, array.Length);
        }
        /// <summary>
        /// 数据排序分组数量
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数据数量</param>
        /// <returns>数据排序分组数量</returns>
        internal static int SortGroupCount<valueType>(this valueType[] array, Func<valueType, ulong> getValue, int startIndex, int count)
        {
            valueType[] sortArray = array.getSort(getValue, startIndex, count);
            ulong key = getValue(sortArray[0]);
            int valueCount = 0;
            for (int index = 1; index != sortArray.Length; ++index)
            {
                ulong nextKey = getValue(sortArray[index]);
                if (key != nextKey)
                {
                    ++valueCount;
                    key = nextKey;
                }
            }
            return valueCount + 1;
        }
    }
}