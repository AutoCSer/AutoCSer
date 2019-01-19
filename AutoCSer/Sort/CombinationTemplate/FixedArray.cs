using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float;char;DateTime*/

namespace AutoCSer.Extension
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
        internal static void MoveNotNull(/*Type[0]*/ulong/*Type[0]*/[] array, int index, int writeIndex, int count)
        {
#if MONO
            int endIndex = index + count;
            if (index < writeIndex && endIndex > writeIndex)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    for (/*Type[0]*/ulong/*Type[0]*/* write = valueFixed + writeIndex + count, start = valueFixed + index, end = valueFixed + endIndex;
                        end != start;
                        *--write = *--end) ;
                }
            }
            else Array.Copy(array, index, array, writeIndex, count);
#else
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array) AutoCSer.Win32.Kernel32.RtlMoveMemory(valueFixed + writeIndex, valueFixed + index, count * sizeof(/*Type[0]*/ulong/*Type[0]*/));
#endif
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="length">翻转数据数量</param>
        internal static void Reverse(/*Type[0]*/ulong/*Type[0]*/[] array, int index, int length)
        {
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + index, end = start + length; start < --end; ++start)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/ value = *start;
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
        public static /*Type[0]*/ulong/*Type[0]*/[] reverse(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            if (array == null) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
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
        internal static /*Type[0]*/ulong/*Type[0]*/[] GetReverse(/*Type[0]*/ulong/*Type[0]*/[] array, int index, int length)
        {
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[length];
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array, newValueFixed = newValues)
            {
                for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + index, end = start + length, wirte = newValueFixed + length; start != end; *--wirte = *start++) ;
            }
            return newValues;
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/ulong/*Type[0]*/[] getReverse(this /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            if (array == null || array.Length == 0) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            return GetReverse(array, 0, array.Length);
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="valueFixed">数据指针</param>
        /// <param name="length">匹配数据数量</param>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为null</returns>
        internal static /*Type[0]*/ulong/*Type[0]*/* IndexOf(/*Type[0]*/ulong/*Type[0]*/* valueFixed, int length, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed, end = valueFixed + length; start != end; ++start)
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
        public static int indexOf(this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array != null)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed, array.Length, value);
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
        internal static /*Type[0]*/ulong/*Type[0]*/* IndexOf(/*Type[0]*/ulong/*Type[0]*/* valueFixed, int length, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed, end = valueFixed + length; start != end; ++start)
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
        public static int indexOf(this /*Type[0]*/ulong/*Type[0]*/[] array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array != null)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed, array.Length, isValue);
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
        /// <returns>第一个匹配值,失败为default(/*Type[0]*/ulong/*Type[0]*/)</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ firstOrDefault(this /*Type[0]*/ulong/*Type[0]*/[] array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue, int index)
        {
            if (array != null && (uint)index < (uint)array.Length)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed + index, array.Length - index, isValue);
                    if (valueIndex != null) return *valueIndex;
                }
            }
            return default(/*Type[0]*/ulong/*Type[0]*/);
        }
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数量</returns>
        public static int count(this /*Type[0]*/ulong/*Type[0]*/[] array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array == null) return 0;
            int value = 0;
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                for (/*Type[0]*/ulong/*Type[0]*/* end = valueFixed + array.Length; end != valueFixed; )
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
        public static /*Type[0]*/ulong/*Type[0]*/[] replaceFirst(this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ value, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array == null) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* valueIndex = IndexOf(valueFixed, array.Length, isValue);
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
        public static /*Type[0]*/ulong/*Type[0]*/[] getArray<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (array.isEmpty()) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[array.Length];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* writeValue = newValueFixed;
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
        internal static LeftArray</*Type[0]*/ulong/*Type[0]*/> GetFind(this /*Type[0]*/ulong/*Type[0]*/[] array, int index, int length, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[length < sizeof(int) ? sizeof(int) : length];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues, valueFixed = array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* write = newValueFixed;
                for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + index, end = valueFixed + length; start != end; ++start)
                {
                    if (isValue(*start)) *write++ = *start;
                }
                return new LeftArray</*Type[0]*/ulong/*Type[0]*/> { Array = newValues, Length = (int)(write - newValueFixed) };
            }
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> getFind(this /*Type[0]*/ulong/*Type[0]*/[] array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return array.isEmpty() ? default(LeftArray</*Type[0]*/ulong/*Type[0]*/>) : GetFind(array, 0, array.Length, isValue);
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getFindArray(this /*Type[0]*/ulong/*Type[0]*/[] array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            int length = array.length();
            if (length == 0) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            {
                UnmanagedPool pool = UnmanagedPool.GetDefaultPool(length = ((length + 63) >> 6) << 3);
                Pointer.Size data = pool.GetSize64(length);
                try
                {
                    Memory.ClearUnsafe(data.ULong, length >> 3);
                    return AutoCSer.Extension.FixedArray.GetFindArray(array, 0, array.Length, isValue, new AutoCSer.MemoryMap(data.Data));
                }
                finally { pool.PushOnly(ref data); }
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
        internal static /*Type[0]*/ulong/*Type[0]*/[] GetFindArray(/*Type[0]*/ulong/*Type[0]*/[] array, int index, int count, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue, MemoryMap map)
        {
            int length = 0, mapIndex = 0;
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* startFixed = valueFixed + index, end = startFixed + count;
                for (/*Type[0]*/ulong/*Type[0]*/* start = startFixed; start != end; ++start, ++mapIndex)
                {
                    if (isValue(*start))
                    {
                        ++length;
                        map.Set(mapIndex);
                    }
                }
                if (length != 0)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[length];
                    fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
                    {
                        /*Type[0]*/
                        ulong/*Type[0]*/* write = newValueFixed + length;
                        while (mapIndex != 0)
                        {
                            if (map.Get(--mapIndex) != 0) *--write = startFixed[mapIndex];
                        }
                    }
                    return newValues;
                }
            }
            return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max(this /*Type[0]*/ulong/*Type[0]*/[] array, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.isEmpty())
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                return false;
            }
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                value = *valueFixed;
                for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + 1, end = valueFixed + array.Length; start != end; ++start)
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
        public static /*Type[0]*/ulong/*Type[0]*/ max(this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
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
        public static bool maxKey<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.isEmpty())
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                return false;
            }
            value = getKey(array[0]);
            foreach (valueType nextValue in array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ nextKey = getKey(nextValue);
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
        public static /*Type[0]*/ulong/*Type[0]*/ maxKey<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
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
        public static bool max<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            if (array.isEmpty())
            {
                value = default(valueType);
                return false;
            }
            /*Type[0]*/
            ulong/*Type[0]*/ maxKey = getKey(value = array[0]);
            foreach (valueType nextValue in array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ nextKey = getKey(nextValue);
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
        public static valueType max<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
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
        public static bool min(this /*Type[0]*/ulong/*Type[0]*/[] array, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.isEmpty())
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
                return false;
            }
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array)
            {
                value = *valueFixed;
                for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed + 1, end = valueFixed + array.Length; start != end; ++start)
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
        public static /*Type[0]*/ulong/*Type[0]*/ min(this /*Type[0]*/ulong/*Type[0]*/[] array, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
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
        public static bool minKey<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.isEmpty())
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MaxValue;
                return false;
            }
            value = getKey(array[0]);
            foreach (valueType nextValue in array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ nextKey = getKey(nextValue);
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
        public static /*Type[0]*/ulong/*Type[0]*/ minKey<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
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
        public static bool min<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            if (array.isEmpty())
            {
                value = default(valueType);
                return false;
            }
            value = array[0];
            /*Type[0]*/
            ulong/*Type[0]*/ minKey = getKey(value);
            foreach (valueType nextValue in array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ nextKey = getKey(nextValue);
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
        public static valueType min<valueType>(this valueType[] array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
        {
            valueType value;
            return min(array, getKey, out value) ? value : nullValue;
        }
    }
}