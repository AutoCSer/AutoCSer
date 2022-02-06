using System;

namespace AutoCSer.Xml
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
        protected readonly static StateSearcher enumSearcher = new StateSearcher(ref enumSearchData);
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        protected static void deSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            int index = enumSearcher.SearchEnum(xmlDeSerializer);
            if (index != -1) value = enumValues[index];
            else if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        /// <param name="index">第一个枚举索引</param>
        /// <param name="nextIndex">第二个枚举索引</param>
        protected static void getIndex(XmlDeSerializer xmlDeSerializer, ref T value, out int index, ref int nextIndex)
        {
            if ((index = enumSearcher.SearchFlagEnum(xmlDeSerializer)) == -1)
            {
                if (xmlDeSerializer.Config.IsMatchEnum)
                {
                    xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    return;
                }
                else
                {
                    do
                    {
                        if (xmlDeSerializer.IsNextFlagEnum() == 0) return;
                    }
                    while ((index = enumSearcher.SearchFlagEnum(xmlDeSerializer)) == -1);
                }
            }
            do
            {
                if (xmlDeSerializer.IsNextFlagEnum() == 0)
                {
                    value = enumValues[index];
                    return;
                }
                if ((nextIndex = enumSearcher.SearchFlagEnum(xmlDeSerializer)) != -1) break;
            }
            while (true);
        }
    }
}
