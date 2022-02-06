using System;
using AutoCSer.Metadata;
using System.Reflection;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Xml
{
    /// <summary>
    /// 类型序列化
    /// </summary>
    /// <typeparam name="valueType">目标类型</typeparam>
    internal unsafe static partial class TypeSerializer<valueType>
    {
        /// <summary>
        /// 成员转换
        /// </summary>
        private static readonly Action<XmlSerializer, valueType> memberSerializer;
        /// <summary>
        /// 成员转换
        /// </summary>
#if NOJIT
        private static readonly Action<MemberMap, Serializer, object> memberMapSerializer;
#else
        private static readonly Action<MemberMap, XmlSerializer, valueType> memberMapSerializer;
#endif
        /// <summary>
        /// 转换委托
        /// </summary>
        private static readonly Action<XmlSerializer, valueType> defaultSerializer;
        /// <summary>
        /// XML序列化类型配置
        /// </summary>
        private static readonly XmlSerializeAttribute attribute;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(XmlSerializer xmlSerializer, valueType value)
        {
            if (isValueType) StructSerialize(xmlSerializer, value);
            else if (value != null) ClassSerialize(xmlSerializer, value);
        }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize(XmlSerializer xmlSerializer, valueType value)
        {
            if (defaultSerializer == null) MemberSerialize(xmlSerializer, value);
            else defaultSerializer(xmlSerializer, value);
        }
        /// <summary>
        /// 引用类型对象转换XML字符串
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        internal static void ClassSerialize(XmlSerializer xmlSerializer, valueType value)
        {
            if (defaultSerializer == null)
            {
                if (xmlSerializer.Push(value))
                {
                    MemberSerialize(xmlSerializer, value);
                    xmlSerializer.Pop();
                }
            }
            else defaultSerializer(xmlSerializer, value);
        }
        /// <summary>
        /// 值类型对象转换XML字符串
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        internal static void MemberSerialize(XmlSerializer xmlSerializer, valueType value)
        {
            //charStream xmlStream = xmlSerializer.CharStream;
            SerializeConfig config = xmlSerializer.Config;
            MemberMap memberMap = config.MemberMap;
            if (memberMap == null) memberSerializer(xmlSerializer, value);
            else if (object.ReferenceEquals(memberMap.Type, MemberMap<valueType>.MemberMapType))
            {
                config.MemberMap = null;
                try
                {
                    memberMapSerializer(memberMap, xmlSerializer, value);
                }
                finally { config.MemberMap = memberMap; }
            }
            else
            {
                xmlSerializer.Warning = SerializeWarning.MemberMap;
                if (config.IsMemberMapErrorToDefault) memberSerializer(xmlSerializer, value);
            }
        }

        /// <summary>
        /// 枚举转换字符串
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void enumToString(XmlSerializer xmlSerializer, valueType value)
        {
            xmlSerializer.CallSerialize(value.ToString());
        }
        /// <summary>
        /// 不支持序列化
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void ignore(XmlSerializer xmlSerializer, valueType value) { }

        static TypeSerializer()
        {
            Type type = typeof(valueType);
            MethodInfo methodInfo = XmlSerializer.GetSerializeMethod(type);
            if (methodInfo != null)
            {
                defaultSerializer = (Action<XmlSerializer, valueType>)Delegate.CreateDelegate(typeof(Action<XmlSerializer, valueType>), methodInfo);
                isValueType = true;
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1) defaultSerializer = (Action<XmlSerializer, valueType>)SerializeMethodCache.GetArray(type.GetElementType());
                else defaultSerializer = ignore;
                isValueType = true;
                return;
            }
            if (type.IsEnum)
            {
                defaultSerializer = enumToString;
                isValueType = true;
                return;
            }
            if (type.IsInterface || type.IsPointer || typeof(Delegate).IsAssignableFrom(type))
            {
                defaultSerializer = ignore;
                isValueType = true;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    defaultSerializer = (Action<XmlSerializer, valueType>)StructGenericType.Get(type.GetGenericArguments()[0]).XmlSerializeNullableMethod;
                    isValueType = true;
                    return;
                }
            }
            if ((methodInfo = SerializeMethodCache.GetCustom(type)) != null)
            {
                if (type.IsValueType)
                {
#if NOJIT
                    defaultSerializer = new CustomSerializer(methodInfo).Serialize;
#else
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomXmlSerializer", null, new Type[] { typeof(XmlSerializer), type }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarga_S, 1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    defaultSerializer = (Action<XmlSerializer, valueType>)dynamicMethod.CreateDelegate(typeof(Action<XmlSerializer, valueType>));
#endif
                }
                else defaultSerializer = (Action<XmlSerializer, valueType>)Delegate.CreateDelegate(typeof(Action<XmlSerializer, valueType>), methodInfo);
                isValueType = true;
            }
            else
            {
                Delegate enumerableDelegate = SerializeMethodCache.GetIEnumerable(type);
                if (enumerableDelegate != null)
                {
                    defaultSerializer = (Action<XmlSerializer, valueType>)enumerableDelegate;
                    isValueType = true;
                }
                else
                {
                    Type attributeType;
                    attribute = type.customAttribute<XmlSerializeAttribute>(out attributeType) ?? (type.Name[0] == '<' ? XmlSerializeAttribute.AnonymousTypeMember : XmlSerializer.DefaultAttribute);
                    if (type.IsValueType) isValueType = true;
                    else if (attribute != XmlSerializer.DefaultAttribute && attributeType != type)
                    {
                        for (Type baseType = type.BaseType; baseType != typeof(object); baseType = baseType.BaseType)
                        {
                            XmlSerializeAttribute baseAttribute = baseType.customAttribute<XmlSerializeAttribute>();
                            if (baseAttribute != null)
                            {
                                if (baseAttribute.IsBaseType)
                                {
                                    methodInfo = SerializeMethodCache.BaseSerializeMethod.MakeGenericMethod(baseType, type);
                                    defaultSerializer = (Action<XmlSerializer, valueType>)Delegate.CreateDelegate(typeof(Action<XmlSerializer, valueType>), methodInfo);
                                    return;
                                }
                                break;
                            }
                        }
                    }
                    LeftArray<KeyValue<FieldIndex, XmlSerializeMemberAttribute>> fields = SerializeMethodCache.GetFields(MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), attribute);
                    LeftArray<PropertyMethod> properties = SerializeMethodCache.GetProperties(MemberIndexGroup<valueType>.GetProperties(attribute.MemberFilters), attribute);
                    bool isBox = false;
                    if (type.IsValueType && fields.Length + properties.Length == 1)
                    {
                        BoxSerializeAttribute boxSerialize = AutoCSer.Metadata.TypeAttribute.GetAttribute<BoxSerializeAttribute>(type);
                        if (boxSerialize != null && boxSerialize.IsXml) isBox = true;
                    }
#if NOJIT
                if (isBox) defaultSerializer = memberSerializer = new FieldPropertySerializer(ref fields, ref properties).SerializeBox;
                else
                {
                    memberSerializer = new FieldPropertySerializer(ref fields, ref properties).Serialize;
                    memberMapSerializer = new MemberMapSerializer(ref fields, ref properties).Serialize;
                }
#else
                    SerializeMemberDynamicMethod dynamicMethod = new SerializeMemberDynamicMethod(type);
                    SerializeMemberMapDynamicMethod memberMapDynamicMethod = isBox ? default(SerializeMemberMapDynamicMethod) : new SerializeMemberMapDynamicMethod(type);
                    foreach (KeyValue<FieldIndex, XmlSerializeMemberAttribute> member in fields)
                    {
                        if (isBox) dynamicMethod.PushBox(member.Key);
                        else
                        {
                            dynamicMethod.Push(member.Key, member.Value);
                            memberMapDynamicMethod.Push(member.Key, member.Value);
                        }
                    }
                    foreach (PropertyMethod member in properties)
                    {
                        if (isBox) dynamicMethod.PushBox(member.Property, member.Method);
                        else
                        {
                            dynamicMethod.Push(member.Property, member.Method, member.Attribute);
                            memberMapDynamicMethod.Push(member.Property, member.Method, member.Attribute);
                        }
                    }
                    memberSerializer = (Action<XmlSerializer, valueType>)dynamicMethod.Create<Action<XmlSerializer, valueType>>();
                    if (isBox) defaultSerializer = memberSerializer;
                    else memberMapSerializer = (Action<MemberMap, XmlSerializer, valueType>)memberMapDynamicMethod.Create<Action<MemberMap, XmlSerializer, valueType>>();
#endif
                }
            }
        }
    }
}
