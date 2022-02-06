using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;double;float;char;DateTime*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class LeftArrayExtensionSort
    {
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] getReverse(this LeftArray<ulong> array)
        {
            if (array.Length == 0) return EmptyArray<ulong>.Array;
            return FixedArray.GetReverse(array.Array, 0, array.Length);
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<ulong> reverse(this LeftArray<ulong> array)
        {
            if (array.Length > 1) FixedArray.Reverse(array.Array, 0, array.Length);
            return array;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf(this LeftArray<ulong> array, ulong value)
        {
            if (array.Length != 0)
            {
                fixed (ulong* valueFixed = array.GetFixedBuffer())
                {
                    ulong* index = FixedArray.IndexOf(valueFixed, array.Length, value);
                    if (index != null) return (int)(index - valueFixed);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf(this LeftArray<ulong> array, Func<ulong, bool> isValue)
        {
            if (array.Length != 0)
            {
                fixed (ulong* valueFixed = array.GetFixedBuffer())
                {
                    ulong* index = FixedArray.IndexOf(valueFixed, array.Length, isValue);
                    if (index != null) return (int)(index - valueFixed);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="index">起始位置</param>
        /// <returns>第一个匹配值,失败为default(ulong)</returns>
        public static ulong firstOrDefault(this LeftArray<ulong> array, Func<ulong, bool> isValue, int index)
        {
            if ((uint)index < (uint)array.Length)
            {
                fixed (ulong* valueFixed = array.GetFixedBuffer())
                {
                    ulong* valueIndex = FixedArray.IndexOf(valueFixed + index, array.Length - index, isValue);
                    if (valueIndex != null) return *valueIndex;
                }
            }
            return default(ulong);
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public static int count(this LeftArray<ulong> array, Func<ulong, bool> isValue)
        {
            if (array.Length == 0) return 0;
            int value = 0;
            fixed (ulong* valueFixed = array.GetFixedBuffer())
            {
                ulong* start = valueFixed, end = valueFixed + array.Length;
                do
                {
                    if (isValue(*start)) ++value;
                }
                while (++start != end);
            }
            return value;
        }
        /// <summary>
        /// 替换数据
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="value">新值</param>
        /// <param name="isValue">数据匹配器</param>
        public static LeftArray<ulong> replaceFirst(this LeftArray<ulong> array, ulong value, Func<ulong, bool> isValue)
        {
            if (array.Length != 0)
            {
                fixed (ulong* valueFixed = array.GetFixedBuffer())
                {
                    ulong* valueIndex = FixedArray.IndexOf(valueFixed, array.Length, isValue);
                    if (valueIndex != null) *valueIndex = value;
                }
            }
            return array;
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        public static LeftArray<ulong> find(this LeftArray<ulong> array, Func<ulong, bool> isValue)
        {
            if (array.Length == 0) return array;
            fixed (ulong* valueFixed = array.GetFixedBuffer())
            {
                ulong* write = valueFixed, start = valueFixed, end = valueFixed + array.Length;
                do
                {
                    if (isValue(*start)) *write++ = *start;
                }
                while (++start != end);
                return new LeftArray<ulong> { Array = array.Array, Length = (int)(write - valueFixed) };
            }
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<ulong> getFind(this LeftArray<ulong> array, Func<ulong, bool> isValue)
        {
            return array.Length == 0 ? new LeftArray<ulong>(0) : FixedArray.GetFind(array.Array, 0, array.Length, isValue);
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public static ulong[] getFindArray(this LeftArray<ulong> array, Func<ulong, bool> isValue)
        {
            if (array.Length == 0) return EmptyArray<ulong>.Array;
            int length = ((array.Length + 63) >> 6) << 3;
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(length);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(length);
            try
            {
                AutoCSer.Memory.Common.Clear(data.ULong, length >> 3);
                return FixedArray.GetFindArray(array.Array, 0, array.Length, isValue, new MemoryMap(data.Data));
            }
            finally { pool.Push(ref data); }
        }
        /// <summary>
        /// 数组类型转换
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="subArray">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public static ulong[] getArray<valueType>(this LeftArray<valueType> subArray, Func<valueType, ulong> getValue)
        {
            if (subArray.Length == 0) return EmptyArray<ulong>.Array;
            valueType[] array = subArray.Array;
            ulong[] newValues = new ulong[subArray.Length];
            fixed (ulong* newValueFixed = newValues)
            {
                ulong* write = newValueFixed;
                int index = 0;
                do
                {
                    *write++ = getValue(array[index++]);
                }
                while (index != subArray.Length);
            }
            return newValues;
        }
        /// <summary>
        /// 数组类型转换
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public static valueType[] getArray<valueType>(this LeftArray<ulong> array, Func<ulong, valueType> getValue)
        {
            if (array.Length == 0) return EmptyArray<valueType>.Array;
            valueType[] newValues = new valueType[array.Length];
            fixed (ulong* arrayFixed = array.GetFixedBuffer())
            {
                ulong* start = arrayFixed, end = arrayFixed + array.Length;
                int index = 0;
                do
                {
                    newValues[index++] = getValue(*start);
                }
                while (++start != end);
            }
            return newValues;
        }
        /// <summary>
        /// 遍历foreach
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="method">调用函数</param>
        /// <returns>数据数组</returns>
        public static LeftArray<ulong> each(this LeftArray<ulong> array, Action<ulong> method)
        {
            if (array.Length != 0)
            {
                fixed (ulong* valueFixed = array.GetFixedBuffer())
                {
                    for (ulong* start = valueFixed, end = valueFixed + array.Length; start != end; method(*start++)) ;
                }
            }
            return array;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool Max(this LeftArray<ulong> array, out ulong value)
        {
            if (array.Length == 0)
            {
                value = ulong.MinValue;
                return false;
            }
            fixed (ulong* valueFixed = array.GetFixedBuffer())
            {
                ulong* start = valueFixed, end = valueFixed + array.Length;
                for (value = *start; ++start != end; )
                {
                    if (*start > value) value = *start;
                }
            }
            return true;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong Max(this LeftArray<ulong> array, ulong nullValue)
        {
            ulong value;
            return Max(array, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="subArray">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool MaxKey<valueType>(this LeftArray<valueType> subArray, Func<valueType, ulong> getKey, out ulong value)
        {
            if (subArray.Length == 0)
            {
                value = ulong.MinValue;
                return false;
            }
            valueType[] array = subArray.Array;
            int index = subArray.Length - 1;
            value = getKey(array[index]);
            while (index != 0)
            {
                ulong nextKey = getKey(array[--index]);
                if (nextKey > value) value = nextKey;
            }
            return true;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong MaxKey<valueType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey, ulong nullValue)
        {
            ulong value;
            return MaxKey(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="subArray">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool Max<valueType>(this LeftArray<valueType> subArray, Func<valueType, ulong> getKey, out valueType value)
        {
            if (subArray.Length == 0)
            {
                value = default(valueType);
                return false;
            }
            valueType[] array = subArray.Array;
            ulong maxKey = getKey(value = array[0]);
            for (int index = 1; index != subArray.Length; ++index)
            {
                valueType nextValue = array[index];
                ulong nextKey = getKey(nextValue);
                if (nextKey > maxKey)
                {
                    maxKey = nextKey;
                    value = nextValue;
                }
            }
            return true;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Max<valueType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey, valueType nullValue)
        {
            valueType value;
            return Max(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool Min(this LeftArray<ulong> array, out ulong value)
        {
            if (array.Length == 0)
            {
                value = ulong.MinValue;
                return false;
            }
            fixed (ulong* valueFixed = array.GetFixedBuffer())
            {
                ulong* start = valueFixed, end = valueFixed + array.Length;
                for (value = *start; ++start != end; )
                {
                    if (*start < value) value = *start;
                }
            }
            return true;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong Min(this LeftArray<ulong> array, ulong nullValue)
        {
            ulong value;
            return Min(array, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="subArray">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool MinKey<valueType>(this LeftArray<valueType> subArray, Func<valueType, ulong> getKey, out ulong value)
        {
            if (subArray.Length == 0)
            {
                value = ulong.MinValue;
                return false;
            }
            valueType[] array = subArray.Array;
            int index = subArray.Length - 1;
            value = getKey(array[index]);
            while (index != 0)
            {
                ulong nextKey = getKey(array[--index]);
                if (nextKey < value) value = nextKey;
            }
            return true;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong MinKey<valueType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey, ulong nullValue)
        {
            ulong value;
            return MinKey(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="subArray">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool Min<valueType>(this LeftArray<valueType> subArray, Func<valueType, ulong> getKey, out valueType value)
        {
            if (subArray.Length == 0)
            {
                value = default(valueType);
                return false;
            }
            valueType[] array = subArray.Array;
            ulong minKey = getKey(value = array[0]);
            for (int index = 1; index != subArray.Length; ++index)
            {
                valueType nextValue = array[index];
                ulong nextKey = getKey(nextValue);
                if (nextKey < minKey)
                {
                    minKey = nextKey;
                    value = nextValue;
                }
            }
            return true;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Min<valueType>(this LeftArray<valueType> array, Func<valueType, ulong> getKey, valueType nullValue)
        {
            valueType value;
            return Min(array, getKey, out value) ? value : nullValue;
        }
    }
}