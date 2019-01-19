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
        /// 转换成字典
        /// </summary>
        /// <typeparam name="valueType">枚举值类型</typeparam>
        /// <typeparam name="keyType">哈希键值类型</typeparam>
        /// <param name="values">值集合</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>字典</returns>
        public static Dictionary<keyType, valueType> getDictionary<valueType, keyType>
            (this IEnumerable<valueType> values, Func<valueType, keyType> getKey)
            where keyType : IEquatable<keyType>
        {
            if (values != null)
            {
                Dictionary<keyType, valueType> dictionary = DictionaryCreator<keyType>.Create<valueType>();
                foreach (valueType value in values) dictionary[getKey(value)] = value;
                return dictionary;
            }
            return null;
        }
    }
}
