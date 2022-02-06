using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 反序列化池对象
    /// </summary>
    /// <typeparam name="memberMapType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MemberMapValue<memberMapType, valueType>
        where valueType : class, memberMapType, IMemberMapValueLink<valueType>
        where memberMapType : class
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap<memberMapType> MemberMap;
        /// <summary>
        /// 目标数据
        /// </summary>
        internal valueType Value;
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        internal void Set(valueType value, MemberMap<memberMapType> memberMap)
        {
            Value = value;
            MemberMap = memberMap;
        }
        /// <summary>
        /// 对象转换成JSON字符串
        /// </summary>
        /// <param name="toJsoner">对象转换成JSON字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.JsonSerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void toJson(AutoCSer.JsonSerializer toJsoner)
        {
            new AutoCSer.Metadata.MemberMapValue<memberMapType> { MemberMap = MemberMap, Value = Value }.ToJson(toJsoner);
        }
        /// <summary>
        /// 对象转换成JSON字符串
        /// </summary>
        /// <param name="parser">Json解析器</param>
        [AutoCSer.JsonDeSerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void parseJson(AutoCSer.JsonDeSerializer parser)
        {
            if (MemberMap == null) MemberMap = new MemberMap<memberMapType>();
            parser.MemberMap = MemberMap;
            if (Value == null)
            {
                valueType poolValue = Value = MemberMapValueLinkPool<valueType>.Pop();
                try
                {
                    AutoCSer.Json.TypeDeSerializer<valueType>.DeSerialize(parser, ref Value);
                }
                finally
                {
                    if (poolValue != null && poolValue != Value) MemberMapValueLinkPool<valueType>.PushNotNull(poolValue);
                }
            }
            else
            {
                memberMapType parseValue = Value;
                AutoCSer.Json.TypeDeSerializer<memberMapType>.DeSerialize(parser, ref parseValue);
            }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerializer serializer)
        {
            new AutoCSer.Metadata.MemberMapValue<memberMapType> { MemberMap = MemberMap, Value = Value }.Serialize(serializer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer">序列化数据</param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
        {
            if (deSerializer.CheckNullValue() == 0) Value = default(valueType);
            else
            {
                MemberMap oldMemberMap = deSerializer.MemberMap;
                deSerializer.MemberMap = MemberMap;
                try
                {
                    if (Value == null)
                    {
                        valueType poolValue = Value = MemberMapValueLinkPool<valueType>.Pop();
                        try
                        {
                            AutoCSer.BinarySerialize.TypeDeSerializer<valueType>.DeSerialize(deSerializer, ref Value);
                        }
                        finally
                        {
                            if (poolValue != null && poolValue != Value) MemberMapValueLinkPool<valueType>.PushNotNull(poolValue);
                        }
                    }
                    else
                    {
                        memberMapType parseValue = Value;
                        AutoCSer.BinarySerialize.TypeDeSerializer<memberMapType>.DeSerialize(deSerializer, ref parseValue);
                    }
                }
                finally
                {
                    MemberMap = (MemberMap<memberMapType>)deSerializer.MemberMap;
                    deSerializer.MemberMap = oldMemberMap;
                }
            }
        }
    }
}
