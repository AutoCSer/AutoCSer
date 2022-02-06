using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;double;float;char;DateTime*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 可枚举相关扩展
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max(this System.Collections.Generic.IEnumerable<ulong> values, out ulong value)
        {
            if (values != null)
            {
                bool isValue = false;
                value = ulong.MinValue;
                foreach (ulong nextValue in values)
                {
                    if (nextValue > value) value = nextValue;
                    isValue = true;
                }
                if (isValue) return true;
            }
            value = ulong.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong max(this System.Collections.Generic.IEnumerable<ulong> values, ulong nullValue)
        {
            ulong value;
            return max(values, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool max<valueType>(this System.Collections.Generic.IEnumerable<valueType> values, Func<valueType, ulong> getKey, out valueType value)
        {
            value = default(valueType);
            if (values != null)
            {
                int count = -1;
                ulong key = ulong.MinValue;
                foreach (valueType nextValue in values)
                {
                    if (++count == 0) key = getKey(value = nextValue);
                    else
                    {
                        ulong nextKey = getKey(nextValue);
                        if (nextKey > key)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
                    }
                }
                if (count != -1) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType max<valueType>(this System.Collections.Generic.IEnumerable<valueType> values, Func<valueType, ulong> getKey, valueType nullValue)
        {
            valueType value;
            return max(values, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">最大值</param>
        /// <returns>是否存在最大值</returns>
        public static bool maxKey<valueType>(this System.Collections.Generic.IEnumerable<valueType> values, Func<valueType, ulong> getKey, out ulong value)
        {
            if (values != null)
            {
                int count = -1;
                value = ulong.MinValue;
                foreach (valueType nextValue in values)
                {
                    if (++count == 0) value = getKey(nextValue);
                    else
                    {
                        ulong nextKey = getKey(nextValue);
                        if (nextKey > value) value = nextKey;
                    }
                }
                if (count != -1) return true;
            }
            value = ulong.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最大值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最大值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong maxKey<valueType>(this System.Collections.Generic.IEnumerable<valueType> values, Func<valueType, ulong> getKey, ulong nullValue)
        {
            ulong value;
            return maxKey(values, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min(this System.Collections.Generic.IEnumerable<ulong> values, out ulong value)
        {
            if (values != null)
            {
                bool isValue = false;
                value = ulong.MinValue;
                foreach (ulong nextValue in values)
                {
                    if (nextValue < value) value = nextValue;
                    isValue = true;
                }
                if (isValue) return true;
            }
            value = ulong.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <param name="values">值集合</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong min(this System.Collections.Generic.IEnumerable<ulong> values, ulong nullValue)
        {
            ulong value;
            return min(values, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool min<valueType>(this System.Collections.Generic.IEnumerable<valueType> values, Func<valueType, ulong> getKey, out valueType value)
        {
            value = default(valueType);
            if (values != null)
            {
                int count = -1;
                ulong key = ulong.MinValue;
                foreach (valueType nextValue in values)
                {
                    if (++count == 0) key = getKey(value = nextValue);
                    else
                    {
                        ulong nextKey = getKey(nextValue);
                        if (nextKey < key)
                        {
                            value = nextValue;
                            key = nextKey;
                        }
                    }
                }
                if (count != -1) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType min<valueType>(this System.Collections.Generic.IEnumerable<valueType> values, Func<valueType, ulong> getKey, valueType nullValue)
        {
            valueType value;
            return min(values, getKey, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="value">最小值</param>
        /// <returns>是否存在最小值</returns>
        public static bool minKey<valueType>(this System.Collections.Generic.IEnumerable<valueType> values, Func<valueType, ulong> getKey, out ulong value)
        {
            if (values != null)
            {
                int count = -1;
                value = ulong.MinValue;
                foreach (valueType nextValue in values)
                {
                    if (++count == 0) value = getKey(nextValue);
                    else
                    {
                        ulong nextKey = getKey(nextValue);
                        if (nextKey < value) value = nextKey;
                    }
                }
                if (count != -1) return true;
            }
            value = ulong.MinValue;
            return false;
        }
        /// <summary>
        /// 获取最小值记录
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">获取排序键的委托</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>最小值,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ulong minKey<valueType>(this System.Collections.Generic.IEnumerable<valueType> values, Func<valueType, ulong> getKey, ulong nullValue)
        {
            ulong value;
            return minKey(values, getKey, out value) ? value : nullValue;
        }
    }
}
