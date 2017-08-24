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
        [AutoCSer.Json.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void toJson(AutoCSer.Json.Serializer toJsoner)
        {
            new AutoCSer.Metadata.MemberMapValue<memberMapType> { MemberMap = MemberMap, Value = Value }.ToJson(toJsoner);
        }
        /// <summary>
        /// 对象转换成JSON字符串
        /// </summary>
        /// <param name="parser">Json解析器</param>
        [AutoCSer.Json.ParseCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void parseJson(AutoCSer.Json.Parser parser)
        {
            if (MemberMap == null) MemberMap = new MemberMap<memberMapType>();
            parser.MemberMap = MemberMap;
            if (Value == null)
            {
                valueType poolValue = Value = MemberMapValueLinkPool<valueType>.Pop();
                try
                {
                    AutoCSer.Json.TypeParser<valueType>.Parse(parser, ref Value);
                }
                finally
                {
                    if (poolValue != null && poolValue != Value) MemberMapValueLinkPool<valueType>.PushNotNull(poolValue);
                }
            }
            else
            {
                memberMapType parseValue = Value;
                AutoCSer.Json.TypeParser<memberMapType>.Parse(parser, ref parseValue);
            }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            new AutoCSer.Metadata.MemberMapValue<memberMapType> { MemberMap = MemberMap, Value = Value }.Serialize(serializer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer">序列化数据</param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
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
