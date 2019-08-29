using System;
using AutoCSer.Metadata;
using System.Reflection;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    internal unsafe static partial class TypeSerializer<valueType>
    {
        /// <summary>
        /// 二进制数据序列化类型配置
        /// </summary>
        private static readonly SerializeAttribute attribute;
        /// <summary>
        /// 序列化委托
        /// </summary>
        internal static readonly Action<Serializer, valueType> DefaultSerializer;
#if NOJIT
        /// <summary>
        /// 固定分组成员序列化
        /// </summary>
        private static readonly Action<Serializer, object> fixedMemberSerializer;
        /// <summary>
        /// 固定分组成员位图序列化
        /// </summary>
        private static readonly Action<MemberMap, Serializer, object> fixedMemberMapSerializer;
        /// <summary>
        /// 成员序列化
        /// </summary>
        private static readonly Action<Serializer, object> memberSerializer;
        /// <summary>
        /// 成员位图序列化
        /// </summary>
        private static readonly Action<MemberMap, Serializer, object> memberMapSerializer;
#else
        /// <summary>
        /// 固定分组成员序列化
        /// </summary>
        private static readonly Action<Serializer, valueType> fixedMemberSerializer;
        /// <summary>
        /// 固定分组成员位图序列化
        /// </summary>
        private static readonly Action<MemberMap, Serializer, valueType> fixedMemberMapSerializer;
        /// <summary>
        /// 成员序列化
        /// </summary>
        private static readonly Action<Serializer, valueType> memberSerializer;
        /// <summary>
        /// 成员位图序列化
        /// </summary>
        private static readonly Action<MemberMap, Serializer, valueType> memberMapSerializer;
#endif
        /// <summary>
        /// JSON混合序列化位图
        /// </summary>
        private static readonly MemberMap jsonMemberMap;
        /// <summary>
        /// JSON混合序列化成员索引集合
        /// </summary>
        private static readonly int[] jsonMemberIndexs;
        /// <summary>
        /// 序列化成员数量
        /// </summary>
        private static readonly int memberCountVerify;
        /// <summary>
        /// 固定分组字节数
        /// </summary>
        private static readonly int fixedSize;
        /// <summary>
        /// 固定分组填充字节数
        /// </summary>
        private static readonly int fixedFillSize;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;
        /// <summary>
        /// 是否支持循环引用处理
        /// </summary>
        internal static readonly bool IsReferenceMember;
        /// <summary>
        /// 是否处理成员位图
        /// </summary>
        private static readonly bool isMemberMap;
        /// <summary>
        /// 当没有 JSON 序列化成员时是否预留 JSON 序列化标记
        /// </summary>
        private static readonly bool isJson;

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serialize(Serializer serializer, valueType value)
        {
            if (isValueType) StructSerialize(serializer, value);
            else ClassSerialize(serializer, value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize(Serializer serializer, valueType value)
        {
            if (DefaultSerializer == null) MemberSerialize(serializer, value);
            else DefaultSerializer(serializer, value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        internal static void ClassSerialize(Serializer serializer, valueType value)
        {
            if (DefaultSerializer == null)
            {
                if (serializer.CheckPoint(value))
                {
                    if (serializer.Config.IsRealType)
                    {
                        Type type = value.GetType();
                        if (type != typeof(valueType))
                        {
                            if (serializer.CheckPoint(value))
                            {
                                serializer.Stream.Write(Serializer.RealTypeValue);
                                //SerializeMethodCache.GetRealSerializer(type)(serializer, value);
                                GenericType.Get(type).BinarySerializeRealTypeObjectDelegate(serializer, value);
                            }
                            return;
                        }
                    }
                    if (Emit.Constructor<valueType>.New == null) serializer.Stream.Write(Serializer.NullValue);
                    else MemberSerialize(serializer, value);
                }
            }
            else DefaultSerializer(serializer, value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        internal static void MemberSerialize(Serializer serializer, valueType value)
        {
            MemberMap memberMap = isMemberMap ? serializer.SerializeMemberMap<valueType>() : null;
            UnmanagedStream stream = serializer.Stream;
            if (memberMap == null)
            {
                stream.PrepLength(fixedSize);
                stream.UnsafeWrite(memberCountVerify);
                fixedMemberSerializer(serializer, value);
                stream.SerializeFill(fixedFillSize);
                //stream.PrepLength();
                if (memberSerializer != null) memberSerializer(serializer, value);
                if (jsonMemberMap == null)
                {
                    if (isJson) stream.Write(0);
                }
                else AutoCSer.Json.Serializer.Serialize(value, stream, serializer.GetJsonConfig(jsonMemberMap));
            }
            else
            {
                stream.PrepLength(fixedSize - sizeof(int));
                //int length = stream.OffsetLength;
                int startIndex = stream.ByteSize;
                fixedMemberMapSerializer(memberMap, serializer, value);
                //if (serializer.Config.IsFill) stream.SerializeFill((length - stream.OffsetLength) & 3);
                //else stream.ByteSize += (length - stream.OffsetLength) & 3;
                stream.SerializeFillWithStartIndex(startIndex);
                //stream.PrepLength();
                if (memberMapSerializer != null) memberMapSerializer(memberMap, serializer, value);
                if (jsonMemberMap == null || (memberMap = serializer.GetJsonMemberMap<valueType>(memberMap, jsonMemberIndexs)) == null)
                {
                    if (isJson) stream.Write(0);
                }
                else AutoCSer.Json.Serializer.Serialize(value, stream, serializer.GetJsonConfig(memberMap));
            }
        }
        /// <summary>
        /// 真实类型序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        internal static void RealTypeObject(Serializer serializer, object value)
        {
            if (isValueType)
            {
                TypeSerializer<RemoteType>.StructSerialize(serializer, typeof(valueType));
                StructSerialize(serializer, (valueType)value);
            }
            else
            {
                if (Emit.Constructor<valueType>.New == null) serializer.Stream.Write(Serializer.NullValue);
                else
                {
                    TypeSerializer<RemoteType>.StructSerialize(serializer, typeof(valueType));
                    if (DefaultSerializer == null)
                    {
                        //if (serializer.CheckPoint(value)) MemberSerialize(serializer, (valueType)value);
                        MemberSerialize(serializer, (valueType)value);
                    }
                    else DefaultSerializer(serializer, (valueType)value);
                }
            }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void BaseSerialize<childType>(Serializer serializer, childType value) where childType : valueType
        {
            if (serializer.CheckPoint(value)) StructSerialize(serializer, value);
        }

        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumByte(Serializer serializer, valueType value)
        {
            serializer.Stream.Write((uint)Emit.EnumCast<valueType, byte>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumSByte(Serializer serializer, valueType value)
        {
            serializer.Stream.Write((int)Emit.EnumCast<valueType, sbyte>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumShort(Serializer serializer, valueType value)
        {
            serializer.Stream.Write((int)Emit.EnumCast<valueType, short>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumUShort(Serializer serializer, valueType value)
        {
            serializer.Stream.Write((uint)Emit.EnumCast<valueType, ushort>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumInt(Serializer serializer, valueType value)
        {
            serializer.Stream.Write(Emit.EnumCast<valueType, int>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumUInt(Serializer serializer, valueType value)
        {
            serializer.Stream.Write(Emit.EnumCast<valueType, uint>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumLong(Serializer serializer, valueType value)
        {
            serializer.Stream.Write(Emit.EnumCast<valueType, long>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumULong(Serializer serializer, valueType value)
        {
            serializer.Stream.Write(Emit.EnumCast<valueType, ulong>.ToInt(value));
        }
        /// <summary>
        /// 不支持对象转换null
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void toNull(Serializer serializer, valueType value)
        {
            serializer.Stream.Write(Serializer.NullValue);
        }
        /// <summary>
        /// 找不到构造函数
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        private static void noConstructor(Serializer serializer, valueType value)
        {
            if (serializer.CheckPoint(value))
            {
                if (serializer.Config.IsRealType)
                {
                    Type type = value.GetType();
                    if (type == typeof(valueType)) serializer.Stream.Write(Serializer.NullValue);
                    //else SerializeMethodCache.GetRealSerializer(type)(serializer, value);
                    else GenericType.Get(type).BinarySerializeRealTypeObjectDelegate(serializer, value);
                }
                else serializer.Stream.Write(Serializer.NullValue);
            }
        }

        static TypeSerializer()
        {
            Type type = typeof(valueType), attributeType;
            MethodInfo methodInfo = Serializer.GetSerializeMethod(type);
            attribute = type.customAttribute<SerializeAttribute>(out attributeType) ?? Serializer.DefaultAttribute;
            if (methodInfo != null)
            {
                DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                isValueType = true;
                IsReferenceMember = false;
                return;
            }
            if (type.IsArray)
            {
                isValueType = true;
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType();
                    if (!elementType.IsPointer && !typeof(Delegate).IsAssignableFrom(elementType))
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                Type enumType = System.Enum.GetUnderlyingType(elementType);
                                if (enumType == typeof(uint)) methodInfo = GenericType.Get(elementType).BinarySerializeEnumUIntArrayMethod;// SerializeMethodCache.EnumUIntArrayMethod;
                                else if (enumType == typeof(byte)) methodInfo = GenericType.Get(elementType).BinarySerializeEnumByteArrayMethod;//SerializeMethodCache.EnumByteArrayMethod;
                                else if (enumType == typeof(ulong)) methodInfo = GenericType.Get(elementType).BinarySerializeEnumULongArrayMethod;//SerializeMethodCache.EnumULongArrayMethod;
                                else if (enumType == typeof(ushort)) methodInfo = GenericType.Get(elementType).BinarySerializeEnumUShortArrayMethod;//SerializeMethodCache.EnumUShortArrayMethod;
                                else if (enumType == typeof(long)) methodInfo = GenericType.Get(elementType).BinarySerializeEnumLongArrayMethod;//SerializeMethodCache.EnumLongArrayMethod;
                                else if (enumType == typeof(short)) methodInfo = GenericType.Get(elementType).BinarySerializeEnumShortArrayMethod;//SerializeMethodCache.EnumShortArrayMethod;
                                else if (enumType == typeof(sbyte)) methodInfo = GenericType.Get(elementType).BinarySerializeEnumSByteArrayMethod;//SerializeMethodCache.EnumSByteArrayMethod;
                                else methodInfo = GenericType.Get(elementType).BinarySerializeEnumIntArrayMethod;//SerializeMethodCache.EnumIntArrayMethod;
                                //methodInfo = methodInfo.MakeGenericMethod(elementType);
                                IsReferenceMember = false;
                            }
                            else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                //methodInfo = SerializeMethodCache.NullableArrayMethod.MakeGenericMethod(elementType = elementType.GetGenericArguments()[0]);
                                methodInfo = StructGenericType.Get(elementType = elementType.GetGenericArguments()[0]).BinarySerializeNullableArrayMethod;
                                IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                            }
                            else
                            {
                                //methodInfo = SerializeMethodCache.StructArrayMethod.MakeGenericMethod(elementType);
                                methodInfo = GenericType.Get(elementType).BinarySerializeStructArrayMethod;
                                IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                            }
                        }
                        else
                        {
                            //methodInfo = SerializeMethodCache.ArrayMethod.MakeGenericMethod(elementType);
                            methodInfo = ClassGenericType.Get(elementType).BinarySerializeArrayMethod;
                            IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                        }
                        DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                        return;
                    }
                }
                DefaultSerializer = toNull;
                IsReferenceMember = false;
                return;
            }
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(uint)) DefaultSerializer = enumUInt;
                else if (enumType == typeof(byte)) DefaultSerializer = enumByte;
                else if (enumType == typeof(ulong)) DefaultSerializer = enumULong;
                else if (enumType == typeof(ushort)) DefaultSerializer = enumUShort;
                else if (enumType == typeof(long)) DefaultSerializer = enumLong;
                else if (enumType == typeof(short)) DefaultSerializer = enumShort;
                else if (enumType == typeof(sbyte)) DefaultSerializer = enumSByte;
                else DefaultSerializer = enumInt;
                isValueType = true;
                IsReferenceMember = false;
                return;
            }
            if (type.IsPointer || typeof(Delegate).IsAssignableFrom(type))
            {
                DefaultSerializer = toNull;
                IsReferenceMember = false;
                isValueType = true;
                return;
            }

            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                Type[] parameterTypes = type.GetGenericArguments();
                if (genericType == typeof(LeftArray<>))
                {
                    //DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.LeftArraySerializeMethod.MakeGenericMethod(parameterTypes));
                    DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), GenericType.Get(parameterTypes[0]).BinarySerializeLeftArrayMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
#if !Serialize
                if (genericType == typeof(SubArray<>))
                {
                    //DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.SubArraySerializeMethod.MakeGenericMethod(parameterTypes));
                    DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), GenericType.Get(parameterTypes[0]).BinarySerializeSubArrayMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
#endif
                if (genericType == typeof(Dictionary<,>) || genericType == typeof(SortedDictionary<,>) || genericType == typeof(SortedList<,>))
                {
                    //DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.DictionarySerializeMethod.MakeGenericMethod(type, parameterTypes[0], parameterTypes[1]));
                    DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), DictionaryGenericType3.Get(type, parameterTypes[0], parameterTypes[1]).BinarySerializeDictionaryMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]) || SerializeMethodCache.IsReferenceMember(parameterTypes[1]);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(Nullable<>))
                {
                    //DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.NullableSerializeMethod.MakeGenericMethod(parameterTypes));
                    DefaultSerializer = (Action<Serializer, valueType>)StructGenericType.Get(parameterTypes[0]).BinarySerializeNullableDelegate;
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    //DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), SerializeMethodCache.KeyValuePairSerializeMethod.MakeGenericMethod(parameterTypes));
                    DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), GenericType2.Get(parameterTypes).BinarySerializeKeyValuePairMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]) || SerializeMethodCache.IsReferenceMember(parameterTypes[1]);
                    isValueType = true;
                    return;
                }
            }
            if ((methodInfo = SerializeMethodCache.GetCustom(type, true)) != null)
            {
                if (type.IsValueType)
                {
#if NOJIT
                    DefaultSerializer = new CustomSerializer(methodInfo).Serialize;
#else
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomSerializer", null, new Type[] { typeof(Serializer), type }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarga_S, 1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    DefaultSerializer = (Action<Serializer, valueType>)dynamicMethod.CreateDelegate(typeof(Action<Serializer, valueType>));
#endif
                }
                else DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                IsReferenceMember = attribute.IsReferenceMember;
                isValueType = true;
                return;
            }
            if (type.IsInterface || type.IsAbstract || Emit.Constructor<valueType>.New == null)
            {
                DefaultSerializer = noConstructor;
                isValueType = IsReferenceMember = true;
                return;
            }
            ConstructorInfo constructorInfo = null;
            Type argumentType = null;
            IsReferenceMember = attribute.IsReferenceMember;
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType)
                {
                    Type genericType = interfaceType.GetGenericTypeDefinition();
                    if (genericType == typeof(ICollection<>))
                    {
                        Type[] parameterTypes = interfaceType.GetGenericArguments();
                        argumentType = parameterTypes[0];
                        parameterTypes[0] = argumentType.MakeArrayType();
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null);
                        if (constructorInfo != null) break;
                        parameterTypes[0] = typeof(IList<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null);
                        if (constructorInfo != null) break;
                        parameterTypes[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null);
                        if (constructorInfo != null) break;
                        parameterTypes[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null);
                        if (constructorInfo != null) break;
                    }
                    else if (genericType == typeof(IDictionary<,>))
                    {
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { interfaceType }, null);
                        if (constructorInfo != null)
                        {
                            Type[] parameters = interfaceType.GetGenericArguments();
                            //methodInfo = (type.IsValueType ? SerializeMethodCache.StructDictionaryMethod : SerializeMethodCache.ClassDictionaryMethod).MakeGenericMethod(type, parameters[0], parameters[1]);
                            if (type.IsValueType) methodInfo = DictionaryGenericType3.Get(type, parameters[0], parameters[1]).BinarySerializeStructDictionaryMethod;
                            else methodInfo = DictionaryGenericType3.Get(type, parameters[0], parameters[1]).BinarySerializeClassDictionaryMethod;
                            DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                            return;
                        }
                    }
                }
            }
            if (constructorInfo != null)
            {
                if (argumentType.IsValueType && argumentType.IsEnum)
                {
                    Type enumType = System.Enum.GetUnderlyingType(argumentType);
                    //if (enumType == typeof(uint)) methodInfo = type.IsValueType ? SerializeMethodCache.StructEnumUIntCollectionMethod : SerializeMethodCache.ClassEnumUIntCollectionMethod;
                    //else if (enumType == typeof(byte)) methodInfo = type.IsValueType ? SerializeMethodCache.StructEnumByteCollectionMethod : SerializeMethodCache.ClassEnumByteCollectionMethod;
                    //else if (enumType == typeof(ulong)) methodInfo = type.IsValueType ? SerializeMethodCache.StructEnumULongCollectionMethod : SerializeMethodCache.ClassEnumULongCollectionMethod;
                    //else if (enumType == typeof(ushort)) methodInfo = type.IsValueType ? SerializeMethodCache.StructEnumUShortCollectionMethod : SerializeMethodCache.ClassEnumUShortCollectionMethod;
                    //else if (enumType == typeof(long)) methodInfo = type.IsValueType ? SerializeMethodCache.StructEnumLongCollectionMethod : SerializeMethodCache.ClassEnumLongCollectionMethod;
                    //else if (enumType == typeof(short)) methodInfo = type.IsValueType ? SerializeMethodCache.StructEnumShortCollectionMethod : SerializeMethodCache.ClassEnumShortCollectionMethod;
                    //else if (enumType == typeof(sbyte)) methodInfo = type.IsValueType ? SerializeMethodCache.StructEnumSByteCollectionMethod : SerializeMethodCache.ClassEnumSByteCollectionMethod;
                    //else methodInfo = type.IsValueType ? SerializeMethodCache.StructEnumIntCollectionMethod : SerializeMethodCache.ClassEnumIntCollectionMethod;
                    //methodInfo = methodInfo.MakeGenericMethod(argumentType, type);
                    if (enumType == typeof(uint)) methodInfo = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumUIntCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumUIntCollectionMethod;
                    else if (enumType == typeof(byte)) methodInfo = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumByteCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumByteCollectionMethod;
                    else if (enumType == typeof(ulong)) methodInfo = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumULongCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumULongCollectionMethod;
                    else if (enumType == typeof(ushort)) methodInfo = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumUShortCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumUShortCollectionMethod;
                    else if (enumType == typeof(long)) methodInfo = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumLongCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumLongCollectionMethod;
                    else if (enumType == typeof(short)) methodInfo = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumShortCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumShortCollectionMethod;
                    else if (enumType == typeof(sbyte)) methodInfo = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumSByteCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumSByteCollectionMethod;
                    else methodInfo = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumIntCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumIntCollectionMethod;
                }
                //else methodInfo = (type.IsValueType ? SerializeMethodCache.StructCollectionMethod : SerializeMethodCache.ClassCollectionMethod).MakeGenericMethod(argumentType, type);
                else
                {
                    if (type.IsValueType) methodInfo = CollectionGenericType2.Get(type, argumentType).BinarySerializeStructCollectionMethod;
                    else methodInfo = CollectionGenericType2.Get(type, argumentType).BinarySerializeClassCollectionMethod;
                }
                DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                return;
            }
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
                            DefaultSerializer = (Action<Serializer, valueType>)Delegate.CreateDelegate(typeof(Action<Serializer, valueType>), methodInfo);
                            return;
                        }
                        break;
                    }
                }
            }
            isJson = attribute.GetIsJson;
            isMemberMap = attribute.GetIsMemberMap;
            Fields<FieldSize> fields = SerializeMethodCache.GetFields(attribute.IsAnonymousFields ? MemberIndexGroup<valueType>.GetAnonymousFields(attribute.MemberFilters) : MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), isJson, out memberCountVerify);
            //if (!type.IsValueType && (fields.FixedFields.length | fields.Fields.length | fields.JsonFields.length) == 0)
            //{
            //    DefaultSerializer = noMember;
            //    isValueType = true;
            //    IsReferenceMember = false;
            //    return;
            //}
            fixedFillSize = -fields.FixedSize & 3;
            fixedSize = (fields.FixedSize + (sizeof(int) + 3)) & (int.MaxValue - 3);
#if NOJIT
            fixedMemberSerializer = new FieldFerializer(ref fields.FixedFields).Serialize;
            if (isMemberMap) fixedMemberMapSerializer = new MemberMapSerializer(ref fields.FixedFields).Serialize;
            if (fields.FieldArray.Length != 0)
            {
                memberSerializer = new FieldFerializer(ref fields.FieldArray).Serialize;
                if (isMemberMap) memberMapSerializer = new MemberMapSerializer(ref fields.FieldArray).Serialize;
            }
#else
            SerializeMemberDynamicMethod fixedDynamicMethod = new SerializeMemberDynamicMethod(type);
            SerializeMemberMapDynamicMethod fixedMemberMapDynamicMethod = isMemberMap ? new SerializeMemberMapDynamicMethod(type) : default(SerializeMemberMapDynamicMethod);
            foreach (FieldSize member in fields.FixedFields)
            {
                fixedDynamicMethod.Push(member);
                if (isMemberMap) fixedMemberMapDynamicMethod.Push(member);
            }
            fixedMemberSerializer = (Action<Serializer, valueType>)fixedDynamicMethod.Create<Action<Serializer, valueType>>();
            if (isMemberMap) fixedMemberMapSerializer = (Action<MemberMap, Serializer, valueType>)fixedMemberMapDynamicMethod.Create<Action<MemberMap, Serializer, valueType>>();

            if (fields.FieldArray.Length != 0)
            {
                SerializeMemberDynamicMethod dynamicMethod = new SerializeMemberDynamicMethod(type);
                SerializeMemberMapDynamicMethod memberMapDynamicMethod = isMemberMap ? new SerializeMemberMapDynamicMethod(type) : default(SerializeMemberMapDynamicMethod);
                foreach (FieldSize member in fields.FieldArray)
                {
                    dynamicMethod.Push(member);
                    if (isMemberMap) memberMapDynamicMethod.Push(member);
                }
                memberSerializer = (Action<Serializer, valueType>)dynamicMethod.Create<Action<Serializer, valueType>>();
                if (isMemberMap) memberMapSerializer = (Action<MemberMap, Serializer, valueType>)memberMapDynamicMethod.Create<Action<MemberMap, Serializer, valueType>>();
            }
#endif
            if (fields.JsonFields.Length != 0)
            {
                jsonMemberMap = new MemberMap<valueType>();
                jsonMemberIndexs = new int[fields.JsonFields.Length];
                int index = 0;
                foreach (FieldIndex field in fields.JsonFields) jsonMemberMap.SetMember(jsonMemberIndexs[index++] = field.MemberIndex);
            }
        }
    }
}
