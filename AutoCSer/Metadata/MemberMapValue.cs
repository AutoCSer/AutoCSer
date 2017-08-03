using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员位图对象绑定
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MemberMapValue<valueType>
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap MemberMap;
        /// <summary>
        /// 目标数据
        /// </summary>
        internal valueType Value;
        /// <summary>
        /// 对象转换成JSON字符串
        /// </summary>
        /// <param name="serializer">对象转换成JSON字符串</param>
        [AutoCSer.Json.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void ToJson(AutoCSer.Json.Serializer serializer)
        {
            MemberMap memberMap = MemberMap;
            if (memberMap == null || memberMap.IsDefault) serializer.TypeSerialize(Value);
            else
            {
                MemberMapValueJsonSerializeConfig config = MemberMapValueJsonSerializeConfig.Pop() ?? new MemberMapValueJsonSerializeConfig();
                AutoCSer.Json.SerializeConfig oldConfig = serializer.Config;
                AutoCSer.MemberCopy.Copyer<AutoCSer.Json.SerializeConfig>.Copy(config, oldConfig);
                (serializer.Config = config).MemberMap = memberMap;
                try
                {
                    serializer.TypeSerialize(Value);
                }
                finally
                {
                    serializer.Config = oldConfig;
                    config.MemberMap = null;
                    MemberMapValueJsonSerializeConfig.PushNotNull(config);
                }
            }
        }
        /// <summary>
        /// 对象转换成JSON字符串
        /// </summary>
        /// <param name="parser">Json解析器</param>
        [AutoCSer.Json.ParseCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void parseJson(AutoCSer.Json.Parser parser)
        {
            if (MemberMap == null) MemberMap = new MemberMap<valueType>();
            parser.MemberMap = MemberMap;
            parser.TypeParse(ref Value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void Serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            if (MemberMap == null || MemberMap.IsDefault) serializer.TypeSerialize(Value);
            else
            {
                AutoCSer.BinarySerialize.SerializeMemberMap serializeMemberMap = serializer.GetCustomMemberMap(MemberMap);
                try
                {
                    serializer.TypeSerialize(Value);
                }
                finally { serializer.SetCustomMemberMap(ref serializeMemberMap); }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            MemberMap memberMap = deSerializer.SetCustomMemberMap(MemberMap);
            try
            {
                deSerializer.TypeDeSerialize(ref Value);
            }
            finally { MemberMap = deSerializer.SetCustomMemberMap(memberMap); }
        }
    }
    /// <summary>
    /// 成员位图对象绑定
    /// </summary>
    /// <typeparam name="memberMapType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct MemberMapValue<memberMapType, valueType>
        where valueType : class, memberMapType
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
        /// 对象转换成JSON字符串
        /// </summary>
        /// <param name="toJsoner">对象转换成JSON字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Json.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void toJson(AutoCSer.Json.Serializer toJsoner)
        {
            new MemberMapValue<memberMapType> { MemberMap = MemberMap, Value = Value }.ToJson(toJsoner);
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
            if (Value == null) parser.TypeParse(ref Value);
            else
            {
                memberMapType parseValue = Value;
                parser.TypeParse(ref parseValue);
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
            new MemberMapValue<memberMapType> { MemberMap = MemberMap, Value = Value }.Serialize(serializer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            MemberMap memberMap = deSerializer.SetCustomMemberMap(MemberMap);
            try
            {
                if (Value == null) deSerializer.TypeDeSerialize(ref Value);
                else
                {
                    memberMapType parseValue = Value;
                    deSerializer.TypeDeSerialize(ref parseValue);
                }
            }
            finally { MemberMap = (MemberMap<memberMapType>)deSerializer.SetCustomMemberMap(memberMap); }
        }
    }
}
