using System;
using System.Reflection;

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
        private sealed class FieldFerializer
        {
            /// <summary>
            /// 字段集合
            /// </summary>
            private SerializeField[] fields;
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="fields"></param>
            public FieldFerializer(ref LeftArray<FieldSize> fields)
            {
                this.fields = new SerializeField[fields.Length];
                int index = 0;
                foreach (FieldSize field in fields) this.fields[index++].Set(field);
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            public void Serialize(Serializer serializer, object value)
            {
                foreach (SerializeField field in fields)
                {
                    field.SerializeMethod(serializer, field.Field.GetValue(value));
                }
            }
        }
    }
}
