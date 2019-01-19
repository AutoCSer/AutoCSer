using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型序列化
    /// </summary>
    internal unsafe static partial class TypeSerializer<valueType>
    {
        /// <summary>
        /// 序列化（反射模式）
        /// </summary>
        private class FieldPropertySerializer
        {
            /// <summary>
            /// 字段信息
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            protected struct Field
            {
                /// <summary>
                /// 字段信息
                /// </summary>
                private FieldInfo field;
                /// <summary>
                /// 字段输出名称
                /// </summary>
                public string Name;
                /// <summary>
                /// 序列化函数
                /// </summary>
                private Action<Serializer, object> serializeMethod;
                /// <summary>
                /// 自定义序列化函数
                /// </summary>
                private Action<object, Serializer> customSerializeMethod;
                /// <summary>
                /// 成员位图编号
                /// </summary>
                public int MemberIndex;
                /// <summary>
                /// 设置字段信息
                /// </summary>
                /// <param name="field"></param>
                public void Set(FieldIndex field)
                {
                    this.field = field.Member;
                    Name = field.AnonymousName;
                    MemberIndex = field.MemberIndex;
                    bool isCustom = false;
                    MethodInfo method = SerializeMethodCache.GetMemberMethodInfo(this.field.FieldType, ref isCustom);
                    if(isCustom) customSerializeMethod = (Action<object, Serializer>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(this.field.FieldType, typeof(Serializer)).GetMethod("getObjectType", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                    else serializeMethod = (Action<Serializer, object>)typeof(AutoCSer.Reflection.InvokeMethod<,>).MakeGenericType(typeof(Serializer), this.field.FieldType).GetMethod("getTypeObject", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="serializer"></param>
                /// <param name="value"></param>
                [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void Serialize(Serializer serializer, object value)
                {
                    if (serializeMethod != null) serializeMethod(serializer, field.GetValue(value));
                    else customSerializeMethod(field.GetValue(value), serializer);
                }
            }
            /// <summary>
            /// 属性信息
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            protected struct Property
            {
                /// <summary>
                /// 属性信息
                /// </summary>
                public PropertyInfo PropertyInfo;
                /// <summary>
                /// 获取函数
                /// </summary>
                private Func<object, object> getMethod;
                /// <summary>
                /// 序列化函数
                /// </summary>
                private Action<Serializer, object> serializeMethod;
                /// <summary>
                /// 自定义序列化函数
                /// </summary>
                private Action<object, Serializer> customSerializeMethod;
                /// <summary>
                /// 成员位图编号
                /// </summary>
                public int MemberIndex;
                /// <summary>
                /// 设置属性信息
                /// </summary>
                /// <param name="property"></param>
                public void Set(KeyValue<PropertyIndex, MethodInfo> property)
                {
                    PropertyInfo = property.Key.Member;
                    if (PropertyInfo.DeclaringType.IsValueType) getMethod = (Func<object, object>)typeof(AutoCSer.Reflection.InvokeMethodRefReturn<,>).MakeGenericType(PropertyInfo.DeclaringType, PropertyInfo.PropertyType).GetMethod("getObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { PropertyInfo.GetGetMethod(true) });
                    else getMethod = (Func<object, object>)typeof(AutoCSer.Reflection.InvokeMethodReturn<,>).MakeGenericType(PropertyInfo.DeclaringType, PropertyInfo.PropertyType).GetMethod("getObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { PropertyInfo.GetGetMethod(true) });
                    MemberIndex = property.Key.MemberIndex;
                    bool isCustom = false;
                    MethodInfo method = SerializeMethodCache.GetMemberMethodInfo(PropertyInfo.PropertyType, ref isCustom);
                    if (isCustom) customSerializeMethod = (Action<object, Serializer>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(PropertyInfo.PropertyType, typeof(Serializer)).GetMethod("getObjectType", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                    else serializeMethod = (Action<Serializer, object>)typeof(AutoCSer.Reflection.InvokeMethod<,>).MakeGenericType(typeof(Serializer), PropertyInfo.PropertyType).GetMethod("getTypeObject", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="serializer"></param>
                /// <param name="value"></param>
                [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void Serialize(Serializer serializer, object value)
                {
                    if (serializeMethod != null) serializeMethod(serializer, getMethod(value));
                    else customSerializeMethod(getMethod(value), serializer);
                }
            }
            /// <summary>
            /// 字段集合
            /// </summary>
            protected Field[] fields;
            /// <summary>
            /// 属性集合
            /// </summary>
            protected Property[] properties;
            /// <summary>
            /// 成员序列化
            /// </summary>
            /// <param name="fields"></param>
            /// <param name="properties"></param>
            public FieldPropertySerializer(ref LeftArray<FieldIndex> fields, ref LeftArray<KeyValue<PropertyIndex, MethodInfo>> properties)
            {
                this.fields = new Field[fields.Count];
                int index = 0;
                foreach (FieldIndex field in fields) this.fields[index++].Set(field);
                this.properties = new Property[properties.Count];
                index = 0;
                foreach (KeyValue<PropertyIndex, MethodInfo> property in properties) this.properties[index++].Set(property);
            }
            /// <summary>
            /// 字段序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="field"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            protected static void serialize(Serializer serializer, object value, Field field)
            {
                CharStream charStream = serializer.CharStream;
                charStream.PrepLength(field.Name.Length + 5);
                charStream.UnsafeWrite('"');
                charStream.UnsafeSimpleWrite(field.Name);
                charStream.UnsafeWrite('"');
                charStream.UnsafeWrite(':');
                field.Serialize(serializer, value);
            }
            /// <summary>
            /// 属性序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="property"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            protected static void serialize(Serializer serializer, object value, Property property)
            {
                PropertyInfo propertyInfo = property.PropertyInfo;
                CharStream charStream = serializer.CharStream;
                charStream.PrepLength(propertyInfo.Name.Length + 5);
                charStream.UnsafeWrite('"');
                charStream.UnsafeSimpleWrite(propertyInfo.Name);
                charStream.UnsafeWrite('"');
                charStream.UnsafeWrite(':');
                property.Serialize(serializer, value);
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            public void Serialize(Serializer serializer, valueType value)
            {
                object objectValue = value;
                CharStream charStream = serializer.CharStream;
                byte isNext = 0;
                foreach (Field field in fields)
                {
                    if (isNext == 0) isNext = 1;
                    else charStream.Write(',');
                    serialize(serializer, objectValue, field);
                }
                foreach (Property property in properties)
                {
                    if (isNext == 0) isNext = 1;
                    else charStream.Write(',');
                    serialize(serializer, objectValue, property);
                }
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            public void SerializeBox(Serializer serializer, valueType value)
            {
                if (fields.Length == 0) properties[0].Serialize(serializer, value);
                else fields[0].Serialize(serializer, value);
            }
        }
    }
}
