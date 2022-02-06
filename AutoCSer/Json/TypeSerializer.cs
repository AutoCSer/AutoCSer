using System;
using AutoCSer.Metadata;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    internal unsafe static partial class TypeSerializer<T>
    {
        /// <summary>
        /// 成员转换
        /// </summary>
#if NOJIT
        private static readonly Action<MemberMap, Serializer, object, CharStream> memberMapSerializer;
#else
        private static readonly Action<MemberMap, JsonSerializer, T, CharStream> memberMapSerializer;
#endif
        /// <summary>
        /// 成员转换
        /// </summary>
        private static readonly Action<JsonSerializer, T> memberSerializer;
        /// <summary>
        /// 转换委托
        /// </summary>
        private static readonly Action<JsonSerializer, T> defaultSerializer;
        /// <summary>
        /// JSON序列化类型配置
        /// </summary>
        private static readonly JsonSerializeAttribute attribute;
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
        internal static void Serialize(JsonSerializer jsonSerializer, ref T value)
        {
            if (isValueType) StructSerialize(jsonSerializer, ref value);
            else serialize(jsonSerializer, value);
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void Serialize(JsonSerializer jsonSerializer, T value)
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
        internal static void StructSerialize(JsonSerializer jsonSerializer, ref T value)
        {
            if (defaultSerializer == null) MemberSerialize(jsonSerializer, ref value);
            else defaultSerializer(jsonSerializer, value);
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize(JsonSerializer jsonSerializer, T value)
        {
            if (defaultSerializer == null) MemberSerialize(jsonSerializer, ref value);
            else defaultSerializer(jsonSerializer, value);
        }
        /// <summary>
        /// 引用类型对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void serialize(JsonSerializer jsonSerializer, T value)
        {
            if (value == null) jsonSerializer.CharStream.WriteJsonNull();
            else ClassSerialize(jsonSerializer, value);
        }
        /// <summary>
        /// 引用类型对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void ClassSerialize(JsonSerializer jsonSerializer, T value)
        {
            if (defaultSerializer == null)
            {
                if (jsonSerializer.Push(value))
                {
                    MemberSerialize(jsonSerializer, ref value);
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
        internal static void MemberSerialize(JsonSerializer jsonSerializer, ref T value)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            SerializeConfig config = jsonSerializer.Config;
#if AutoCSer
            byte isView;
            if (viewClientTypeName != null && config.IsViewClientType)
            {
                jsonStream.SimpleWrite(viewClientTypeName);
                isView = 1;
            }
            else
            {
#endif
            jsonStream.PrepCharSize(2);
            jsonStream.Data.Write('{');
#if AutoCSer
                isView = 0;
            }
#endif
            MemberMap memberMap = config.MemberMap;
            if (memberMap == null) memberSerializer(jsonSerializer, value);
            else if (object.ReferenceEquals(memberMap.Type, MemberMap<T>.MemberMapType))
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
                *(int*)jsonStream.GetBeforeMove(2 * sizeof(char)) = '}' + (')' << 16);
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="array">数组对象</param>
        internal static void Array(JsonSerializer jsonSerializer, T[] array)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            jsonStream.Write('[');
            byte isFirst = 1;
            foreach (T value in array)
            {
                if (isFirst == 0) jsonStream.Write(',');
                else isFirst = 0;
                serialize(jsonSerializer, value);
            }
            jsonStream.Write(']');
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="array">数组对象</param>
        internal static void StructArray(JsonSerializer jsonSerializer, T[] array)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            jsonStream.Write('[');
            for (int index = 0; index != array.Length; ++index)
            {
                if (index != 0) jsonStream.Write(',');
                StructSerialize(jsonSerializer, ref array[index]);
            }
            jsonStream.Write(']');
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="values">枚举集合</param>
        internal static void Enumerable(JsonSerializer jsonSerializer, IEnumerable<T> values)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            jsonStream.Write('[');
            byte isFirst = 1;
            if (isValueType)
            {
                foreach (T value in values)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    StructSerialize(jsonSerializer, value);
                    isFirst = 0;
                }
            }
            else
            {
                foreach (T value in values)
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
        internal static void KeyValuePair<dictionaryValueType>(JsonSerializer jsonSerializer, KeyValuePair<T, dictionaryValueType> value)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            jsonStream.WriteJsonKeyValueKey(21);
            TypeSerializer<T>.Serialize(jsonSerializer, value.Key);
            jsonStream.WriteJsonKeyValueValue();
            TypeSerializer<dictionaryValueType>.Serialize(jsonSerializer, value.Value);
            jsonStream.Write('}');
        }
        /// <summary>
        /// 字典转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="dictionary">数据对象</param>
        internal static void Dictionary<dictionaryValueType>(JsonSerializer jsonSerializer, Dictionary<T, dictionaryValueType> dictionary)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            byte isFirst = 1;
            if (jsonSerializer.Config.IsDictionaryToObject)
            {
                jsonStream.Write('{');
                foreach (KeyValuePair<T, dictionaryValueType> value in dictionary)
                {
                    if (isFirst == 0) jsonStream.Write(',');
                    TypeSerializer<T>.Serialize(jsonSerializer, value.Key);
                    jsonStream.Write(':');
                    TypeSerializer<dictionaryValueType>.Serialize(jsonSerializer, value.Value);
                    isFirst = 0;
                }
                jsonStream.Write('}');
            }
            else
            {
                jsonStream.Write('[');
                foreach (KeyValuePair<T, dictionaryValueType> value in dictionary)
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
        internal static void StringDictionary(JsonSerializer jsonSerializer, Dictionary<string, T> dictionary)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            jsonStream.Write('{');
            byte isFirst = 1;
            if (isValueType)
            {
                foreach (KeyValuePair<string, T> value in dictionary)
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
                foreach (KeyValuePair<string, T> value in dictionary)
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
        /// 枚举转换字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void EnumToString(JsonSerializer jsonSerializer, T value)
        {
            string stringValue = value.ToString();
            char charValue = stringValue[0];
            if ((uint)(charValue - '1') < 9 || charValue == '-') jsonSerializer.CharStream.SimpleWrite(stringValue);
            else jsonSerializer.CharStream.WriteQuote(stringValue);
        }

        static TypeSerializer()
        {
            Type type = typeof(T);
            MethodInfo methodInfo = JsonSerializer.GetSerializeMethod(type);
            if (methodInfo != null)
            {
                defaultSerializer = (Action<JsonSerializer, T>)Delegate.CreateDelegate(typeof(Action<JsonSerializer, T>), methodInfo);
                isValueType = true;
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType();
                    if (elementType.IsValueType && (!elementType.IsGenericType || elementType.GetGenericTypeDefinition() != typeof(Nullable<>)))
                    {
                        defaultSerializer = (Action<JsonSerializer, T>)StructGenericType.Get(elementType).JsonSerializeStructArrayMethod;
                    }
                    else defaultSerializer = (Action<JsonSerializer, T>)GenericType.Get(elementType).JsonSerializeArrayMethod;
                }
                else defaultSerializer = (Action<JsonSerializer, T>)GenericType.Get(type).JsonSerializeNotSupportDelegate;
                isValueType = true;
                return;
            }
            if (type.IsEnum)
            {
                defaultSerializer = EnumToString;
                isValueType = true;
                return;
            }
            if (type.isSerializeNotSupport())
            {
                defaultSerializer = (Action<JsonSerializer, T>)GenericType.Get(type).JsonSerializeNotSupportDelegate;
                isValueType = true;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>))
                {
                    defaultSerializer = (Action<JsonSerializer, T>)SerializeMethodCache.GetDictionary(type);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(Nullable<>))
                {
                    defaultSerializer = (Action<JsonSerializer, T>)StructGenericType.Get(type.GetGenericArguments()[0]).JsonSerializeNullableMethod;
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    defaultSerializer = (Action<JsonSerializer, T>)GenericType2.Get(type.GetGenericArguments()).JsonSerializeKeyValuePairMethod;
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
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomJsonSerializer", null, new Type[] { typeof(JsonSerializer), type }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarga_S, 1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    defaultSerializer = (Action<JsonSerializer, T>)dynamicMethod.CreateDelegate(typeof(Action<JsonSerializer, T>));
#endif
                }
                else defaultSerializer = (Action<JsonSerializer, T>)Delegate.CreateDelegate(typeof(Action<JsonSerializer, T>), methodInfo);
                isValueType = true;
            }
            else
            {
                Delegate enumerableDelegate = SerializeMethodCache.GetIEnumerable(type);
                if (enumerableDelegate != null)
                {
                    defaultSerializer = (Action<JsonSerializer, T>)enumerableDelegate;
                    isValueType = true;
                }
                else
                {
                    Type attributeType;
                    attribute = type.customAttribute<JsonSerializeAttribute>(out attributeType) ?? (type.Name[0] == '<' ? JsonSerializeAttribute.AnonymousTypeMember : JsonSerializer.AllMemberAttribute);
                    if (type.IsValueType) isValueType = true;
                    else if (attribute != JsonSerializer.AllMemberAttribute && attributeType != type)
                    {
                        for (Type baseType = type.BaseType; baseType != typeof(object); baseType = baseType.BaseType)
                        {
                            JsonSerializeAttribute baseAttribute = baseType.customAttribute<JsonSerializeAttribute>();
                            if (baseAttribute != null)
                            {
                                if (baseAttribute.IsBaseType)
                                {
                                    methodInfo = SerializeMethodCache.BaseSerializeMethod.MakeGenericMethod(baseType, type);
                                    defaultSerializer = (Action<JsonSerializer, T>)Delegate.CreateDelegate(typeof(Action<JsonSerializer, T>), methodInfo);
                                    return;
                                }
                                break;
                            }
                        }
                    }
                    LeftArray<FieldIndex> fields = SerializeMethodCache.GetFields(MemberIndexGroup<T>.GetFields(attribute.MemberFilters), attribute);
                    LeftArray<KeyValue<PropertyIndex, MethodInfo>> properties = SerializeMethodCache.GetProperties(MemberIndexGroup<T>.GetProperties(attribute.MemberFilters), attribute);
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
                    memberSerializer = (Action<JsonSerializer, T>)dynamicMethod.Create<Action<JsonSerializer, T>>();
                    if (isBox) defaultSerializer = memberSerializer;
                    else memberMapSerializer = (Action<MemberMap, JsonSerializer, T, CharStream>)memberMapDynamicMethod.Create<Action<MemberMap, JsonSerializer, T, CharStream>>();
#endif
                }
            }
        }
    }
}
