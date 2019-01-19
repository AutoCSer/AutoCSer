using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Xml
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
                /// 集合子节点名称
                /// </summary>
                private string itemName;
                /// <summary>
                /// 是否输出判断函数
                /// </summary>
                private Func<Serializer, object, bool> isOutputMethod;
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
                public void Set(KeyValue<FieldIndex, MemberAttribute> field)
                {
                    this.field = field.Key.Member;
                    Name = field.Key.AnonymousName;
                    if (field.Value != null) itemName = field.Value.ItemName;
                    MemberIndex = field.Key.MemberIndex;
                    MethodInfo method = SerializeMethodCache.GetIsOutputMethod(this.field.FieldType);
                    if (method != null) isOutputMethod = (Func<Serializer, object, bool>)typeof(AutoCSer.Reflection.InvokeMethodReturn<,,>).MakeGenericType(typeof(Serializer), this.field.FieldType, typeof(bool)).GetMethod("getTypeObjectReturnType", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                    bool isCustom = false;
                    method = SerializeMethodCache.GetMemberMethodInfo(this.field.FieldType, ref isCustom);
                    if (isCustom) customSerializeMethod = (Action<object, Serializer>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(this.field.FieldType, typeof(Serializer)).GetMethod("getObjectType", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                    else serializeMethod = (Action<Serializer, object>)typeof(AutoCSer.Reflection.InvokeMethod<,>).MakeGenericType(typeof(Serializer), this.field.FieldType).GetMethod("getTypeObject", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                }
                /// <summary>
                /// 判断是否需要序列化
                /// </summary>
                /// <param name="serializer"></param>
                /// <param name="value"></param>
                /// <param name="fieldValue"></param>
                /// <returns></returns>
                [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public bool IsSerialize(Serializer serializer, object value, out object fieldValue)
                {
                    fieldValue = field.GetValue(value);
                    return isOutputMethod == null || isOutputMethod(serializer, fieldValue);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="serializer"></param>
                /// <param name="value"></param>
                [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void Serialize(Serializer serializer, object value)
                {
                    if (itemName != null) serializer.ItemName = itemName;
                    if (serializeMethod != null) serializeMethod(serializer, value);
                    else customSerializeMethod.Invoke(value, serializer);
                }
            }
            /// <summary>
            /// 属性信息
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            protected struct Poperty
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
                /// 集合子节点名称
                /// </summary>
                private string itemName;
                /// <summary>
                /// 是否输出判断函数
                /// </summary>
                private Func<Serializer, object, bool> isOutputMethod;
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
                public void Set(PropertyMethod property)
                {
                    this.PropertyInfo = property.Property.Member;
                    if (PropertyInfo.DeclaringType.IsValueType) getMethod = (Func<object, object>)typeof(AutoCSer.Reflection.InvokeMethodRefReturn<,>).MakeGenericType(PropertyInfo.DeclaringType, PropertyInfo.PropertyType).GetMethod("getObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { PropertyInfo.GetGetMethod(true) });
                    else getMethod = (Func<object, object>)typeof(AutoCSer.Reflection.InvokeMethodReturn<,>).MakeGenericType(PropertyInfo.DeclaringType, PropertyInfo.PropertyType).GetMethod("getObjectReturn", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { PropertyInfo.GetGetMethod(true) });
                    if (property.Attribute != null) itemName = property.Attribute.ItemName;
                    MemberIndex = property.Property.MemberIndex;
                    MethodInfo method = SerializeMethodCache.GetIsOutputMethod(this.PropertyInfo.PropertyType);
                    if (method != null) isOutputMethod = (Func<Serializer, object, bool>)typeof(AutoCSer.Reflection.InvokeMethodReturn<,,>).MakeGenericType(typeof(Serializer), this.PropertyInfo.PropertyType, typeof(bool)).GetMethod("getTypeObjectReturnType", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                    bool isCustom = false;
                    method = SerializeMethodCache.GetMemberMethodInfo(this.PropertyInfo.PropertyType, ref isCustom);
                    if (isCustom) customSerializeMethod = (Action<object, Serializer>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(PropertyInfo.PropertyType, typeof(Serializer)).GetMethod("getObjectType", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                    else serializeMethod = (Action<Serializer, object>)typeof(AutoCSer.Reflection.InvokeMethod<,>).MakeGenericType(typeof(Serializer), PropertyInfo.PropertyType).GetMethod("getTypeObject", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { method });
                }
                /// <summary>
                /// 判断是否需要序列化
                /// </summary>
                /// <param name="serializer"></param>
                /// <param name="value"></param>
                /// <param name="propertyValue"></param>
                /// <returns></returns>
                [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public bool IsSerialize(Serializer serializer, object value, out object propertyValue)
                {
                    propertyValue = getMethod(value);
                    return isOutputMethod == null || isOutputMethod(serializer, propertyValue);
                }
                /// <summary>
                /// 序列化
                /// </summary>
                /// <param name="serializer"></param>
                /// <param name="propertyValue"></param>
                [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public void Serialize(Serializer serializer, object propertyValue)
                {
                    if (itemName != null) serializer.ItemName = itemName;
                    if (serializeMethod != null) serializeMethod(serializer, propertyValue);
                    else customSerializeMethod(propertyValue, serializer);
                }
            }
            /// <summary>
            /// 字段集合
            /// </summary>
            protected Field[] fields;
            /// <summary>
            /// 属性集合
            /// </summary>
            protected Poperty[] properties;
            /// <summary>
            /// 成员序列化
            /// </summary>
            /// <param name="fields"></param>
            /// <param name="properties"></param>
            public FieldPropertySerializer(ref LeftArray<KeyValue<FieldIndex, MemberAttribute>> fields, ref LeftArray<PropertyMethod> properties)
            {
                this.fields = new Field[fields.Count];
                int index = 0;
                foreach (KeyValue<FieldIndex, MemberAttribute> field in fields) this.fields[index++].Set(field);
                this.properties = new Poperty[properties.Count];
                index = 0;
                foreach (PropertyMethod property in properties) this.properties[index++].Set(property);
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
                object fieldValue;
                if (field.IsSerialize(serializer, value, out fieldValue))
                {
                    CharStream charStream = serializer.CharStream;
                    charStream.PrepLength(field.Name.Length + 2);
                    charStream.UnsafeWrite('<');
                    charStream.UnsafeSimpleWrite(field.Name);
                    charStream.UnsafeWrite('>');
                    field.Serialize(serializer, fieldValue);
                    charStream.PrepLength(field.Name.Length + 3);
                    charStream.UnsafeWrite('<');
                    charStream.UnsafeWrite('/');
                    charStream.UnsafeSimpleWrite(field.Name);
                    charStream.UnsafeWrite('>');
                }
            }
            /// <summary>
            /// 属性序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="property"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            protected static void serialize(Serializer serializer, object value, Poperty property)
            {
                object propertyValue;
                if (property.IsSerialize(serializer, value, out propertyValue))
                {
                    PropertyInfo propertyInfo = property.PropertyInfo;
                    CharStream charStream = serializer.CharStream;
                    charStream.PrepLength(propertyInfo.Name.Length + 2);
                    charStream.UnsafeWrite('<');
                    charStream.UnsafeSimpleWrite(propertyInfo.Name);
                    charStream.UnsafeWrite('>');
                    property.Serialize(serializer, propertyValue);
                    charStream.PrepLength(propertyInfo.Name.Length + 3);
                    charStream.UnsafeWrite('<');
                    charStream.UnsafeWrite('/');
                    charStream.UnsafeSimpleWrite(propertyInfo.Name);
                    charStream.UnsafeWrite('>');
                }
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            public void Serialize(Serializer serializer, valueType value)
            {
                object objectValue = value;
                foreach (Field field in fields) serialize(serializer, objectValue, field);
                foreach (Poperty property in properties) serialize(serializer, objectValue, property);
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            public void SerializeBox(Serializer serializer, valueType value)
            {
                if (fields.Length == 0)
                {
                    Poperty property = properties[0];
                    object propertyValue;
                    if (property.IsSerialize(serializer, value, out propertyValue)) property.Serialize(serializer, propertyValue);
                }
                else
                {
                    Field field = fields[0];
                    object fieldValue;
                    if (field.IsSerialize(serializer, value, out fieldValue)) field.Serialize(serializer, fieldValue);
                }
            }
        }
    }
}
