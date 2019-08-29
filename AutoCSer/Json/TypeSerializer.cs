using System;
using AutoCSer.Metadata;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Json
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
#if NOJIT
        private static readonly Action<MemberMap, Serializer, object, CharStream> memberMapSerializer;
#else
        private static readonly Action<MemberMap, Serializer, valueType, CharStream> memberMapSerializer;
#endif
        /// <summary>
        /// 成员转换
        /// </summary>
        private static readonly Action<Serializer, valueType> memberSerializer;
        /// <summary>
        /// 转换委托
        /// </summary>
        private static readonly Action<Serializer, valueType> defaultSerializer;
        /// <summary>
        /// JSON序列化类型配置
        /// </summary>
        private static readonly SerializeAttribute attribute;
#if AutoCSer
        /// <summary>
        /// 客户端视图类型名称
        /// </summary>
        private static readonly string viewClientTypeName;
#endif
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void Serialize(Serializer jsonSerializer, ref valueType value)
        {
            if (isValueType) StructSerialize(jsonSerializer, ref value);
            else serialize(jsonSerializer, value);
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(Serializer jsonSerializer, valueType value)
        {
            Serialize(jsonSerializer, ref value);
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize(Serializer jsonSerializer, ref valueType value)
        {
            if (defaultSerializer == null) MemberSerialize(jsonSerializer, value);
            else defaultSerializer(jsonSerializer, value);
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize(Serializer jsonSerializer, valueType value)
        {
            StructSerialize(jsonSerializer, ref value);
        }
        /// <summary>
        /// 引用类型对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void serialize(Serializer jsonSerializer, valueType value)
        {
            if (value == null) jsonSerializer.CharStream.WriteJsonNull();
            else ClassSerialize(jsonSerializer, value);
        }
        /// <summary>
        /// 引用类型对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void ClassSerialize(Serializer jsonSerializer, valueType value)
        {
            if (defaultSerializer == null)
            {
                if (jsonSerializer.Push(value))
                {
                    MemberSerialize(jsonSerializer, value);
                    jsonSerializer.Pop();
                }
            }
            else defaultSerializer(jsonSerializer, value);
        }
        /// <summary>
        /// 值类型对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void MemberSerialize(Serializer jsonSerializer, valueType value)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            SerializeConfig config = jsonSerializer.Config;
#if AutoCSer
            byte isView;
            if (viewClientTypeName != null && config.IsViewClientType)
            {
                jsonStream.SimpleWriteNotNull(viewClientTypeName);
                isView = 1;
            }
            else
            {
#endif
            jsonStream.PrepLength(2);
            jsonStream.UnsafeWrite('{');
#if AutoCSer
                isView = 0;
            }
#endif
            MemberMap memberMap = config.MemberMap;
            if (memberMap == null) memberSerializer(jsonSerializer, value);
            else if (memberMap.Type == MemberMap<valueType>.TypeInfo)
            {
                config.MemberMap = null;
                try
                {
                    memberMapSerializer(memberMap, jsonSerializer, value, jsonStream);
                }
                finally { config.MemberMap = memberMap; }
            }
            else
            {
                jsonSerializer.Warning = SerializeWarning.MemberMap;
                if (config.IsMemberMapErrorToDefault) memberSerializer(jsonSerializer, value);
            }
#if AutoCSer
            if (isView == 0) jsonStream.Write('}');
            else
#endif
            {
                *(int*)jsonStream.GetPrepSizeCurrent(2) = '}' + (')' << 16);
                jsonStream.ByteSize += 2 * sizeof(char);
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="array">数组对象</param>
        internal static void Array(Serializer jsonSerializer, valueType[] array)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            jsonStream.Write('[');
            byte isFirst = 1;
            if (isValueType)
            {
                foreach (valueType value in array)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    StructSerialize(jsonSerializer, value);
                    isFirst = 0;
                }
            }
            else
            {
                foreach (valueType value in array)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    serialize(jsonSerializer, value);
                    isFirst = 0;
                }
            }
            jsonStream.Write(']');
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="values">枚举集合</param>
        internal static void Enumerable(Serializer jsonSerializer, IEnumerable<valueType> values)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            jsonStream.Write('[');
            byte isFirst = 1;
            if (isValueType)
            {
                foreach (valueType value in values)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    StructSerialize(jsonSerializer, value);
                    isFirst = 0;
                }
            }
            else
            {
                foreach (valueType value in values)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    serialize(jsonSerializer, value);
                    isFirst = 0;
                }
            }
            jsonStream.Write(']');
        }
        /// <summary>
        /// 字典转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void KeyValuePair<dictionaryValueType>(Serializer jsonSerializer, KeyValuePair<valueType, dictionaryValueType> value)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            byte* data = (byte*)jsonStream.GetPrepSizeCurrent(21);
            *(int*)data = '{' + ('"' << 16);
            *(int*)(data + sizeof(char) * 2) = 'K' + ('e' << 16);
            *(int*)(data + sizeof(char) * 4) = 'y' + ('"' << 16);
            *(char*)(data + sizeof(char) * 6) = ':';
            jsonStream.ByteSize += 7 * sizeof(char);
            TypeSerializer<valueType>.Serialize(jsonSerializer, value.Key);
            data = (byte*)jsonStream.GetPrepSizeCurrent(12);
            *(int*)data = ',' + ('"' << 16);
            *(int*)(data + sizeof(char) * 2) = 'V' + ('a' << 16);
            *(int*)(data + sizeof(char) * 4) = 'l' + ('u' << 16);
            *(int*)(data + sizeof(char) * 6) = 'e' + ('"' << 16);
            *(char*)(data + sizeof(char) * 8) = ':';
            jsonStream.ByteSize += 9 * sizeof(char);
            TypeSerializer<dictionaryValueType>.Serialize(jsonSerializer, value.Value);
            jsonStream.Write('}');
        }
        /// <summary>
        /// 字典转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="dictionary">数据对象</param>
        internal static void Dictionary<dictionaryValueType>(Serializer jsonSerializer, Dictionary<valueType, dictionaryValueType> dictionary)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            byte isFirst = 1;
            if (jsonSerializer.Config.IsDictionaryToObject)
            {
                jsonStream.Write('{');
                foreach (KeyValuePair<valueType, dictionaryValueType> value in dictionary)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    TypeSerializer<valueType>.Serialize(jsonSerializer, value.Key);
                    jsonStream.Write(':');
                    TypeSerializer<dictionaryValueType>.Serialize(jsonSerializer, value.Value);
                    isFirst = 0;
                }
                jsonStream.Write('}');
            }
            else
            {
                jsonStream.Write('[');
                foreach (KeyValuePair<valueType, dictionaryValueType> value in dictionary)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    KeyValuePair(jsonSerializer, value);
                    isFirst = 0;
                }
                jsonStream.Write(']');
            }
        }
        /// <summary>
        /// 字典转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="dictionary">字典</param>
        internal static void StringDictionary(Serializer jsonSerializer, Dictionary<string, valueType> dictionary)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            jsonStream.Write('{');
            byte isFirst = 1;
            if (isValueType)
            {
                foreach (KeyValuePair<string, valueType> value in dictionary)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    jsonStream.WriteJsonDictionaryKey(value.Key);
                    jsonStream.Write(':');
                    StructSerialize(jsonSerializer, value.Value);
                    isFirst = 0;
                }
            }
            else
            {
                foreach (KeyValuePair<string, valueType> value in dictionary)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    jsonStream.WriteJsonDictionaryKey(value.Key);
                    jsonStream.Write(':');
                    serialize(jsonSerializer, value.Value);
                    isFirst = 0;
                }
            }
            jsonStream.Write('}');
        }

        /// <summary>
        /// 不支持多维数组
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void arrayManyRank(Serializer jsonSerializer, valueType value)
        {
            jsonSerializer.CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 枚举转换字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void enumToString(Serializer jsonSerializer, valueType value)
        {
            jsonSerializer.EnumToString(value);
        }
        /// <summary>
        /// 不支持对象转换null
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void toNull(Serializer jsonSerializer, valueType value)
        {
            jsonSerializer.CharStream.WriteJsonNull();
        }

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
                //if (type.GetArrayRank() == 1) defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.GetArray(type.GetElementType()));
                if (type.GetArrayRank() == 1) defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), GenericType.Get(type.GetElementType()).JsonSerializeArrayMethod);
                else defaultSerializer = arrayManyRank;
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
                defaultSerializer = toNull;
                isValueType = true;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>))
                {
                    defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.GetDictionary(type));
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(Nullable<>))
                {
                    //defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.GetNullable(type));
                    defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), StructGenericType.Get(type.GetGenericArguments()[0]).JsonSerializeNullableMethod);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    //defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.GetKeyValuePair(type));
                    defaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), GenericType2.Get(type.GetGenericArguments()).JsonSerializeKeyValuePairMethod);
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
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomJsonSerializer", null, new Type[] { typeof(Serializer), type }, type, true);
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
                attribute = type.customAttribute<SerializeAttribute>(out attributeType) ?? (type.Name[0] == '<' ? SerializeAttribute.AnonymousTypeMember : Serializer.AllMemberAttribute);
                if (type.IsValueType) isValueType = true;
                else if (attribute != Serializer.AllMemberAttribute && attributeType != type)
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
                LeftArray<FieldIndex> fields = SerializeMethodCache.GetFields(MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), attribute);
                LeftArray<KeyValue<PropertyIndex, MethodInfo>> properties = SerializeMethodCache.GetProperties(MemberIndexGroup<valueType>.GetProperties(attribute.MemberFilters), attribute);
                bool isBox = false;
                if (type.IsValueType && fields.Length + properties.Length == 1)
                {
                    BoxSerializeAttribute boxSerialize = AutoCSer.Metadata.TypeAttribute.GetAttribute<BoxSerializeAttribute>(type);
                    if (boxSerialize != null && boxSerialize.IsJson) isBox = true;
                }
#if AutoCSer
                AutoCSer.WebView.ClientTypeAttribute clientType = isBox ? null : AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.WebView.ClientTypeAttribute>(type);
                if (clientType != null)
                {
                    if (clientType.MemberName == null) viewClientTypeName = "new " + clientType.GetClientName(type) + "({";
                    else viewClientTypeName = clientType.GetClientName(type) + ".Get({";
                }
#endif
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
                foreach (FieldIndex member in fields)
                {
                    if (isBox) dynamicMethod.PushBox(member);
                    else
                    {
                        dynamicMethod.Push(member);
                        memberMapDynamicMethod.Push(member);
                    }
                }
                foreach (KeyValue<PropertyIndex, MethodInfo> member in properties)
                {
                    if (isBox) dynamicMethod.PushBox(member.Key, member.Value);
                    else
                    {
                        dynamicMethod.Push(member.Key, member.Value);
                        memberMapDynamicMethod.Push(member.Key, member.Value);
                    }
                }
                memberSerializer = (Action<Serializer, valueType>)dynamicMethod.Create<Action<Serializer, valueType>>();
                if (isBox) defaultSerializer = memberSerializer;
                else memberMapSerializer = (Action<MemberMap, Serializer, valueType, CharStream>)memberMapDynamicMethod.Create<Action<MemberMap, Serializer, valueType, CharStream>>();
#endif
            }
        }
    }
}
