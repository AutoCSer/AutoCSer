using System;
using System.Reflection;

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    internal unsafe static partial class TypeDeSerializer<valueType>
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        private sealed class FieldDeSerializer
        {
            /// <summary>
            /// 字段信息
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            private struct field
            {
                /// <summary>
                /// 字段信息
                /// </summary>
                public FieldInfo Field;
                /// <summary>
                /// 序列化函数信息
                /// </summary>
                internal Func<DeSerializer, object, object> DeSerializeMethod;
                /// <summary>
                /// 设置字段信息
                /// </summary>
                /// <param name="field"></param>
                public void Set(BinarySerialize.FieldSize field)
                {
                    Field = field.Field;
                    DeSerializeMethod = (Func<DeSerializer, object, object>)typeof(AutoCSer.Reflection.InvokeMethodRef2<,>).MakeGenericType(typeof(DeSerializer), Field.FieldType).GetMethod("getTypeObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { DeSerializer.GetDeSerializeMethod(Field.FieldType) ?? DeSerializeMethodCache.Get(Field.FieldType) });
                }
            }
            /// <summary>
            /// 字段集合
            /// </summary>
            private field[] fields;
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="fields"></param>
            public FieldDeSerializer(ref LeftArray<BinarySerialize.FieldSize> fields)
            {
                this.fields = new field[fields.Length];
                int index = 0;
                foreach (BinarySerialize.FieldSize field in fields) this.fields[index++].Set(field);
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="deSerializer"></param>
            /// <param name="value"></param>
            public unsafe void DeSerialize(DeSerializer deSerializer, object value)
            {
                foreach (field field in fields)
                {
                    object fieldValue = field.Field.GetValue(value), newValue = field.DeSerializeMethod(deSerializer, fieldValue);
                    if (!object.ReferenceEquals(newValue, fieldValue)) field.Field.SetValue(value, newValue);
                }
            }
        }
    }
}
