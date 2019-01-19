using System;
using System.Collections.Generic;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 可枚举相关扩展
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="size">数据容器初始化大小</param>
        /// <returns>数组</returns>
        public static LeftArray<valueType> getLeftArray<valueType>(this IEnumerable<valueType> values, int size = 0)
        {
            if (values == null) return default(LeftArray<valueType>);
            LeftArray<valueType> array = new LeftArray<valueType>(size);
            foreach (valueType value in values) array.Add(value);
            return array;
        }
        /// <summary>
        /// 根据集合内容返回数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getValue">获取数据数组</param>
        /// <param name="size">数据容器初始化大小</param>
        /// <returns>数组</returns>
        public static LeftArray<arrayType> getLeftArray<valueType, arrayType>(this IEnumerable<valueType> values, Func<valueType, arrayType> getValue, int size = 0)
        {
            if (values == null) return default(LeftArray<arrayType>);
            LeftArray<arrayType> array = new LeftArray<arrayType>(size);
            foreach (valueType value in values) array.Add(getValue(value));
            return array;
        }
    }
}
