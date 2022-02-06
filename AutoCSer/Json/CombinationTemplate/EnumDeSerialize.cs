using System;
/*ulong,ULong,Unsigned;long,Long,Signed;uint,UInt,Unsigned;int,Int,Signed;ushort,UShort,Unsigned;short,Short,Signed;byte,Byte,Unsigned;sbyte,SByte,Signed*/

namespace AutoCSer.Json
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
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryDeSerializeNumber(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.IsEnumNumberUnsigned())
            {
                ulong intValue = 0;
                jsonDeSerializer.CallSerialize(ref intValue);
                value = AutoCSer.Metadata.EnumGenericType<T, ulong>.FromInt(intValue);
            }
            else if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success) return false;
            return true;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value)) deSerialize(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value">目标数据</param>
        public static void DeSerializeFlags(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (!tryDeSerializeNumber(jsonDeSerializer, ref value))
            {
                if (enumSearcher.State == null)
                {
                    if (jsonDeSerializer.Config.IsMatchEnum) jsonDeSerializer.DeSerializeState = DeSerializeState.NoFoundEnumValue;
                    else jsonDeSerializer.Ignore();
                }
                else
                {
                    int index, nextIndex = -1;
                    getIndex(jsonDeSerializer, ref value, out index, ref nextIndex);
                    if (nextIndex != -1)
                    {
                        ulong intValue = enumInts.ULong[index];
                        intValue |= enumInts.ULong[nextIndex];
                        while (jsonDeSerializer.Quote != 0)
                        {
                            index = enumSearcher.NextFlagEnum(jsonDeSerializer);
                            if (jsonDeSerializer.DeSerializeState == DeSerializeState.Success)
                            {
                                if (index != -1) intValue |= enumInts.ULong[index];
                            }
                            else return;
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
