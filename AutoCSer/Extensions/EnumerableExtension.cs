using System;
using System.Collections.Generic;
using System.Collections;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 可枚举相关扩展
    /// </summary>
    public static partial class EnumerableExtension
    {
        /// <summary>
        /// 枚举数字
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<int> Range(int startIndex, int count)
        {
            while (count > 0)
            {
                yield return startIndex++;
                --count;
            }
        }
        /// <summary>
        /// 条件过滤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="isValue"></param>
        /// <returns></returns>
        public static IEnumerable<T> where<T>(this IEnumerable<T> values, Func<T, bool> isValue)
        {
            if (values != null)
            {
                foreach (T value in values)
                {
                    if (isValue(value)) yield return value;
                }
            }
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<T> cast<T>(this IEnumerable values)
        {
            if (values != null)
            {
                foreach (T value in values) yield return value;
            }
        }
        /// <summary>
        /// 连接集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="concatValues"></param>
        /// <returns></returns>
        public static IEnumerable<T> concat<T>(this IEnumerable<T> values, IEnumerable<T> concatValues)
        {
            if (values != null)
            {
                foreach (T value in values) yield return value;
            }
            if (concatValues != null)
            {
                foreach (T value in concatValues) yield return value;
            }
        }
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="VT">枚举值类型</typeparam>
        /// <typeparam name="KT">哈希键值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> getDictionary<VT, KT>
            (this IEnumerable<VT> values, Func<VT, KT> getKey)
            where KT : IEquatable<KT>
        {
            if (values != null)
            {
                Dictionary<KT, VT> dictionary = DictionaryCreator<KT>.Create<VT>();
                foreach (VT value in values) dictionary[getKey(value)] = value;
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="size">数据容器初始化大小</param>
        /// <returns>数组</returns>
        public static LeftArray<T> getLeftArray<T>(this IEnumerable<T> values, int size = 0)
        {
            if (values == null) return new LeftArray<T>(0);
            LeftArray<T> array = new LeftArray<T>(size);
            foreach (T value in values) array.Add(value);
            return array;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="VT">目标数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getValue">获取数据数组</param>
        /// <param name="size">数据容器初始化大小</param>
        /// <returns>数组</returns>
        public static LeftArray<VT> getLeftArray<T, VT>(this IEnumerable<T> values, Func<T, VT> getValue, int size = 0)
        {
            if (values == null) return new LeftArray<VT>(0);
            LeftArray<VT> array = new LeftArray<VT>(size);
            foreach (T value in values) array.Add(getValue(value));
            return array;
        }
    }
}
