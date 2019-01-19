using System;
using System.Reflection;

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    internal unsafe static partial class TypeSerializer<valueType>
    {
        /// <summary>
        /// 序列化（反射模式）
        /// </summary>
        private sealed class FieldFerializer
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
                public Action<UnmanagedStream, object> SerializeMethod;
                /// <summary>
                /// 设置字段信息
                /// </summary>
                /// <param name="field"></param>
                public void Set(BinarySerialize.FieldSize field)
                {
                    Field = field.Field;
                    SerializeMethod = (Action<UnmanagedStream, object>)typeof(AutoCSer.Reflection.InvokeMethod<,>).MakeGenericType(typeof(UnmanagedStream), Field.FieldType).GetMethod("getTypeObject", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { SimpleSerialize.Serializer.GetSerializeMethod(Field.FieldType) ?? SerializeMethodCache.Get(Field.FieldType) });
                }
            }
            /// <summary>
            /// 字段集合
            /// </summary>
            private field[] fields;
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="fields"></param>
            public FieldFerializer(ref LeftArray<BinarySerialize.FieldSize> fields)
            {
                this.fields = new field[fields.Length];
                int index = 0;
                foreach (BinarySerialize.FieldSize field in fields) this.fields[index++].Set(field);
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            public void Serialize(UnmanagedStream stream, object value)
            {
                foreach (field field in fields) field.SerializeMethod(stream, field.Field.GetValue(value));
            }
        }
    }
}
