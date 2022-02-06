using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using AutoCSer.Extensions;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 枚举值反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class EnumDeSerialize<T> where T : struct, IConvertible// System.Enum
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        protected static readonly T[] enumValues;
        /// <summary>
        /// 枚举名称查找数据
        /// </summary>
        protected static Pointer enumSearchData;

        static EnumDeSerialize()
        {
            Dictionary<string, T> values = ((T[])System.Enum.GetValues(typeof(T))).getDictionary(value => value.ToString());
            enumValues = values.getArray(value => value.Value);
            enumSearchData = AutoCSer.StateSearcher.CharBuilder.Create(values.getArray(value => value.Key), true);
        }
    }
}
