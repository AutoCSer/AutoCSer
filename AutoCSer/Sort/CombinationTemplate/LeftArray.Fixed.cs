using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float;char;DateTime*/

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组子串扩展
    /// </summary>
    public static unsafe partial class LeftArray
    {
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static /*Type[0]*/ulong/*Type[0]*/[] getReverse(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array)
        {
            if (array.Length == 0) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            return FixedArray.GetReverse(array.Array, 0, array.Length);
        }
        /// <summary>
        /// 逆转数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <returns>翻转后的新数组</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> reverse(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array)
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
        public static int indexOf(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.Length != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* index = FixedArray.IndexOf(valueFixed, array.Length, value);
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
        public static int indexOf(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array.Length != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* index = FixedArray.IndexOf(valueFixed, array.Length, isValue);
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
        /// <returns>第一个匹配值,失败为default(/*Type[0]*/ulong/*Type[0]*/)</returns>
        public static /*Type[0]*/ulong/*Type[0]*/ firstOrDefault(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue, int index)
        {
            if ((uint)index < (uint)array.Length)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = FixedArray.IndexOf(valueFixed + index, array.Length - index, isValue);
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
        public static int count(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array.Length == 0) return 0;
            int value = 0;
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* start = valueFixed, end = valueFixed + array.Length;
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
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> replaceFirst(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, /*Type[0]*/ulong/*Type[0]*/ value, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array.Length != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
                {
                    /*Type[0]*/
                    ulong/*Type[0]*/* valueIndex = FixedArray.IndexOf(valueFixed, array.Length, isValue);
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
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> find(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array.Length == 0) return array;
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* write = valueFixed, start = valueFixed, end = valueFixed + array.Length;
                do
                {
                    if (isValue(*start)) *write++ = *start;
                }
                while (++start != end);
                return new LeftArray</*Type[0]*/ulong/*Type[0]*/> { Array = array.Array, Length = (int)(write - valueFixed) };
            }
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> getFind(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            return array.Length == 0 ? default(LeftArray</*Type[0]*/ulong/*Type[0]*/>) : FixedArray.GetFind(array.Array, 0, array.Length, isValue);
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getFindArray(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, Func</*Type[0]*/ulong/*Type[0]*/, bool> isValue)
        {
            if (array.Length == 0) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            int length = ((array.Length + 63) >> 6) << 3;
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(length);
            Pointer.Size data = pool.GetSize64(length);
            try
            {
                Memory.ClearUnsafe(data.ULong, length >> 3);
                return FixedArray.GetFindArray(array.Array, 0, array.Length, isValue, new MemoryMap(data.Data));
            }
            finally { pool.PushOnly(ref data); }
        }
        /// <summary>
        /// 数组类型转换
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="subArray">数据数组</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public static /*Type[0]*/ulong/*Type[0]*/[] getArray<valueType>(this LeftArray<valueType> subArray, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getValue)
        {
            if (subArray.Length == 0) return NullValue</*Type[0]*/ulong/*Type[0]*/>.Array;
            valueType[] array = subArray.Array;
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[subArray.Length];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* write = newValueFixed;
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
        public static valueType[] getArray<valueType>(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, Func</*Type[0]*/ulong/*Type[0]*/, valueType> getValue)
        {
            if (array.Length == 0) return NullValue<valueType>.Array;
            valueType[] newValues = new valueType[array.Length];
            fixed (/*Type[0]*/ulong/*Type[0]*/* arrayFixed = array.Array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* start = arrayFixed, end = arrayFixed + array.Length;
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
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> each(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, Action</*Type[0]*/ulong/*Type[0]*/> method)
        {
            if (array.Length != 0)
            {
                fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
                {
                    for (/*Type[0]*/ulong/*Type[0]*/* start = valueFixed, end = valueFixed + array.Length; start != end; method(*start++)) ;
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
        public static bool Max(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.Length == 0)
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                return false;
            }
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* start = valueFixed, end = valueFixed + array.Length;
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
        public static /*Type[0]*/ulong/*Type[0]*/ Max(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
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
        public static bool MaxKey<valueType>(this LeftArray<valueType> subArray, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (subArray.Length == 0)
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                return false;
            }
            valueType[] array = subArray.Array;
            int index = subArray.Length - 1;
            value = getKey(array[index]);
            while (index != 0)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ nextKey = getKey(array[--index]);
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
        public static /*Type[0]*/ulong/*Type[0]*/ MaxKey<valueType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
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
        public static bool Max<valueType>(this LeftArray<valueType> subArray, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            if (subArray.Length == 0)
            {
                value = default(valueType);
                return false;
            }
            valueType[] array = subArray.Array;
            /*Type[0]*/
            ulong/*Type[0]*/ maxKey = getKey(value = array[0]);
            for (int index = 1; index != subArray.Length; ++index)
            {
                valueType nextValue = array[index];
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
        public static valueType Max<valueType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
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
        public static bool Min(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (array.Length == 0)
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                return false;
            }
            fixed (/*Type[0]*/ulong/*Type[0]*/* valueFixed = array.Array)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* start = valueFixed, end = valueFixed + array.Length;
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
        public static /*Type[0]*/ulong/*Type[0]*/ Min(this LeftArray</*Type[0]*/ulong/*Type[0]*/> array, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
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
        public static bool MinKey<valueType>(this LeftArray<valueType> subArray, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out /*Type[0]*/ulong/*Type[0]*/ value)
        {
            if (subArray.Length == 0)
            {
                value = /*Type[0]*/ulong/*Type[0]*/.MinValue;
                return false;
            }
            valueType[] array = subArray.Array;
            int index = subArray.Length - 1;
            value = getKey(array[index]);
            while (index != 0)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ nextKey = getKey(array[--index]);
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
        public static /*Type[0]*/ulong/*Type[0]*/ MinKey<valueType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, /*Type[0]*/ulong/*Type[0]*/ nullValue)
        {
            /*Type[0]*/
            ulong/*Type[0]*/ value;
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
        public static bool Min<valueType>(this LeftArray<valueType> subArray, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, out valueType value)
        {
            if (subArray.Length == 0)
            {
                value = default(valueType);
                return false;
            }
            valueType[] array = subArray.Array;
            /*Type[0]*/
            ulong/*Type[0]*/ minKey = getKey(value = array[0]);
            for (int index = 1; index != subArray.Length; ++index)
            {
                valueType nextValue = array[index];
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
        public static valueType Min<valueType>(this LeftArray<valueType> array, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, valueType nullValue)
        {
            valueType value;
            return Min(array, getKey, out value) ? value : nullValue;
        }
    }
}