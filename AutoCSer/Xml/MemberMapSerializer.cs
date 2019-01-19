using System;
using AutoCSer.Metadata;

namespace AutoCSer.Xml
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
            public MemberMapSerializer(ref LeftArray<KeyValue<FieldIndex, MemberAttribute>> fields, ref LeftArray<PropertyMethod> properties) : base(ref fields, ref properties) { }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            public void Serialize(MemberMap memberMap, Serializer serializer, object value)
            {
                foreach (Field field in fields)
                {
                    if (memberMap.IsMember(field.MemberIndex)) serialize(serializer, value, field);
                }
                foreach (Poperty property in properties)
                {
                    if (memberMap.IsMember(property.MemberIndex)) serialize(serializer, value, property);
                }
            }
        }
    }
}
