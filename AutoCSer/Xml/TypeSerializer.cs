using System;
using AutoCSer.Metadata;
using System.Reflection;
using AutoCSer.Extension;
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
        private static readonly Action<Serializer, valueType> memberSerializer;
        /// <summary>
        /// 成员转换
        /// </summary>
#if NOJIT
        private static readonly Action<MemberMap, Serializer, object> memberMapSerializer;
#else
        private static readonly Action<MemberMap, Serializer, valueType> memberMapSerializer;
#endif
        /// <summary>
        /// 转换委托
        /// </summary>
        private static readonly Action<Serializer, valueType> defaultSerializer;
        /// <summary>
        /// XML序列化类型配置
        /// </summary>
        private static readonly SerializeAttribute attribute;
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
        internal static void Serialize(Serializer xmlSerializer, valueType value)
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
        internal static void StructSerialize(Serializer xmlSerializer, valueType value)
        {
            if (defaultSerializer == null) MemberSerialize(xmlSerializer, value);
            else defaultSerializer(xmlSerializer, value);
        }
        /// <summary>
        /// 引用类型对象转换XML字符串
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        internal static void ClassSerialize(Serializer xmlSerializer, valueType value)
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
        internal static void MemberSerialize(Serializer xmlSerializer, valueType value)
        {
            //charStream xmlStream = xmlSerializer.CharStream;
            SerializeConfig config = xmlSerializer.Config;
            MemberMap memberMap = config.MemberMap;
            if (memberMap == null) memberSerializer(xmlSerializer, value);
            else if (memberMap.Type == MemberMap<valueType>.TypeInfo)
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
        private static void enumToString(Serializer xmlSerializer, valueType value)
        {
            xmlSerializer.Serialize(value.ToString());
        }
        /// <summary>
        /// 不支持序列化
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void ignore(Serializer xmlSerializer, valueType value) { }

        static TypeSerializer()
        {
            Type type = typeof(valueType);
            MethodInfo methodInfo = Serializer.GetSerializeMethod(type);
            if (methodInfo != null)
            {
                defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                isValueType = true;
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1) defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.GetArray(type.GetElementType()));
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
                    //defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.GetNullable(type));
                    defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), StructGenericType.Get(type.GetGenericArguments()[0]).XmlSerializeNullableMethod);
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
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomXmlSerializer", null, new Type[] { typeof(Serializer), type }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarga_S, 1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    defaultSerializer = (Action<Serializer, valueType>)dynamicMethod.CreateDelegate(typeof(Action<Serializer, valueType>));
#endif
                }
                else defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                isValueType = true;
            }
            else if ((methodInfo = SerializeMethodCache.GetIEnumerable(type)) != null)
            {
                defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                isValueType = true;
            }
            else
            {
                Type attributeType;
                attribute = type.customAttribute<SerializeAttribute>(out attributeType) ?? (type.Name[0] == '<' ? SerializeAttribute.AnonymousTypeMember : Serializer.DefaultAttribute);
                if (type.IsValueType) isValueType = true;
                else if (attribute != Serializer.DefaultAttribute && attributeType != type)
                {
                    for (Type baseType = type.BaseType; baseType != typeof(object); baseType = baseType.BaseType)
                    {
                        SerializeAttribute baseAttribute = baseType.customAttribute<SerializeAttribute>();
                        if (baseAttribute != null)
                        {
                            if (baseAttribute.IsBaseType)
                            {
                                methodInfo = SerializeMethodCache.BaseSerializeMethod.MakeGenericMethod(baseType, type);
                                defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                                return;
                            }
                            break;
                        }
                    }
                }
                LeftArray<KeyValue<FieldIndex, MemberAttribute>> fields = SerializeMethodCache.GetFields(MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), attribute);
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
                foreach (KeyValue<FieldIndex, MemberAttribute> member in fields)
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
                memberSerializer = (Action<Serializer, valueType>)dynamicMethod.Create<Action<Serializer, valueType>>();
                if (isBox) defaultSerializer = memberSerializer;
                else memberMapSerializer = (Action<MemberMap, Serializer, valueType>)memberMapDynamicMethod.Create<Action<MemberMap, Serializer, valueType>>();
#endif
            }
        }
    }
}
