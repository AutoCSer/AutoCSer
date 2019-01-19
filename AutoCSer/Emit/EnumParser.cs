using System;
using System.Collections.Generic;
using AutoCSer.Extension;

namespace AutoCSer.Emit
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    /// <typeparam name="valueType">目标类型</typeparam>
    internal abstract class EnumParser<valueType>
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        protected static readonly valueType[] enumValues;
        /// <summary>
        /// 枚举名称查找数据
        /// </summary>
        protected static Pointer enumSearchData;

        static EnumParser()
        {
            Dictionary<string, valueType> values = ((valueType[])System.Enum.GetValues(typeof(valueType))).getDictionary(value => value.ToString());
            enumValues = values.getArray(value => value.Value);
            enumSearchData = AutoCSer.StateSearcher.CharBuilder.Create(values.getArray(value => value.Key), true).Pointer;
        }
    }
}
