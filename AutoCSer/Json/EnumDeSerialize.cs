using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class EnumDeSerialize<T> : AutoCSer.TextSerialize.EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举名称查找数据
        /// </summary>
        protected readonly static StateSearcher enumSearcher = new StateSearcher(enumSearchData);
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        protected static void deSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            int index = enumSearcher.SearchString(jsonDeSerializer);
            if (index != -1) value = enumValues[index];
            else if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) jsonDeSerializer.SearchStringEnd();
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        /// <param name="index">第一个枚举索引</param>
        /// <param name="nextIndex">第二个枚举索引</param>
        protected static void getIndex(JsonDeSerializer jsonDeSerializer, ref T value, out int index, ref int nextIndex)
        {
            if ((index = enumSearcher.SearchFlagEnum(jsonDeSerializer)) == -1)
            {
                if (!jsonDeSerializer.Config.IsMatchEnum)
                {
                    do
                    {
                        if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success || jsonDeSerializer.Quote == 0) return;
                    }
                    while ((index = enumSearcher.NextFlagEnum(jsonDeSerializer)) == -1);
                }
                else
                {
                    jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    return;
                }
            }
            do
            {
                if (jsonDeSerializer.Quote == 0)
                {
                    value = enumValues[index];
                    return;
                }
                if ((nextIndex = enumSearcher.NextFlagEnum(jsonDeSerializer)) != -1) break;
            }
            while (jsonDeSerializer.DeSerializeState == DeSerializeState.Success);
        }
    }
}
