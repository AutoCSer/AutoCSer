using System;
using System.Collections.Generic;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 随机对象生成配置
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        /// 默认随机对象生成配置
        /// </summary>
        internal static readonly Config Default = new Config { History = DictionaryCreator.CreateAny<Type, ListArray<object>>() };
        /// <summary>
        /// 时间是否精确到秒
        /// </summary>
        public bool IsSecondDateTime;
        /// <summary>
        /// 浮点数是否转换成字符串
        /// </summary>
        public bool IsParseFloat;
        /// <summary>
        /// 是否生成字符0
        /// </summary>
        public bool IsNullChar = true;
        /// <summary>
        /// 历史对象集合
        /// </summary>
        public Dictionary<Type, ListArray<object>> History;
        /// <summary>
        /// 获取历史对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal object TryGetValue(Type type)
        {
            if (History != null && AutoCSer.Random.Default.NextBit() == 0)
            {
                ListArray<object> objects;
                if (History.TryGetValue(type, out objects)) return objects.Array[AutoCSer.Random.Default.Next(objects.Length)];
            }
            return null;
        }
        /// <summary>
        /// 保存历史对象
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal valueType SaveHistory<valueType>(valueType value)
        {
            if (History != null && value != null)
            {
                ListArray<object> objects;
                if (!History.TryGetValue(typeof(valueType), out objects)) History.Add(typeof(valueType), objects = new ListArray<object>());
                objects.Add(value);
            }
            return value;
        }
    }
}
