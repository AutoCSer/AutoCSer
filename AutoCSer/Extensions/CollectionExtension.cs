using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 集合相关扩展
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// ICollection泛型转换
        /// </summary>
        /// <param name="value">数据集合</param>
        /// <returns>泛型数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ICollection<T> toGeneric<T>(this ICollection value)
        {
            return new ToGenericCollection<T>(value);
        }

        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">数据集合</param>
        /// <returns>null为0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int count<T>(this ICollection<T> value)
        {
            return value != null ? value.Count : 0;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <returns>数组</returns>
        public static T[] getArray<T>(this ICollection<T> values)
        {
            if (values.Count == 0) return EmptyArray<T>.Array;
            T[] newValues = new T[values.Count];
            int index = 0;
            foreach (T value in values) newValues[index++] = value;
            if (index != newValues.Length) System.Array.Resize(ref newValues, index);
            return newValues;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="T">枚举值类型</typeparam>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>数组</returns>
        public static VT[] getArray<T, VT>(this ICollection<T> values, Func<T, VT> getValue)
        {
            if (values.Count == 0) return EmptyArray<VT>.Array;
            VT[] newValues = new VT[values.Count];
            int index = 0;
            foreach (T value in values) newValues[index++] = getValue(value);
            return newValues;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="T">枚举值类型</typeparam>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>匹配数组</returns>
        public static VT[] getFindArrayNotNull<T, VT>(this ICollection<T> values, Func<T, VT> getValue)
            where VT : class
        {
            if (values.Count != 0)
            {
                int index = 0;
                foreach (T value in values)
                {
                    if (getValue(value) != null) ++index;
                }
                if (index != 0)
                {
                    VT[] newValues = new VT[index];
                    index = 0;
                    foreach (T value in values)
                    {
                        VT arrayValue = getValue(value);
                        if (arrayValue != null) newValues[index++] = arrayValue;
                    }
                    return newValues;
                }
            }
            return EmptyArray<VT>.Array;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="T">枚举值类型</typeparam>
        /// <typeparam name="VT">返回数组类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getValue">获取数组值的委托</param>
        /// <returns>匹配数组</returns>
        public static LeftArray<VT> getFindNotNull<T, VT>(this ICollection<T> values, Func<T, VT> getValue)
            where VT : class
        {
            if (values.Count != 0)
            {
                VT[] newValues = new VT[values.Count];
                int index = 0;
                foreach (T value in values)
                {
                    VT arrayValue = getValue(value);
                    if (arrayValue != null) newValues[index++] = arrayValue;
                }
                return new LeftArray<VT>(index, newValues);
            }
            return new LeftArray<VT>(0);
        }
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="VT">枚举值类型</typeparam>
        /// <typeparam name="KT">哈希键值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>字典</returns>
        public static Dictionary<KT, VT> getDictionary<VT, KT>(this ICollection<VT> values, Func<VT, KT> getKey)
            where KT : IEquatable<KT>
        {
            Dictionary<KT, VT> dictionary = DictionaryCreator<KT>.Create<VT>(values.Count);
            foreach (VT value in values) dictionary[getKey(value)] = value;
            return dictionary;
        }

        /// <summary>
        /// 查找符合条件的记录集合
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="isValue">判断记录是否符合条件的委托</param>
        /// <returns>符合条件的记录集合</returns>
        public static LeftArray<T> getFind<T>(this ICollection<T> values, Func<T, bool> isValue)
        {
            int count = values.count();
            if (count != 0)
            {
                T[] value = new T[count];
                count = 0;
                foreach (T nextValue in values)
                {
                    if (isValue(nextValue)) value[count++] = nextValue;
                }
                return new LeftArray<T> { Array = value, Length = count };
            }
            return new LeftArray<T>(0);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="values">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString(this ICollection<string> values, string join)
        {
            return values != null ? string.Join(join, values.getArray()) : null;
        }
    }
}
