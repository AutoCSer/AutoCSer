using System;
using System.Reflection;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    internal unsafe partial class TypeDeSerializer<valueType>
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        private sealed class FieldDeSerializer
        {
            /// <summary>
            /// 字段集合
            /// </summary>
            private DeSerializeField[] fields;
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="fields"></param>
            public FieldDeSerializer(ref LeftArray<FieldSize> fields)
            {
                this.fields = new DeSerializeField[fields.Length];
                int index = 0;
                foreach (FieldSize field in fields) this.fields[index++].Set(field);
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="deSerializer"></param>
            /// <param name="value"></param>
            public void DeSerialize(DeSerializer deSerializer, ref valueType value)
            {
                object objectValue = value;
                foreach (DeSerializeField field in fields)
                {
                    if (field.Field.IsStatic) field.DeSerializeMethod(deSerializer, AutoCSer.Emit.ConstructorAttribute.GetDefault(field.Field.FieldType));
                    else
                    {
                        object fieldValue = field.Field.GetValue(objectValue), newValue = field.DeSerializeMethod(deSerializer, fieldValue);
                        if (!object.ReferenceEquals(newValue, fieldValue)) field.Field.SetValue(objectValue, newValue);
                    }
                }
                value = (valueType)objectValue;
            }
        }
    }
}
