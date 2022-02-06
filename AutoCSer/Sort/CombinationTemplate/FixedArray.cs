using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;double;float;char;DateTime*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public unsafe static partial class FixedArray
    {
        /// <summary>
        /// 移动数据块
        /// </summary>
        /// <param name="array">待处理数组</param>
        /// <param name="index">原始数据位置</param>
        /// <param name="writeIndex">目标数据位置</param>
        /// <param name="count">移动数据数量</param>
#if !MONO
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
        internal static void MoveNotNull(ulong[] array, int index, int writeIndex, int count)
        {
#if MONO
            int endIndex = index + count;
            if (index < writeIndex && endIndex > writeIndex)
            {
                fixed (ulong* valueFixed = array)
                {
                    for (ulong* write = valueFixed + writeIndex + count, start = valueFixed + index, end = valueFixed + endIndex;
                        end != start;
                        *--write = *--end) ;
                }
            }
            else Array.Copy(array, index, array, writeIndex, count);
#else
            fixed (ulong* valueFixed = array) AutoCSer.Win32.Kernel32.RtlMoveMemory(valueFixed + writeIndex, valueFixed + index, count * sizeof(ulong));
#endif
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">翻转数据数量</param>
        internal static void Reverse(ulong[] array, int index, int length)
        {
            fixed (ulong* valueFixed = array)
            {
                for (ulong* start = valueFixed + index, end = start + length; start < --end; ++start)
                {
                    ulong value = *start;
                    *start = *end;
                    *end = value;
                }
            }
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] reverse(this ulong[] array)
        {
            if (array == null) return EmptyArray<ulong>.Array;
            if (array.Length > 1) Reverse(array, 0, array.Length);
            return array;
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">翻转数据数量</param>
        /// <returns>翻转后的新数组</returns>
        internal static ulong[] GetReverse(ulong[] array, int index, int length)
        {
            ulong[] newValues = new ulong[length];
            fixed (ulong* valueFixed = array, newValueFixed = newValues)
            {
                for (ulong* start = valueFixed + index, end = start + length, wirte = newValueFixed + length; start != end; *--wirte = *start++) ;
            }
            return newValues;
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong[] getReverse(this ulong[] array)
        {
            if (array == null || array.Length == 0) return EmptyArray<ulong>.Array;
            return GetReverse(array, 0, array.Length);
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="valueFixed">数据指针</param>
        /// <param name="length">匹配数据数量</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为null</returns>
        internal static ulong* IndexOf(ulong* valueFixed, int length, ulong value)
        {
            for (ulong* start = valueFixed, end = valueFixed + length; start != end; ++start)
            {
                if (*start == value) return start;
            }
            return null;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf(this ulong[] array, ulong value)
        {
            if (array != null)
            {
                fixed (ulong* valueFixed = array)
                {
                    ulong* valueIndex = IndexOf(valueFixed, array.Length, value);
                    if (valueIndex != null) return (int)(valueIndex - valueFixed);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="valueFixed">数组数据</param>
        /// <param name="length">匹配数据数量</param>
        /// <param name="isValue">匹配数据委托</param>
        /// <returns>匹配位置,失败为null</returns>
        internal static ulong* IndexOf(ulong* valueFixed, int length, Func<ulong, bool> isValue)
        {
            for (ulong* start = valueFixed, end = valueFixed + length; start != end; ++start)
            {
                if (isValue(*start)) return start;
            }
            return null;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配位置,失败为-1</returns>
        public static int indexOf(this ulong[] array, Func<ulong, bool> isValue)
        {
            if (array != null)
            {
                fixed (ulong* valueFixed = array)
                {
                    ulong* valueIndex = IndexOf(valueFixed, array.Length, isValue);
                    if (valueIndex != null) return (int)(valueIndex - valueFixed);
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
        public static ulong firstOrDefault(this ulong[] array, Func<ulong, bool> isValue, int index)
        {
            if (array != null && (uint)index < (uint)array.Length)
            {
                fixed (ulong* valueFixed = array)
                {
                    ulong* valueIndex = IndexOf(valueFixed + index, array.Length - index, isValue);
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
        public static int count(this ulong[] array, Func<ulong, bool> isValue)
        {
            if (array == null) return 0;
            int value = 0;
            fixed (ulong* valueFixed = array)
            {
                for (ulong* end = valueFixed + array.Length; end != valueFixed; )
                {
                    if (isValue(*--end)) ++value;
                }
            }
            return value;
        }
        /// <summary>
        /// 替换数据
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="value">新值</param>
        /// <param name="isValue">数据匹配器</param>
        public static ulong[] replaceFirst(this ulong[] array, ulong value, Func<ulong, bool> isValue)
        {
            if (array == null) return EmptyArray<ulong>.Array;
            fixed (ulong* valueFixed = array)
            {
                ulong* valueIndex = IndexOf(valueFixed, array.Length, isValue);
                if (valueIndex != null) *valueIndex = value;
            }
            return array;
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="valueType">数组类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public static ulong[] getArray<valueType>(this valueType[] array, Func<valueType, ulong> getValue)
        {
            if (array.isEmpty()) return EmptyArray<ulong>.Array;
            ulong[] newValues = new ulong[array.Length];
            fixed (ulong* newValueFixed = newValues)
            {
                ulong* writeValue = newValueFixed;
                foreach (valueType value in array) *writeValue++ = getValue(value);
            }
            return newValues;
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">翻转数据数量</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        internal static LeftArray<ulong> GetFind(this ulong[] array, int index, int length, Func<ulong, bool> isValue)
        {
            ulong[] newValues = new ulong[length < sizeof(int) ? sizeof(int) : length];
            fixed (ulong* newValueFixed = newValues, valueFixed = array)
            {
                ulong* write = newValueFixed;
                for (ulong* start = valueFixed + index, end = valueFixed + length; start != end; ++start)
                {
                    if (isValue(*start)) *write++ = *start;
                }
                return new LeftArray<ulong> { Array = newValues, Length = (int)(write - newValueFixed) };
            }
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<ulong> getFind(this ulong[] array, Func<ulong, bool> isValue)
        {
            return array.isEmpty() ? new LeftArray<ulong>(0) : GetFind(array, 0, array.Length, isValue);
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public static ulong[] getFindArray(this ulong[] array, Func<ulong, bool> isValue)
        {
            int length = array.length();
            if (length == 0) return EmptyArray<ulong>.Array;
            {
                AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(length = ((length + 63) >> 6) << 3);
                AutoCSer.Memory.Pointer data = pool.GetMinSize(length);
                try
                {
                    AutoCSer.Memory.Common.Clear(data.ULong, length >> 3);
                    return AutoCSer.Extensions.FixedArray.GetFindArray(array, 0, array.Length, isValue, new AutoCSer.MemoryMap(data.Data));
                }
                finally { pool.Push(ref data); }
            }
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">匹配数据数量</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配数组</returns>
        internal static ulong[] GetFindArray(ulong[] array, int index, int count, Func<ulong, bool> isValue, MemoryMap map)
        {
            int length = 0, mapIndex = 0;
            fixed (ulong* valueFixed = array)
            {
                ulong* startFixed = valueFixed + index, end = startFixed + count;
                for (ulong* start = startFixed; start != end; ++start, ++mapIndex)
                {
                    if (isValue(*start))
                    {
                        ++length;
                        map.Set(mapIndex);
                    }
                }
                if (length != 0)
                {
                    ulong[] newValues = new ulong[length];
                    fixed (ulong* newValueFixed = newValues)
                    {
                        ulong* write = newValueFixed + length;
                        while (mapIndex != 0)
                        {
                            if (map.Get(--mapIndex) != 0) *--write = startFixed[mapIndex];
                        }
                    }
                    return newValues;
                }
            }
            return EmptyArray<ulong>.Array;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max(this ulong[] array, out ulong value)
        {
            if (array.isEmpty())
            {
                value = ulong.MinValue;
                return false;
            }
            fixed (ulong* valueFixed = array)
            {
                value = *valueFixed;
                for (ulong* start = valueFixed + 1, end = valueFixed + array.Length; start != end; ++start)
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
        public static ulong max(this ulong[] array, ulong nullValue)
        {
            ulong value;
            return max(array, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool maxKey<valueType>(this valueType[] array, Func<valueType, ulong> getKey, out ulong value)
        {
            if (array.isEmpty())
            {
                value = ulong.MinValue;
                return false;
            }
            value = getKey(array[0]);
            foreach (valueType nextValue in array)
            {
                ulong nextKey = getKey(nextValue);
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
        public static ulong maxKey<valueType>(this valueType[] array, Func<valueType, ulong> getKey, ulong nullValue)
        {
            ulong value;
            return maxKey(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>(this valueType[] array, Func<valueType, ulong> getKey, out valueType value)
        {
            if (array.isEmpty())
            {
                value = default(valueType);
                return false;
            }
            ulong maxKey = getKey(value = array[0]);
            foreach (valueType nextValue in array)
            {
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
        public static valueType max<valueType>(this valueType[] array, Func<valueType, ulong> getKey, valueType nullValue)
        {
            valueType value;
            return max(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min(this ulong[] array, out ulong value)
        {
            if (array.isEmpty())
            {
                value = ulong.MaxValue;
                return false;
            }
            fixed (ulong* valueFixed = array)
            {
                value = *valueFixed;
                for (ulong* start = valueFixed + 1, end = valueFixed + array.Length; start != end; ++start)
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
        public static ulong min(this ulong[] array, ulong nullValue)
        {
            ulong value;
            return min(array, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool minKey<valueType>(this valueType[] array, Func<valueType, ulong> getKey, out ulong value)
        {
            if (array.isEmpty())
            {
                value = ulong.MaxValue;
                return false;
            }
            value = getKey(array[0]);
            foreach (valueType nextValue in array)
            {
                ulong nextKey = getKey(nextValue);
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
        public static ulong minKey<valueType>(this valueType[] array, Func<valueType, ulong> getKey, ulong nullValue)
        {
            ulong value;
            return minKey(array, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据数组</param>
        /// <param name="getKey">数据获取器</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>(this valueType[] array, Func<valueType, ulong> getKey, out valueType value)
        {
            if (array.isEmpty())
            {
                value = default(valueType);
                return false;
            }
            value = array[0];
            ulong minKey = getKey(value);
            foreach (valueType nextValue in array)
            {
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
        public static valueType min<valueType>(this valueType[] array, Func<valueType, ulong> getKey, valueType nullValue)
        {
            valueType value;
            return min(array, getKey, out value) ? value : nullValue;
        }
    }
}