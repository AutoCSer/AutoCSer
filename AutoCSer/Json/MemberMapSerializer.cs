using System;
using System.Reflection;
using AutoCSer.Metadata;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型序列化
    /// </summary>
    internal unsafe static partial class TypeSerializer<valueType>
    {
        /// <summary>
        /// 成员序列化（反射模式）
        /// </summary>
        private sealed class MemberMapSerializer : FieldPropertySerializer
        {
            /// <summary>
            /// 成员序列化
            /// </summary>
            /// <param name="fields"></param>
            /// <param name="properties"></param>
            public MemberMapSerializer(ref LeftArray<FieldIndex> fields, ref LeftArray<KeyValue<PropertyIndex, MethodInfo>> properties) : base(ref fields, ref properties) { }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="charStream"></param>
            public void Serialize(MemberMap memberMap, Serializer serializer, object value, CharStream charStream)
            {
                byte isNext = 0;
                foreach (Field field in fields)
                {
                    if (memberMap.IsMember(field.MemberIndex))
                    {
                        if (isNext == 0) isNext = 1;
                        else charStream.Write(',');
                        serialize(serializer, value, field);
                    }
                }
                foreach (Property property in properties)
                {
                    if (memberMap.IsMember(property.MemberIndex))
                    {
                        if (isNext == 0) isNext = 1;
                        else charStream.Write(',');
                        serialize(serializer, value, property);
                    }
                }
            }
        }
    }
}
