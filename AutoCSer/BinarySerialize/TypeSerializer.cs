using System;
using AutoCSer.Metadata;
using System.Reflection;
using AutoCSer.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    internal unsafe static partial class TypeSerializer<T>
    {
        /// <summary>
        /// 二进制数据序列化类型配置
        /// </summary>
        private static readonly BinarySerializeAttribute attribute;
        /// <summary>
        /// 序列化委托
        /// </summary>
        internal static readonly Action<BinarySerializer, T> DefaultSerializer;
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
        private static readonly Action<BinarySerializer, T> fixedMemberSerializer;
        /// <summary>
        /// 固定分组成员位图序列化
        /// </summary>
        private static readonly Action<MemberMap, BinarySerializer, T> fixedMemberMapSerializer;
        /// <summary>
        /// 成员序列化
        /// </summary>
        private static readonly Action<BinarySerializer, T> memberSerializer;
        /// <summary>
        /// 成员位图序列化
        /// </summary>
        private static readonly Action<MemberMap, BinarySerializer, T> memberMapSerializer;
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
        internal static void Serialize(BinarySerializer serializer, ref T value)
        {
            if (isValueType) StructSerialize(serializer, ref value);
            else ClassSerialize(serializer, value);
        }
        ///// <summary>
        ///// 对象序列化
        ///// </summary>
        ///// <param name="serializer">二进制数据序列化</param>
        ///// <param name="value">数据对象</param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal static void Serialize(Serializer serializer, valueType value)
        //{
        //    if (isValueType) StructSerialize(serializer, ref value);
        //    else ClassSerialize(serializer, value);
        //}
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize(BinarySerializer serializer, T value)
        {
            if (DefaultSerializer == null) MemberSerialize(serializer, ref value);
            else DefaultSerializer(serializer, value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize(BinarySerializer serializer, ref T value)
        {
            if (DefaultSerializer == null) MemberSerialize(serializer, ref value);
            else DefaultSerializer(serializer, value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        internal static void ClassSerialize(BinarySerializer serializer, T value)
        {
            if (DefaultSerializer == null)
            {
                if (serializer.CheckPoint(value))
                {
                    if (serializer.Config.IsRealType)
                    {
                        Type type = value.GetType();
                        if (type != typeof(T))
                        {
                            //if (serializer.CheckPoint(value))
                            {
                                serializer.Stream.Write(BinarySerializer.RealTypeValue);
                                //SerializeMethodCache.GetRealSerializer(type)(serializer, value);
                                GenericType.Get(type).BinarySerializeRealTypeObjectDelegate(serializer, value);
                            }
                            return;
                        }
                    }
                    if (AutoCSer.Metadata.DefaultConstructor<T>.Type == DefaultConstructorType.None) serializer.Stream.Write(BinarySerializer.NullValue);
                    else MemberSerialize(serializer, ref value);
                }
            }
            else DefaultSerializer(serializer, value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        internal static void MemberSerialize(BinarySerializer serializer, ref T value)
        {
            MemberMap memberMap = isMemberMap ? serializer.SerializeMemberMap<T>() : null;
            UnmanagedStream stream = serializer.Stream;
            if (memberMap == null)
            {
                stream.PrepSize(fixedSize);
                stream.Data.Write(memberCountVerify);
                fixedMemberSerializer(serializer, value);
                stream.Data.SerializeFill(fixedFillSize);
                //stream.PrepLength();
                if (memberSerializer != null) memberSerializer(serializer, value);
                if (jsonMemberMap == null)
                {
                    if (isJson) stream.Write(0);
                }
                else AutoCSer.JsonSerializer.Serialize(ref value, stream, serializer.GetJsonConfig(jsonMemberMap));
            }
            else
            {
                stream.PrepSize(fixedSize - sizeof(int));
                //int length = stream.OffsetLength;
                int startIndex = stream.Data.CurrentIndex;
                fixedMemberMapSerializer(memberMap, serializer, value);
                //if (serializer.Config.IsFill) stream.SerializeFill((length - stream.OffsetLength) & 3);
                //else stream.ByteSize += (length - stream.OffsetLength) & 3;
                stream.Data.SerializeFillWithStartIndex(startIndex);
                //stream.PrepLength();
                if (memberMapSerializer != null) memberMapSerializer(memberMap, serializer, value);
                if (jsonMemberMap == null || (memberMap = serializer.GetJsonMemberMap<T>(memberMap, jsonMemberIndexs)) == null)
                {
                    if (isJson) stream.Write(0);
                }
                else AutoCSer.JsonSerializer.Serialize(ref value, stream, serializer.GetJsonConfig(memberMap));
            }
        }
        /// <summary>
        /// 真实类型序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        internal static void RealTypeObject(BinarySerializer serializer, object value)
        {
            if (isValueType)
            {
                AutoCSer.Reflection.RemoteType remoteType = typeof(T);
                TypeSerializer<AutoCSer.Reflection.RemoteType>.MemberSerialize(serializer, ref remoteType);
                StructSerialize(serializer, (T)value);
            }
            else
            {
                if (AutoCSer.Metadata.DefaultConstructor<T>.Type == DefaultConstructorType.None) serializer.Stream.Write(BinarySerializer.NullValue);
                else
                {
                    AutoCSer.Reflection.RemoteType remoteType = typeof(T);
                    TypeSerializer<AutoCSer.Reflection.RemoteType>.MemberSerialize(serializer, ref remoteType);
                    if (DefaultSerializer == null)
                    {
                        //if (serializer.CheckPoint(value)) MemberSerialize(serializer, (valueType)value);
                        T newValue = (T)value;
                        MemberSerialize(serializer, ref newValue);
                    }
                    else DefaultSerializer(serializer, (T)value);
                }
            }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void BaseSerialize<childType>(BinarySerializer serializer, childType value) where childType : T
        {
            if (serializer.CheckPoint(value)) StructSerialize(serializer, value);
        }

        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumByte(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write((uint)AutoCSer.Metadata.EnumCast<T, byte>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumSByte(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write((int)AutoCSer.Metadata.EnumCast<T, sbyte>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumShort(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write((int)AutoCSer.Metadata.EnumCast<T, short>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumUShort(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write((uint)AutoCSer.Metadata.EnumCast<T, ushort>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumInt(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write(AutoCSer.Metadata.EnumCast<T, int>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumUInt(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write(AutoCSer.Metadata.EnumCast<T, uint>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumLong(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write(AutoCSer.Metadata.EnumCast<T, long>.ToInt(value));
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumULong(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write(AutoCSer.Metadata.EnumCast<T, ulong>.ToInt(value));
        }
        /// <summary>
        /// 不支持对象转换null
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void toNull(BinarySerializer serializer, T value)
        {
            serializer.Stream.Write(BinarySerializer.NullValue);
        }
        /// <summary>
        /// 找不到构造函数
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        private static void noConstructor(BinarySerializer serializer, T value)
        {
            if (serializer.CheckPoint(value))
            {
                if (serializer.Config.IsRealType)
                {
                    Type type = value.GetType();
                    if (type == typeof(T)) serializer.Stream.Write(BinarySerializer.NullValue);
                    //else SerializeMethodCache.GetRealSerializer(type)(serializer, value);
                    else GenericType.Get(type).BinarySerializeRealTypeObjectDelegate(serializer, value);
                }
                else serializer.Stream.Write(BinarySerializer.NullValue);
            }
        }

        static TypeSerializer()
        {
            Type type = typeof(T), attributeType;
            MethodInfo methodInfo = BinarySerializer.GetSerializeMethod(type);
            attribute = type.customAttribute<BinarySerializeAttribute>(out attributeType) ?? BinarySerializer.DefaultAttribute;
            if (methodInfo != null)
            {
                DefaultSerializer = (Action<BinarySerializer, T>)Delegate.CreateDelegate(typeof(Action<BinarySerializer, T>), methodInfo);
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
                        Delegate arrayDelegate;
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                arrayDelegate = EnumGenericType.Get(elementType).BinarySerializeEnumArrayDelegate;
                                IsReferenceMember = false;
                            }
                            else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                arrayDelegate = StructGenericType.Get(elementType = elementType.GetGenericArguments()[0]).BinarySerializeNullableArrayMethod;
                                IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                            }
                            else
                            {
                                arrayDelegate = GenericType.Get(elementType).BinarySerializeStructArrayMethod;
                                IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                            }
                        }
                        else
                        {
                            arrayDelegate = ClassGenericType.Get(elementType).BinarySerializeArrayMethod;
                            IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                        }
                        DefaultSerializer = (Action<BinarySerializer, T>)arrayDelegate;
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
                    DefaultSerializer = (Action<BinarySerializer, T>)GenericType.Get(parameterTypes[0]).BinarySerializeLeftArrayMethod;
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
#if !Serialize
                if (genericType == typeof(SubArray<>))
                {
                    DefaultSerializer = (Action<BinarySerializer, T>)GenericType.Get(parameterTypes[0]).BinarySerializeSubArrayMethod;
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
#endif
                if (genericType == typeof(Dictionary<,>) || genericType == typeof(SortedDictionary<,>) || genericType == typeof(SortedList<,>))
                {
                    DefaultSerializer = (Action<BinarySerializer, T>)DictionaryGenericType3.Get(type, parameterTypes[0], parameterTypes[1]).BinarySerializeDictionaryMethod;
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]) || SerializeMethodCache.IsReferenceMember(parameterTypes[1]);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(Nullable<>))
                {
                    DefaultSerializer = (Action<BinarySerializer, T>)StructGenericType.Get(parameterTypes[0]).BinarySerializeNullableDelegate;
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    DefaultSerializer = (Action<BinarySerializer, T>)GenericType2.Get(parameterTypes).BinarySerializeKeyValuePairMethod;
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
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomSerializer", null, new Type[] { typeof(BinarySerializer), type }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarga_S, 1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    DefaultSerializer = (Action<BinarySerializer, T>)dynamicMethod.CreateDelegate(typeof(Action<BinarySerializer, T>));
#endif
                }
                else DefaultSerializer = (Action<BinarySerializer, T>)Delegate.CreateDelegate(typeof(Action<BinarySerializer, T>), methodInfo);
                IsReferenceMember = attribute.IsReferenceMember;
                isValueType = true;
                return;
            }
            if (type.isSerializeNotSupport())
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
                            if (type.IsValueType) DefaultSerializer = (Action<BinarySerializer, T>)DictionaryGenericType3.Get(type, parameters[0], parameters[1]).BinarySerializeStructDictionaryMethod;
                            else DefaultSerializer = (Action<BinarySerializer, T>)DictionaryGenericType3.Get(type, parameters[0], parameters[1]).BinarySerializeClassDictionaryMethod;
                            return;
                        }
                    }
                }
            }
            if (constructorInfo != null)
            {
                Delegate collectionDelegate;
                if (argumentType.IsValueType && argumentType.IsEnum)
                {
                    Type enumType = System.Enum.GetUnderlyingType(argumentType);
                    if (enumType == typeof(uint)) collectionDelegate = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumUIntCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumUIntCollectionMethod;
                    else if (enumType == typeof(byte)) collectionDelegate = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumByteCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumByteCollectionMethod;
                    else if (enumType == typeof(ulong)) collectionDelegate = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumULongCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumULongCollectionMethod;
                    else if (enumType == typeof(ushort)) collectionDelegate = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumUShortCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumUShortCollectionMethod;
                    else if (enumType == typeof(long)) collectionDelegate = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumLongCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumLongCollectionMethod;
                    else if (enumType == typeof(short)) collectionDelegate = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumShortCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumShortCollectionMethod;
                    else if (enumType == typeof(sbyte)) collectionDelegate = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumSByteCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumSByteCollectionMethod;
                    else collectionDelegate = type.IsValueType ? CollectionGenericType2.Get(type, argumentType).BinarySerializeStructEnumIntCollectionMethod : CollectionGenericType2.Get(type, argumentType).BinarySerializeClassEnumIntCollectionMethod;
                }
                else
                {
                    if (type.IsValueType) collectionDelegate = CollectionGenericType2.Get(type, argumentType).BinarySerializeStructCollectionMethod;
                    else collectionDelegate = CollectionGenericType2.Get(type, argumentType).BinarySerializeClassCollectionMethod;
                }
                DefaultSerializer = (Action<BinarySerializer, T>)collectionDelegate;
                return;
            }
            if (type.IsValueType) isValueType = true;
            else if (attribute != BinarySerializer.DefaultAttribute && attributeType != type)
            {
                for (Type baseType = type.BaseType; baseType != typeof(object); baseType = baseType.BaseType)
                {
                    BinarySerializeAttribute baseAttribute = baseType.customAttribute<BinarySerializeAttribute>();
                    if (baseAttribute != null)
                    {
                        if (baseAttribute.IsBaseType)
                        {
                            methodInfo = SerializeMethodCache.BaseSerializeMethod.MakeGenericMethod(baseType, type);
                            DefaultSerializer = (Action<BinarySerializer, T>)Delegate.CreateDelegate(typeof(Action<BinarySerializer, T>), methodInfo);
                            return;
                        }
                        break;
                    }
                }
            }
            isJson = attribute.GetIsJson;
            isMemberMap = attribute.GetIsMemberMap;
            Fields<FieldSize> fields = SerializeMethodCache.GetFields(attribute.IsAnonymousFields ? MemberIndexGroup<T>.GetAnonymousFields(attribute.MemberFilters) : MemberIndexGroup<T>.GetFields(attribute.MemberFilters), isJson, out memberCountVerify);
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
            fixedMemberSerializer = (Action<BinarySerializer, T>)fixedDynamicMethod.Create<Action<BinarySerializer, T>>();
            if (isMemberMap) fixedMemberMapSerializer = (Action<MemberMap, BinarySerializer, T>)fixedMemberMapDynamicMethod.Create<Action<MemberMap, BinarySerializer, T>>();

            if (fields.FieldArray.Length != 0)
            {
                SerializeMemberDynamicMethod dynamicMethod = new SerializeMemberDynamicMethod(type);
                SerializeMemberMapDynamicMethod memberMapDynamicMethod = isMemberMap ? new SerializeMemberMapDynamicMethod(type) : default(SerializeMemberMapDynamicMethod);
                foreach (FieldSize member in fields.FieldArray)
                {
                    dynamicMethod.Push(member);
                    if (isMemberMap) memberMapDynamicMethod.Push(member);
                }
                memberSerializer = (Action<BinarySerializer, T>)dynamicMethod.Create<Action<BinarySerializer, T>>();
                if (isMemberMap) memberMapSerializer = (Action<MemberMap, BinarySerializer, T>)memberMapDynamicMethod.Create<Action<MemberMap, BinarySerializer, T>>();
            }
#endif
            if (fields.JsonFields.Length != 0)
            {
                jsonMemberMap = new MemberMap<T>();
                jsonMemberIndexs = new int[fields.JsonFields.Length];
                int index = 0;
                foreach (FieldIndex field in fields.JsonFields) jsonMemberMap.SetMember(jsonMemberIndexs[index++] = field.MemberIndex);
            }
        }
    }
}
