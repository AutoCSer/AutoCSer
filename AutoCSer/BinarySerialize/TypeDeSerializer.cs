using System;
using AutoCSer.Metadata;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    internal unsafe partial class TypeDeSerializer<valueType>
    {
        /// <summary>
        /// 二进制数据反序列化委托
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">目标数据</param>
        internal delegate void deSerialize(DeSerializer deSerializer, ref valueType value);
        /// <summary>
        /// 二进制数据反序列化委托
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">目标数据</param>
        private delegate void memberMapDeSerialize(MemberMap memberMap, DeSerializer deSerializer, ref valueType value);
        /// <summary>
        /// 二进制数据序列化类型配置
        /// </summary>
        private static readonly SerializeAttribute attribute;
        /// <summary>
        /// 反序列化委托
        /// </summary>
        internal static readonly deSerialize DefaultDeSerializer;
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
        /// 对象反序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerialize(DeSerializer deSerializer, ref valueType value)
        {
            if (isValueType) StructDeSerialize(deSerializer, ref value);
            else ClassDeSerialize(deSerializer, ref value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructDeSerialize(DeSerializer deSerializer, ref valueType value)
        {
            if (DefaultDeSerializer == null) GetDeSerializer(deSerializer.GlobalVersion).MemberDeSerialize(deSerializer, ref value);
            else DefaultDeSerializer(deSerializer, ref value);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassDeSerialize(DeSerializer deSerializer, ref valueType value)
        {
            if (deSerializer.CheckPoint(ref value))
            {
                if (deSerializer.IsRealType()) realType(deSerializer, ref value);
                else classDeSerialize(deSerializer, ref value);
            }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void classDeSerialize(DeSerializer deSerializer, ref valueType value)
        {
            if (DefaultDeSerializer == null)
            {
                deSerializer.AddPoint(ref value);
                GetDeSerializer(deSerializer.GlobalVersion).MemberDeSerialize(deSerializer, ref value);
            }
            else DefaultDeSerializer(deSerializer, ref value);
        }
        /// <summary>
        /// 对象反序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void BaseDeSerialize<childType>(DeSerializer deSerializer, ref childType value) where childType : valueType
        {
            if (value == null) value = AutoCSer.Emit.Constructor<childType>.New();
            valueType newValue = value;
            classDeSerialize(deSerializer, ref newValue);
        }
        /// <summary>
        /// 真实类型反序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void RealType(DeSerializer deSerializer, ref valueType value)
        {
            if (isValueType) StructDeSerialize(deSerializer, ref value);
            else classDeSerialize(deSerializer, ref value);
        }
        /// <summary>
        /// 真实类型
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        private static void realType(DeSerializer deSerializer, ref valueType value)
        {
            RemoteType remoteType = default(RemoteType);
            TypeDeSerializer<RemoteType>.StructDeSerialize(deSerializer, ref remoteType);
            if (deSerializer.State == DeSerializeState.Success)
            {
                Type type;
                if (remoteType.TryGet(out type))
                {
                    if (value == null || type.IsValueType)
                    {
                        //value = (valueType)DeSerializeMethodCache.GetRealDeSerializer(type)(deSerializer, value);
                        value = (valueType)GenericType.Get(type).BinaryDeSerializeRealTypeObjectDelegate(deSerializer, value);
                    }
                    //else DeSerializeMethodCache.GetRealDeSerializer(type)(deSerializer, value);
                    else GenericType.Get(type).BinaryDeSerializeRealTypeObjectDelegate(deSerializer, value);
                }
                else deSerializer.State = DeSerializeState.RemoteTypeError;
            }
        }
        /// <summary>
        /// 不支持对象转换null
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void fromNull(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.CheckNull();
            value = default(valueType);
        }
        /// <summary>
        /// 找不到构造函数
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void noConstructor(DeSerializer deSerializer, ref valueType value)
        {
            //if (deSerializer.IsObjectRealType) deSerializer.State = DeSerializeState.NotNull;
            //else realType(deSerializer, ref value);
            if (deSerializer.IsObjectRealType) realType(deSerializer, ref value);
            else deSerializer.State = DeSerializeState.NotNull;
        }

        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumByte(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.EnumByte(ref value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumSByte(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.EnumSByte(ref value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumShort(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.EnumShort(ref value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumUShort(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.EnumUShort(ref value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumInt(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.EnumInt(ref value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumUInt(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.EnumUInt(ref value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumLong(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.EnumLong(ref value);
        }
        /// <summary>
        /// 枚举值序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">枚举值序列化</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void enumULong(DeSerializer deSerializer, ref valueType value)
        {
            deSerializer.EnumULong(ref value);
        }
        static TypeDeSerializer()
        {
            Type type = typeof(valueType), attributeType;
            MethodInfo methodInfo = DeSerializer.GetDeSerializeMethod(type);
            attribute = type.customAttribute<SerializeAttribute>(out attributeType) ?? Serializer.DefaultAttribute;
            if (methodInfo != null)
            {
                DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), methodInfo);
                IsReferenceMember = false;
                isValueType = true;
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
                                if (enumType == typeof(uint)) methodInfo = GenericType.Get(elementType).BinaryDeSerializeEnumUIntArrayMethod;// DeSerializeMethodCache.EnumUIntArrayMethod.MakeGenericMethod(elementType);
                                else if (enumType == typeof(byte)) methodInfo = GenericType.Get(elementType).BinaryDeSerializeEnumByteArrayMethod;// DeSerializeMethodCache.EnumByteArrayMethod.MakeGenericMethod(elementType);
                                else if (enumType == typeof(ulong)) methodInfo = GenericType.Get(elementType).BinaryDeSerializeEnumULongArrayMethod;// DeSerializeMethodCache.EnumULongArrayMethod.MakeGenericMethod(elementType);
                                else if (enumType == typeof(ushort)) methodInfo = GenericType.Get(elementType).BinaryDeSerializeEnumUShortArrayMethod;// DeSerializeMethodCache.EnumUShortArrayMethod.MakeGenericMethod(elementType);
                                else if (enumType == typeof(long)) methodInfo = GenericType.Get(elementType).BinaryDeSerializeEnumLongArrayMethod;// DeSerializeMethodCache.EnumLongArrayMethod.MakeGenericMethod(elementType);
                                else if (enumType == typeof(short)) methodInfo = GenericType.Get(elementType).BinaryDeSerializeEnumShortArrayMethod;// DeSerializeMethodCache.EnumShortArrayMethod.MakeGenericMethod(elementType);
                                else if (enumType == typeof(sbyte)) methodInfo = GenericType.Get(elementType).BinaryDeSerializeEnumSByteArrayMethod;// DeSerializeMethodCache.EnumSByteArrayMethod.MakeGenericMethod(elementType);
                                else methodInfo = GenericType.Get(elementType).BinaryDeSerializeEnumIntArrayMethod;// DeSerializeMethodCache.EnumIntArrayMethod.MakeGenericMethod(elementType);
                                IsReferenceMember = false;
                            }
                            else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                //methodInfo = DeSerializeMethodCache.NullableArrayMethod.MakeGenericMethod(elementType = elementType.GetGenericArguments()[0]);
                                methodInfo = StructGenericType.Get(elementType = elementType.GetGenericArguments()[0]).BinaryDeSerializeNullableArrayMethod;
                                IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                            }
                            else
                            {
                                //methodInfo = DeSerializeMethodCache.StructArrayMethod.MakeGenericMethod(elementType);
                                methodInfo = GenericType.Get(elementType).BinaryDeSerializeStructArrayMethod;
                                IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                            }
                        }
                        else
                        {
                            //methodInfo = DeSerializeMethodCache.ArrayMethod.MakeGenericMethod(elementType);
                            methodInfo = ClassGenericType.Get(elementType).BinaryDeSerializeArrayMethod;
                            IsReferenceMember = SerializeMethodCache.IsReferenceMember(elementType);
                        }
                        DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), methodInfo);
                        return;
                    }
                }
                DefaultDeSerializer = fromNull;
                IsReferenceMember = false;
                return;
            }
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(uint)) DefaultDeSerializer = enumUInt;
                else if (enumType == typeof(byte)) DefaultDeSerializer = enumByte;
                else if (enumType == typeof(ulong)) DefaultDeSerializer = enumULong;
                else if (enumType == typeof(ushort)) DefaultDeSerializer = enumUShort;
                else if (enumType == typeof(long)) DefaultDeSerializer = enumLong;
                else if (enumType == typeof(short)) DefaultDeSerializer = enumShort;
                else if (enumType == typeof(sbyte)) DefaultDeSerializer = enumSByte;
                else DefaultDeSerializer = enumInt;
                IsReferenceMember = false;
                isValueType = true;
                return;
            }
            if (type.IsPointer || typeof(Delegate).IsAssignableFrom(type))
            {
                DefaultDeSerializer = fromNull;
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
                    //DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), DeSerializeMethodCache.LeftArrayDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
                    DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), GenericType.Get(type.GetGenericArguments()[0]).BinaryDeSerializeLeftArrayMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
#if !Serialize
                if (genericType == typeof(SubArray<>))
                {
                    //DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), DeSerializeMethodCache.SubArrayDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
                    DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), GenericType.Get(type.GetGenericArguments()[0]).BinaryDeSerializeSubArrayMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
#endif
                if (genericType == typeof(Dictionary<,>))
                {
                    //DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), DeSerializeMethodCache.DictionaryDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
                    DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), GenericType2.Get(type.GetGenericArguments()).BinaryDeSerializeDictionaryMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]) || SerializeMethodCache.IsReferenceMember(parameterTypes[1]);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(Nullable<>))
                {
                    //DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), DeSerializeMethodCache.NullableDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
                    DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), StructGenericType.Get(type.GetGenericArguments()[0]).BinaryDeSerializeNullableMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    //DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), DeSerializeMethodCache.KeyValuePairDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
                    DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), GenericType2.Get(type.GetGenericArguments()).BinaryDeSerializeKeyValuePairMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]) || SerializeMethodCache.IsReferenceMember(parameterTypes[1]);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(SortedDictionary<,>))
                {
                    //DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), DeSerializeMethodCache.SortedDictionaryDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
                    DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), GenericType2.Get(type.GetGenericArguments()).BinaryDeSerializeSortedDictionaryMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]) || SerializeMethodCache.IsReferenceMember(parameterTypes[1]);
                    isValueType = true;
                    return;
                }
                if (genericType == typeof(SortedList<,>))
                {
                    //DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), DeSerializeMethodCache.SortedListDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments()));
                    DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), GenericType2.Get(type.GetGenericArguments()).BinaryDeSerializeSortedListMethod);
                    IsReferenceMember = SerializeMethodCache.IsReferenceMember(parameterTypes[0]) || SerializeMethodCache.IsReferenceMember(parameterTypes[1]);
                    isValueType = true;
                    return;
                }
            }
            if ((methodInfo = SerializeMethodCache.GetCustom(type, false)) != null)
            {
                if (type.IsValueType)
                {
#if NOJIT
                    DefaultDeSerializer = new CustomDeSerializer(methodInfo).DeSerialize;
#else
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomDeSerializer", null, new Type[] { typeof(DeSerializer), type.MakeByRefType() }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    DefaultDeSerializer = (deSerialize)dynamicMethod.CreateDelegate(typeof(deSerialize));
#endif
                }
                else DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), methodInfo);
                IsReferenceMember = attribute.IsReferenceMember;
                isValueType = true;
                return;
            }
            if (type.IsAbstract || type.IsInterface || Emit.Constructor<valueType>.New == null)
            {
                DefaultDeSerializer = noConstructor;
                isValueType = IsReferenceMember = true;
                return;
            }
            IsReferenceMember = attribute.IsReferenceMember;
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType)
                {
                    Type genericType = interfaceType.GetGenericTypeDefinition();
                    if (genericType == typeof(ICollection<>))
                    {
                        Type[] parameters = interfaceType.GetGenericArguments();
                        Type argumentType = parameters[0];
                        parameters[0] = argumentType.MakeArrayType();
                        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //methodInfo = (type.IsValueType ? DeSerializeMethodCache.StructCollectionMethod : DeSerializeMethodCache.ClassCollectionMethod).MakeGenericMethod(type, argumentType);
                            if (type.IsValueType) methodInfo = CollectionGenericType2.Get(type, argumentType).BinaryDeSerializeStructCollectionMethod;
                            else methodInfo = CollectionGenericType2.Get(type, argumentType).BinaryDeSerializeClassCollectionMethod;
                            break;
                        }
                        parameters[0] = typeof(IList<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //methodInfo = (type.IsValueType ? DeSerializeMethodCache.StructCollectionMethod : DeSerializeMethodCache.ClassCollectionMethod).MakeGenericMethod(type, argumentType);
                            if (type.IsValueType) methodInfo = CollectionGenericType2.Get(type, argumentType).BinaryDeSerializeStructCollectionMethod;
                            else methodInfo = CollectionGenericType2.Get(type, argumentType).BinaryDeSerializeClassCollectionMethod;
                            break;
                        }
                        parameters[0] = typeof(ICollection<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //methodInfo = (type.IsValueType ? DeSerializeMethodCache.StructCollectionMethod : DeSerializeMethodCache.ClassCollectionMethod).MakeGenericMethod(type, argumentType);
                            if (type.IsValueType) methodInfo = CollectionGenericType2.Get(type, argumentType).BinaryDeSerializeStructCollectionMethod;
                            else methodInfo = CollectionGenericType2.Get(type, argumentType).BinaryDeSerializeClassCollectionMethod;
                            break;
                        }
                        parameters[0] = typeof(IEnumerable<>).MakeGenericType(argumentType);
                        constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                        if (constructorInfo != null)
                        {
                            //methodInfo = (type.IsValueType ? DeSerializeMethodCache.StructCollectionMethod : DeSerializeMethodCache.ClassCollectionMethod).MakeGenericMethod(type, argumentType);
                            if (type.IsValueType) methodInfo = CollectionGenericType2.Get(type, argumentType).BinaryDeSerializeStructCollectionMethod;
                            else methodInfo = CollectionGenericType2.Get(type, argumentType).BinaryDeSerializeClassCollectionMethod;
                            break;
                        }
                    }
                    else if (genericType == typeof(IDictionary<,>))
                    {
                        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { interfaceType }, null);
                        if (constructorInfo != null)
                        {
                            Type[] parameters = interfaceType.GetGenericArguments();
                            //methodInfo = (type.IsValueType ? DeSerializeMethodCache.StructDictionaryDeSerializeMethod : DeSerializeMethodCache.ClassDictionaryDeSerializeMethod).MakeGenericMethod(type, parameters[0], parameters[1]);
                            if (type.IsValueType) methodInfo = DictionaryGenericType3.Get(type, parameters[0], parameters[1]).BinaryDeSerializeStructDictionaryMethod;
                            else methodInfo = DictionaryGenericType3.Get(type, parameters[0], parameters[1]).BinaryDeSerializeClassDictionaryMethod;
                            break;
                        }
                    }
                }
            }
            if (methodInfo != null)
            {
                DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), methodInfo);
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
                            methodInfo = DeSerializeMethodCache.BaseSerializeMethod.MakeGenericMethod(baseType, type);
                            DefaultDeSerializer = (deSerialize)Delegate.CreateDelegate(typeof(deSerialize), methodInfo);
                            return;
                        }
                        break;
                    }
                }
            }

            LeftArray<DeSerializeVersionField> attributeFields = new LeftArray<DeSerializeVersionField>();
            if ((attribute.MemberFilters & MemberFilters.PublicInstanceField) != 0) appendField(ref attributeFields, MemberIndexGroup<valueType>.Group.PublicFields);
            else
            {
                foreach (FieldIndex field in MemberIndexGroup<valueType>.Group.PublicFields) attributeFields.Add(new DeSerializeVersionField { Field = field });
            }
            if ((attribute.MemberFilters & MemberFilters.NonPublicInstanceField) != 0) appendField(ref attributeFields, MemberIndexGroup<valueType>.Group.NonPublicFields);
            if (attribute.IsAnonymousFields) appendField(ref attributeFields, MemberIndexGroup<valueType>.Group.AnonymousFields);
            foreach (FieldIndex field in new MemberIndexGroup(type, true).NonPublicFields)
            {
                Type fieldType = field.Member.FieldType;
                if (!fieldType.IsPointer && (!fieldType.IsArray || fieldType.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(fieldType))
                {
                    SerializeMemberAttribute memberAttribute = field.GetAttribute<SerializeMemberAttribute>(true);
                    if (memberAttribute != null && memberAttribute.IsSetup && memberAttribute.RemoveGlobalVersion != memberAttribute.GlobalVersion)
                    {
                        attributeFields.Add(new DeSerializeVersionField { Field = field, Attribute = memberAttribute });
                        if (memberAttribute.IsRemove) attributeFields.Add(new DeSerializeVersionField { Field = field, Attribute = memberAttribute, IsRemove = true });
                    }
                }
            }

            isMemberMap = attribute.GetIsMemberMap;
            isJson = attribute.GetIsJson;
            uint globalVersion = 0, removeMemberCount = 0;
            int noSerializeMemberCount = 0;
            LeftArray<DeSerializeVersionFields<valueType>> deSerializeVersionFields = default(LeftArray<DeSerializeVersionFields<valueType>>);
            LeftArray<DeSerializeVersionField> attributeVersionFields = new LeftArray<DeSerializeVersionField>(attributeFields.Length);
            foreach (DeSerializeVersionField field in attributeFields.Sort(DeSerializeVersionField.GlobalVersionSort))
            {
                if (field.GlobalVersion != globalVersion)
                {
                    deSerializeVersionFields.Add(new DeSerializeVersionFields<valueType>(globalVersion, attributeVersionFields.GetArray(), removeMemberCount, noSerializeMemberCount));
                    globalVersion = field.GlobalVersion;
                }
                if (field.IsRemove)
                {
                    attributeVersionFields.RemoveToEnd(value => object.ReferenceEquals(value.Field, field.Field));
                    if (field.Attribute.IsRemove) --removeMemberCount;
                }
                else
                {
                    attributeVersionFields.Add(field);
                    if (field.Attribute != null)
                    {
                        if (field.Attribute.IsRemove) ++removeMemberCount;
                    }
                    else ++noSerializeMemberCount;
                }
            }
            fieldDeSerializer = new DeSerializeVersionFields<valueType>(globalVersion, attributeVersionFields.GetArray(), removeMemberCount, noSerializeMemberCount).CreateOnly(attribute);
            if (deSerializeVersionFields.Length != 0)
            {
                int count = deSerializeVersionFields.Length;
                DeSerializeVersionFields<valueType>[] deSerializeVersionFieldsArray = new DeSerializeVersionFields<valueType>[count];
                foreach (DeSerializeVersionFields<valueType> value in deSerializeVersionFields)
                {
                    deSerializeVersionFieldsArray[--count] = value;
                    deSerializeVersionFieldsArray[count].CreateLock = new object();
                }
                fieldDeSerializers = deSerializeVersionFieldsArray;
            }
        }
        /// <summary>
        /// 追加字段信息
        /// </summary>
        /// <param name="attributeFields"></param>
        /// <param name="fieldIndexs"></param>
        private static void appendField(ref LeftArray<DeSerializeVersionField> attributeFields, FieldIndex[] fieldIndexs)
        {
            foreach (FieldIndex field in fieldIndexs)
            {
                bool isSerialize = false;
                Type fieldType = field.Member.FieldType;
                if (!fieldType.IsPointer && (!fieldType.IsArray || fieldType.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(fieldType))
                {
                    SerializeMemberAttribute memberAttribute = field.GetAttribute<SerializeMemberAttribute>(true);
                    if (memberAttribute == null)
                    {
                        attributeFields.Add(new DeSerializeVersionField { Field = field, Attribute = SerializeMemberAttribute.Null });
                        isSerialize = true;
                    }
                    else if (memberAttribute.IsSetup)
                    {
                        attributeFields.Add(new DeSerializeVersionField { Field = field, Attribute = memberAttribute });
                        if (memberAttribute.IsRemove) attributeFields.Add(new DeSerializeVersionField { Field = field, Attribute = memberAttribute, IsRemove = true });
                        isSerialize = true;
                    }
                }
                if (!isSerialize) attributeFields.Add(new DeSerializeVersionField { Field = field });
            }
        }

        /// <summary>
        /// 默认二进制数据反序列化
        /// </summary>
        private static readonly TypeDeSerializer<valueType> fieldDeSerializer;
        /// <summary>
        /// 默认二进制数据反序列化集合
        /// </summary>
        private static DeSerializeVersionFields<valueType>[] fieldDeSerializers;
        /// <summary>
        /// 获取二进制数据反序列化
        /// </summary>
        /// <param name="globalVersion"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static TypeDeSerializer<valueType> GetDeSerializer(uint globalVersion)
        {
            if (fieldDeSerializers == null || globalVersion >= fieldDeSerializer.globalVersion) return fieldDeSerializer;
            return getDeSerializer(globalVersion);
        }
        /// <summary>
        /// 获取二进制数据反序列化
        /// </summary>
        /// <param name="globalVersion"></param>
        /// <returns></returns>
        private static TypeDeSerializer<valueType> getDeSerializer(uint globalVersion)
        {
            int index = 0;
            foreach (DeSerializeVersionFields<valueType> deSerializeVersionFields in fieldDeSerializers)
            {
                if (globalVersion >= deSerializeVersionFields.GlobalVersion) return deSerializeVersionFields.DeSerializer ?? fieldDeSerializers[index].Create(attribute);
                ++index;
            }
            return null;
        }
        /// <summary>
        /// 固定分组成员序列化
        /// </summary>
        private readonly deSerialize fixedMemberDeSerializer;
        /// <summary>
        /// 固定分组成员位图序列化
        /// </summary>
        private readonly memberMapDeSerialize fixedMemberMapDeSerializer;
        /// <summary>
        /// 成员序列化
        /// </summary>
        private readonly deSerialize memberDeSerializer;
        /// <summary>
        /// 成员位图序列化
        /// </summary>
        private readonly memberMapDeSerialize memberMapDeSerializer;
        /// <summary>
        /// JSON混合序列化位图
        /// </summary>
        private readonly MemberMap jsonMemberMap;
        /// <summary>
        /// JSON混合序列化成员索引集合
        /// </summary>
        private readonly int[] jsonMemberIndexs;
        /// <summary>
        /// 全局版本编号
        /// </summary>
        private readonly uint globalVersion;
        /// <summary>
        /// 固定分组填充字节数
        /// </summary>
        private readonly int fixedFillSize;
        /// <summary>
        /// 序列化成员数量
        /// </summary>
        private readonly int memberCountVerify;
        /// <summary>
        /// 二进制数据反序列化
        /// </summary>
        /// <param name="globalVersion"></param>
        /// <param name="fields"></param>
        /// <param name="memberCountVerify"></param>
        internal TypeDeSerializer(uint globalVersion, ref Fields<FieldSize> fields, int memberCountVerify)
        {
            Type type = typeof(valueType);
            this.globalVersion = globalVersion;
            this.memberCountVerify = memberCountVerify;
            fixedFillSize = -fields.FixedSize & 3;
#if NOJIT
            fixedMemberDeSerializer = new FieldDeSerializer(ref fields.FixedFields).DeSerialize;
            if (isMemberMap) fixedMemberMapDeSerializer = new MemberMapDeSerializer(ref fields.FixedFields).DeSerialize;
            if (fields.FieldArray.Length != 0)
            {
                memberDeSerializer = new FieldDeSerializer(ref fields.FieldArray).DeSerialize;
                if (isMemberMap) memberMapDeSerializer = new MemberMapDeSerializer(ref fields.FieldArray).DeSerialize;
            }
#else
            DeSerializeMemberDynamicMethod fixedDynamicMethod = new DeSerializeMemberDynamicMethod(type);
            DeSerializeMemberMapDynamicMethod fixedMemberMapDynamicMethod = isMemberMap ? new DeSerializeMemberMapDynamicMethod(type) : default(DeSerializeMemberMapDynamicMethod);
            foreach (FieldSize member in fields.FixedFields)
            {
                fixedDynamicMethod.Push(member);
                if (isMemberMap) fixedMemberMapDynamicMethod.Push(member);
            }
            fixedMemberDeSerializer = (deSerialize)fixedDynamicMethod.Create<deSerialize>();
            if (isMemberMap) fixedMemberMapDeSerializer = (memberMapDeSerialize)fixedMemberMapDynamicMethod.Create<memberMapDeSerialize>();

            if (fields.FieldArray.Length != 0)
            {
                DeSerializeMemberDynamicMethod dynamicMethod = new DeSerializeMemberDynamicMethod(type);
                DeSerializeMemberMapDynamicMethod memberMapDynamicMethod = isMemberMap ? new DeSerializeMemberMapDynamicMethod(type) : default(DeSerializeMemberMapDynamicMethod);
                foreach (FieldSize member in fields.FieldArray)
                {
                    dynamicMethod.Push(member);
                    if (isMemberMap) memberMapDynamicMethod.Push(member);
                }
                memberDeSerializer = (deSerialize)dynamicMethod.Create<deSerialize>();
                if (isMemberMap) memberMapDeSerializer = (memberMapDeSerialize)memberMapDynamicMethod.Create<memberMapDeSerialize>();
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
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="deSerializer">二进制数据反序列化</param>
        /// <param name="value">数据对象</param>
        internal void MemberDeSerialize(DeSerializer deSerializer, ref valueType value)
        {
            if (deSerializer.CheckMemberCount(memberCountVerify))
            {
                fixedMemberDeSerializer(deSerializer, ref value);
                deSerializer.Read += fixedFillSize;
                if (memberDeSerializer != null) memberDeSerializer(deSerializer, ref value);
                if (isJson || jsonMemberMap != null) deSerializer.ParseJson(ref value);
            }
            else if (isMemberMap)
            {
                MemberMap memberMap = deSerializer.GetMemberMap<valueType>();
                if (memberMap != null)
                {
                    byte* start = deSerializer.Read;
                    fixedMemberMapDeSerializer(memberMap, deSerializer, ref value);
                    deSerializer.Read += (int)(start - deSerializer.Read) & 3;
                    if (memberMapDeSerializer != null) memberMapDeSerializer(memberMap, deSerializer, ref value);
                    if (isJson) deSerializer.ParseJson(ref value);
                    else if (jsonMemberMap != null)
                    {
                        foreach (int memberIndex in jsonMemberIndexs)
                        {
                            if (memberMap.IsMember(memberIndex))
                            {
                                deSerializer.ParseJson(ref value);
                                return;
                            }
                        }
                    }
                }
            }
            else deSerializer.State = DeSerializeState.MemberMap;
        }
    }
}
