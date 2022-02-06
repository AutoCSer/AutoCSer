using System;
/*ulong,ULong,Unsigned;long,Long,Signed;uint,UInt,Unsigned;int,Int,Signed;ushort,UShort,Unsigned;short,Short,Signed;byte,Byte,Unsigned;sbyte,SByte,Signed*/

namespace AutoCSer.Xml
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    internal sealed unsafe class EnumULongDeSerialize<T> : EnumDeSerialize<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        private static AutoCSer.Memory.Pointer enumInts;
        /// <summary>
        /// 数值解析
        /// </summary>
        /// <param name="xmlDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (xmlDeSerializer.IsEnumNumberUnsigned())
            {
                ulong intValue = 0;
                xmlDeSerializer.DeSerializeNumber(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
            }
            else if (xmlDeSerializer.State == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value)) deSerialize(xmlDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="xmlDeSerializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeFlags(XmlDeSerializer xmlDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(xmlDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (xmlDeSerializer.Config.IsMatchEnum) xmlDeSerializer.State = DeSerializeState.NoFoundEnumValue;
                    else xmlDeSerializer.IgnoreSearchValue();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(xmlDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        ulong intValue = enumInts.ULong[index];
                        intValue |= enumInts.ULong[nextIndex];
                        while (xmlDeSerializer.IsNextFlagEnum() != 0)
                        {
                            if ((index = enumSearcher.NextFlagEnum(xmlDeSerializer)) != -1) intValue |= enumInts.ULong[index];
                        }
                        value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
                    }
                }
            }
        }

        static EnumULongDeSerialize()
        {
            enumInts = AutoCSer.Memory.Unmanaged.GetStaticPointer(enumValues.Length * sizeof(ulong), false);
            ulong* data = enumInts.ULong;
            foreach (T value in enumValues) *(ulong*)data++ = AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value);
        }
    }
}
