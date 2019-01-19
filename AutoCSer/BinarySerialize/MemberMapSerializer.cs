using System;
using System.Reflection;
using AutoCSer.Metadata;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    internal unsafe static partial class TypeSerializer<valueType>
    {
        /// <summary>
        /// 序列化（反射模式）
        /// </summary>
        private sealed class MemberMapSerializer
        {
            /// <summary>
            /// 字段集合
            /// </summary>
            private SerializeField[] fields;
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="fields"></param>
            public MemberMapSerializer(ref LeftArray<FieldSize> fields)
            {
                this.fields = new SerializeField[fields.Length];
                int index = 0;
                foreach (FieldSize field in fields) this.fields[index++].Set(field);
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            public void Serialize(MemberMap memberMap, Serializer serializer, object value)
            {
                foreach (SerializeField field in fields)
                {
                    if (memberMap.IsMember(field.MemberIndex))
                    {
                        field.SerializeMethod(serializer, field.Field.GetValue(value));
                    }
                }
            }
        }
    }
}
