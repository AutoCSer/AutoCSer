using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型解析器
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    internal unsafe static partial class TypeDeSerializer<T>
    {
        /// <summary>
        /// 成员解析器过滤
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct TryDeSerializeFilter
        {
            /// <summary>
            /// 成员解析器
            /// </summary>
            public JsonDeSerializer.DeSerializeDelegate<T> TryDeSerialize;
            /// <summary>
            /// 成员位图索引
            /// </summary>
            public int MemberMapIndex;
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="jsonDeSerializer">Json解析器</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">目标数据</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Call(JsonDeSerializer jsonDeSerializer, MemberMap memberMap, ref T value)
            {
                TryDeSerialize(jsonDeSerializer, ref value);
                memberMap.SetMember(MemberMapIndex);
            }
        }
        /// <summary>
        /// 未知名称处理委托
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        private delegate void UnknownDeSerialize(JsonDeSerializer jsonDeSerializer, ref T value, ref AutoCSer.Memory.Pointer name);
#if !NOJIT
        /// <summary>
        /// 默认名称解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        private delegate int DeSerializeMember(JsonDeSerializer jsonDeSerializer, ref T value, byte* names);
        /// <summary>
        /// 默认名称解析
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <param name="names"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        private delegate int DeSerializeMemberMap(JsonDeSerializer jsonDeSerializer, ref T value, byte* names, MemberMap memberMap);
        /// <summary>
        /// 默认名称解析
        /// </summary>
        private static readonly DeSerializeMember deSerializeMember;
        /// <summary>
        /// 默认名称解析
        /// </summary>
        private static readonly DeSerializeMemberMap deSerializeMemberMap;
#endif
        /// <summary>
        /// JSON 解析类型配置
        /// </summary>
        private static readonly JsonDeSerializeAttribute attribute;
        /// <summary>
        /// 解析委托
        /// </summary>
        internal static readonly JsonDeSerializer.DeSerializeDelegate<T> DefaultDeSerializer;
        /// <summary>
        /// 未知名称处理委托
        /// </summary>
        private static readonly UnknownDeSerialize onUnknownName;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;
        /// <summary>
        /// 是否匿名类型
        /// </summary>
        private static readonly bool isAnonymousType;
        /// <summary>
        /// 成员解析器集合
        /// </summary>
        private static readonly TryDeSerializeFilter[] memberDeSerializers;
        /// <summary>
        /// 包装处理
        /// </summary>
        private static readonly JsonDeSerializer.DeSerializeDelegate<T> unboxDeSerializer;
        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        private static readonly StateSearcher memberSearcher;
        /// <summary>
        /// 默认顺序成员名称数据
        /// </summary>
        private static AutoCSer.Memory.Pointer memberNames;
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerialize(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (DefaultDeSerializer == null)
            {
                if (isValueType) DeSerializeValue(jsonDeSerializer, ref value);
                else deSerializeClass(jsonDeSerializer, ref value);
            }
            else DefaultDeSerializer(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerializeValue(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.SearchObject()) DeSerializeMembers(jsonDeSerializer, ref value);
            else value = default(T);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="value">目标数据</param>
        private static void deSerializeClass(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (jsonDeSerializer.SearchObject())
            {
                if (value == null)
                {
                    if (AutoCSer.Metadata.DefaultConstructor<T>.Type == DefaultConstructorType.None)
                    {
                        jsonDeSerializer.CheckNoConstructor(ref value, isAnonymousType);
                        if (value == null) return;
                    }
                    else value = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
                }
                else if (isAnonymousType) jsonDeSerializer.SetAnonymousType(value);
                DeSerializeMembers(jsonDeSerializer, ref value);
            }
            else
            {
                if (value != null && isAnonymousType) jsonDeSerializer.SetAnonymousType(value);
                value = default(T);
            }
        }
        /// <summary>
        /// 数据成员解析
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeMembers(JsonDeSerializer jsonDeSerializer, ref T value)
        {
#if NOJIT
            byte* names = memberNames.Byte;
#endif
            DeSerializeConfig config = jsonDeSerializer.Config;
            MemberMap memberMap = jsonDeSerializer.MemberMap;
            if (memberMap == null)
            {
#if NOJIT
                int index = 0;
                while ((names = jsonDeSerializer.IsName(names, ref index)) != null)
                {
                    if (index == -1) return;
                    memberDeSerializers[index].TryDeSerialize(jsonDeSerializer, ref value);
                    if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                    ++index;
                }
#else
                int index = deSerializeMember(jsonDeSerializer, ref value, memberNames.Byte);
                if (index == -1) return;
#endif
                if (index == 0 ? jsonDeSerializer.IsFirstObject() : jsonDeSerializer.IsNextObject())
                {
                    bool isQuote;
                    if (onUnknownName == null)
                    {
                        do
                        {
                            if ((index = memberSearcher.SearchName(jsonDeSerializer, out isQuote)) != -1)
                            {
                                if (jsonDeSerializer.SearchColon() == 0) return;
                                memberDeSerializers[index].TryDeSerialize(jsonDeSerializer, ref value);
                            }
                            else
                            {
                                if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                                if (isQuote) jsonDeSerializer.SearchStringEnd();
                                else jsonDeSerializer.SearchNameEnd();
                                if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success || jsonDeSerializer.SearchColon() == 0) return;
                                jsonDeSerializer.Ignore();
                            }
                        }
                        while (jsonDeSerializer.DeSerializeState == DeSerializeState.Success && jsonDeSerializer.IsNextObject());
                    }
                    else
                    {
                        AutoCSer.Memory.Pointer name;
#if !Serialize
                        name.CurrentIndex = 0;
#endif
                        do
                        {
                            name.Data = jsonDeSerializer.Current;
                            if ((index = memberSearcher.SearchName(jsonDeSerializer, out isQuote)) != -1)
                            {
                                if (jsonDeSerializer.SearchColon() == 0) return;
                                memberDeSerializers[index].TryDeSerialize(jsonDeSerializer, ref value);
                            }
                            else
                            {
                                if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                                if (isQuote) jsonDeSerializer.SearchStringEnd();
                                else jsonDeSerializer.SearchNameEnd();
                                if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                                name.ByteSize = (int)((byte*)jsonDeSerializer.Current - (byte*)name.Data);
                                if (jsonDeSerializer.SearchColon() == 0) return;
                                if (isQuote)
                                {
                                    name.Data = name.Byte + 2;
                                    name.ByteSize -= 4;
                                }
                                onUnknownName(jsonDeSerializer, ref value, ref name);
                                if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                            }
                        }
                        while (jsonDeSerializer.IsNextObject());
                    }
                }
            }
            else if (object.ReferenceEquals(memberMap.Type, MemberMap<T>.MemberMapType))
            {
                try
                {
                    memberMap.Empty();
                    jsonDeSerializer.MemberMap = null;
#if NOJIT
                    int index = 0;
                    while ((names = jsonDeSerializer.IsName(names, ref index)) != null)
                    {
                        if (index == -1) return;
                        memberDeSerializers[index].Call(jsonDeSerializer, memberMap, ref value);
                        if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                        ++index;
                    }
#else
                    int index = deSerializeMemberMap(jsonDeSerializer, ref value, memberNames.Byte, memberMap);
                    if (index == -1) return;
#endif
                    if (index == 0 ? jsonDeSerializer.IsFirstObject() : jsonDeSerializer.IsNextObject())
                    {
                        bool isQuote;
                        if (onUnknownName == null)
                        {
                            do
                            {
                                if ((index = memberSearcher.SearchName(jsonDeSerializer, out isQuote)) != -1)
                                {
                                    if (jsonDeSerializer.SearchColon() == 0) return;
                                    memberDeSerializers[index].Call(jsonDeSerializer, memberMap, ref value);
                                }
                                else
                                {
                                    if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                                    if (isQuote) jsonDeSerializer.SearchStringEnd();
                                    else jsonDeSerializer.SearchNameEnd();
                                    if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success || jsonDeSerializer.SearchColon() == 0) return;
                                    jsonDeSerializer.Ignore();
                                }
                            }
                            while (jsonDeSerializer.DeSerializeState == DeSerializeState.Success && jsonDeSerializer.IsNextObject());
                        }
                        else
                        {
                            AutoCSer.Memory.Pointer name;
#if !Serialize
                            name.CurrentIndex = 0;
#endif
                            do
                            {
                                name.Data = jsonDeSerializer.Current;
                                if ((index = memberSearcher.SearchName(jsonDeSerializer, out isQuote)) != -1)
                                {
                                    if (jsonDeSerializer.SearchColon() == 0) return;
                                    memberDeSerializers[index].Call(jsonDeSerializer, memberMap, ref value);
                                }
                                else
                                {
                                    if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                                    if (isQuote) jsonDeSerializer.SearchStringEnd();
                                    else jsonDeSerializer.SearchNameEnd();
                                    if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                                    name.ByteSize = (int)((byte*)jsonDeSerializer.Current - (byte*)name.Data);
                                    if (jsonDeSerializer.SearchColon() == 0) return;
                                    if (isQuote)
                                    {
                                        name.Data = name.Byte + 2;
                                        name.ByteSize -= 4;
                                    }
                                    onUnknownName(jsonDeSerializer, ref value, ref name);
                                    if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return;
                                }
                            }
                            while (jsonDeSerializer.IsNextObject());
                        }
                    }
                }
                finally { jsonDeSerializer.MemberMap = memberMap; }
            }
            else jsonDeSerializer.DeSerializeState = DeSerializeState.MemberMap;
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerializeClass(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (DefaultDeSerializer == null) deSerializeClass(jsonDeSerializer, ref value);
            else DefaultDeSerializer(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerializeStruct(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            if (DefaultDeSerializer == null) DeSerializeValue(jsonDeSerializer, ref value);
            else DefaultDeSerializer(jsonDeSerializer, ref value);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="values">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Array(JsonDeSerializer jsonDeSerializer, ref T[] values)
        {
            int count = ArrayIndex(jsonDeSerializer, ref values);
            if (count >= 0 && count != values.Length) System.Array.Resize(ref values, count);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="jsonDeSerializer">Json解析器</param>
        /// <param name="array">目标数据</param>
        /// <returns>数据数量,-1表示失败</returns>
        internal static int ArrayIndex(JsonDeSerializer jsonDeSerializer, ref T[] array)
        {
            switch (jsonDeSerializer.SearchArray(ref array))
            {
                case 0:
                    array = new T[jsonDeSerializer.Config.NewArraySize];
                    int index = 0;
                    do
                    {
                        if (index != array.Length)
                        {
                            DeSerialize(jsonDeSerializer, ref array[index]);
                            if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return -1;
                            ++index;
                        }
                        else
                        {
                            T value = default(T);
                            DeSerialize(jsonDeSerializer, ref value);
                            if (jsonDeSerializer.DeSerializeState != DeSerializeState.Success) return -1;
                            array = array.copyNew(index == 0 ? jsonDeSerializer.Config.NewArraySize : (index << 1));
                            array[index++] = value;
                        }
                    }
                    while (jsonDeSerializer.IsNextArrayValue());
                    return jsonDeSerializer.DeSerializeState == DeSerializeState.Success ? index : -1;
                case 1: return 0;
                default: return -1;
            }
        }

        /// <summary>
        /// 包装处理
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void unbox(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            unboxDeSerializer(jsonDeSerializer, ref value);
        }

        static TypeDeSerializer()
        {
            Type type = typeof(T);
            MethodInfo methodInfo = JsonDeSerializer.GetDeSerializeMethod(type);
            if (methodInfo != null)
            {
                DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)Delegate.CreateDelegate(typeof(JsonDeSerializer.DeSerializeDelegate<T>), methodInfo);
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1) DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)GenericType.Get(type.GetElementType()).JsonDeSerializeArrayMethod;
                else DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)GenericType.Get(type).JsonDeSerializeNotSupportDelegate;
                return;
            }
            if (type.IsEnum)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false)) DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)EnumGenericType.Get(type).JsonDeSerializeEnumFlagsDelegate;
                else DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)EnumGenericType.Get(type).JsonDeSerializeEnumDelegate;
                return;
            }
            if (type.isSerializeNotSupport())
            {
                DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)GenericType.Get(type).JsonDeSerializeNotSupportDelegate;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>))
                {
                    DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)GenericType2.Get(type.GetGenericArguments()).JsonDeSerializeDictionaryMethod;
                    return;
                }
                if (genericType == typeof(Nullable<>))
                {
                    DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)DeSerializeMethodCache.GetNullable(type);
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)GenericType2.Get(type.GetGenericArguments()).JsonDeSerializeKeyValuePairMethod;
                    isValueType = true;
                    return;
                }
            }
            if ((methodInfo = DeSerializeMethodCache.GetCustom(type)) != null)
            {
                if (type.IsValueType)
                {
#if NOJIT
                    DefaultDeSerializer = new CustomDeSerializer(methodInfo).DeSerialize;
#else
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomJsonDeSerializer", null, new Type[] { typeof(JsonDeSerializer), type.MakeByRefType() }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)dynamicMethod.CreateDelegate(typeof(JsonDeSerializer.DeSerializeDelegate<T>));
#endif
                }
                else DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)Delegate.CreateDelegate(typeof(JsonDeSerializer.DeSerializeDelegate<T>), methodInfo);
            }
            else
            {
                Type attributeType;
                attribute = type.customAttribute<JsonDeSerializeAttribute>(out attributeType) ?? JsonDeSerializer.AllMemberAttribute;
                if ((methodInfo = DeSerializeMethodCache.GetIEnumerableConstructor(type)) != null)
                {
                    DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)Delegate.CreateDelegate(typeof(JsonDeSerializer.DeSerializeDelegate<T>), methodInfo);
                }
                else
                {
                    if (type.IsValueType) isValueType = true;
                    else if (attribute != JsonDeSerializer.AllMemberAttribute && attributeType != type)
                    {
                        for (Type baseType = type.BaseType; baseType != typeof(object); baseType = baseType.BaseType)
                        {
                            JsonDeSerializeAttribute baseAttribute = baseType.customAttribute<JsonDeSerializeAttribute>();
                            if (baseAttribute != null)
                            {
                                if (baseAttribute.IsBaseType)
                                {
                                    methodInfo = DeSerializeMethodCache.BaseDeSerializeMethod.MakeGenericMethod(baseType, type);
                                    DefaultDeSerializer = (JsonDeSerializer.DeSerializeDelegate<T>)Delegate.CreateDelegate(typeof(JsonDeSerializer.DeSerializeDelegate<T>), methodInfo);
                                    return;
                                }
                                break;
                            }
                        }
                    }
                    FieldIndex defaultMember = null;
                    LeftArray<FieldIndex> fields = DeSerializeMethodCache.GetFields(MemberIndexGroup<T>.GetFields(attribute.MemberFilters), attribute, ref defaultMember);
                    LeftArray<KeyValue<PropertyIndex, MethodInfo>> properties = DeSerializeMethodCache.GetProperties(MemberIndexGroup<T>.GetProperties(attribute.MemberFilters), attribute);
                    bool isBox = false;
                    if (type.IsValueType && fields.Length + properties.Length == 1)
                    {
                        BoxSerializeAttribute boxSerialize = AutoCSer.Metadata.TypeAttribute.GetAttribute<BoxSerializeAttribute>(type);
                        if (boxSerialize != null && boxSerialize.IsJson)
                        {
                            isBox = true;
                            defaultMember = null;
                        }
                    }
                    TryDeSerializeFilter[] deSerializers = new TryDeSerializeFilter[fields.Length + properties.Length + (defaultMember == null ? 0 : 1)];
                    //memberMap.type memberMapType = memberMap<valueType>.TypeInfo;
                    string[] names = isBox ? null : new string[deSerializers.Length];
#if !NOJIT
                    DeSerializeDynamicMethod dynamicMethod = isBox ? default(DeSerializeDynamicMethod) : new DeSerializeDynamicMethod(type, false), memberMapDynamicMethod = isBox ? default(DeSerializeDynamicMethod) : new DeSerializeDynamicMethod(type, true);
#endif
                    int index = 0, nameLength = 0, maxNameLength = 0;
                    foreach (FieldIndex member in fields)
                    {
                        TryDeSerializeFilter tryDeSerialize = deSerializers[index] = new TryDeSerializeFilter
                        {
#if NOJIT
                            TryDeSerialize = new FieldDeSerializer(member.Member).DeSerializer(),
#else
                            TryDeSerialize = (JsonDeSerializer.DeSerializeDelegate<T>)DeSerializeMethodCache.CreateDynamicMethod(type, member.Member).CreateDelegate(typeof(JsonDeSerializer.DeSerializeDelegate<T>)),
#endif
                            MemberMapIndex = member.MemberIndex,
                            //MemberFilters = member.Member.IsPublic ? Metadata.MemberFilters.PublicInstanceField : Metadata.MemberFilters.NonPublicInstanceField
                        };
                        if (!isBox)
                        {
#if !NOJIT
                            dynamicMethod.Push(member);
                            memberMapDynamicMethod.Push(member);
#endif
                            string name = member.AnonymousName;
                            if (name.Length > maxNameLength) maxNameLength = name.Length;
                            nameLength += (names[index++] = name).Length;
                            if (member == defaultMember)
                            {
                                deSerializers[deSerializers.Length - 1] = tryDeSerialize;
                                names[deSerializers.Length - 1] = string.Empty;
                            }
                        }
                    }
                    foreach (KeyValue<PropertyIndex, MethodInfo> member in properties)
                    {
                        deSerializers[index] = new TryDeSerializeFilter
                        {
#if NOJIT
                            TryDeSerialize = new PropertyDeSerializer(member.Key.Member).DeSerializer(),
#else
                            TryDeSerialize = (JsonDeSerializer.DeSerializeDelegate<T>)DeSerializeMethodCache.CreateDynamicMethod(type, member.Key.Member, member.Value).CreateDelegate(typeof(JsonDeSerializer.DeSerializeDelegate<T>)),
#endif
                            MemberMapIndex = member.Key.MemberIndex,
                            //MemberFilters = member.Value.IsPublic ? Metadata.MemberFilters.PublicInstanceProperty : Metadata.MemberFilters.NonPublicInstanceProperty
                        };
                        if (!isBox)
                        {
#if !NOJIT
                            dynamicMethod.Push(member.Key, member.Value);
                            memberMapDynamicMethod.Push(member.Key, member.Value);
#endif
                            if (member.Key.Member.Name.Length > maxNameLength) maxNameLength = member.Key.Member.Name.Length;
                            nameLength += (names[index++] = member.Key.Member.Name).Length;
                        }
                    }
                    if (isBox)
                    {
                        unboxDeSerializer = deSerializers[0].TryDeSerialize;
                        DefaultDeSerializer = unbox;
                    }
                    else
                    {
#if !NOJIT
                        deSerializeMember = (DeSerializeMember)dynamicMethod.Create<DeSerializeMember>();
                        deSerializeMemberMap = (DeSerializeMemberMap)memberMapDynamicMethod.Create<DeSerializeMemberMap>();
#endif
                        if (type.Name[0] == '<') isAnonymousType = true;
                        if (maxNameLength > (short.MaxValue >> 1) - 4 || nameLength == 0) memberNames = Unmanaged.NullByte8;
                        else
                        {
                            memberNames = Unmanaged.GetStaticPointer((nameLength + (names.Length - (defaultMember == null ? 0 : 1)) * 5 + 1) << 1, false);
                            byte* write = memberNames.Byte;
                            foreach (string name in names)
                            {
                                if (name.Length != 0)
                                {
                                    if (write == memberNames.Byte)
                                    {
                                        *(short*)write = (short)((name.Length + 3) * sizeof(char));
                                        *(char*)(write + sizeof(short)) = '"';
                                        write += sizeof(short) + sizeof(char);
                                    }
                                    else
                                    {
                                        *(short*)write = (short)((name.Length + 4) * sizeof(char));
                                        *(int*)(write + sizeof(short)) = ',' + ('"' << 16);
                                        write += sizeof(short) + sizeof(int);
                                    }
                                    fixed (char* nameFixed = name) AutoCSer.Extensions.StringExtension.SimpleCopyNotNull(nameFixed, (char*)write, name.Length);
                                    *(int*)(write += name.Length << 1) = '"' + (':' << 16);
                                    write += sizeof(int);
                                }
                            }
                            *(short*)write = 0;
                        }
                        memberSearcher = new StateSearcher(StateSearcher.GetMemberSearcher(type, names));
                        memberDeSerializers = deSerializers;

                        Type refType = type.MakeByRefType();
                        foreach (AutoCSer.Metadata.AttributeMethod attributeMethod in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                        {
                            if ((methodInfo = attributeMethod.Method).ReturnType == typeof(void))
                            {
                                ParameterInfo[] parameters = methodInfo.GetParameters();
                                if (parameters.Length == 3 && parameters[0].ParameterType == typeof(JsonDeSerializer) && parameters[1].ParameterType == refType && parameters[2].ParameterType == Emit.Pub.PointerSizeRefType)
                                {
                                    if (attributeMethod.GetAttribute<JsonDeSerializeUnknownNameAttriubte>() != null)
                                    {
                                        onUnknownName = (UnknownDeSerialize)Delegate.CreateDelegate(typeof(UnknownDeSerialize), methodInfo);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
