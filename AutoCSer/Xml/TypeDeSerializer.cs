using System;
using AutoCSer.Metadata;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Xml
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
            public XmlDeSerializer.DeSerializeDelegate<T> TryDeSerialize;
            /// <summary>
            /// 集合子节点名称
            /// </summary>
            public string ItemName;
            /// <summary>
            /// 成员位图索引
            /// </summary>
            public int MemberMapIndex;
            ///// <summary>
            ///// 成员选择
            ///// </summary>
            //public AutoCSer.Metadata.MemberFilters MemberFilter;
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deSerializer">XML解析器</param>
            /// <param name="value">目标数据</param>
            /// <returns>是否存在下一个数据</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public int Call(XmlDeSerializer deSerializer, ref T value)
            {
                //deSerializer.ItemName = ItemName;
                //if ((deSerializer.Config.MemberFilter & MemberFilter) == MemberFilter)
                //{
                //    TryDeSerialize(deSerializer, ref value);
                //    return deSerializer.State == DeSerializeState.Success ? 1 : 0;
                //}
                //return deSerializer.IgnoreValue();
                deSerializer.ItemName = ItemName;
                TryDeSerialize(deSerializer, ref value);
                return deSerializer.State == DeSerializeState.Success ? 1 : 0;
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deSerializer">XML解析器</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">目标数据</param>
            /// <returns>是否存在下一个数据</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public int Call(XmlDeSerializer deSerializer, MemberMap memberMap, ref T value)
            {
                //deSerializer.ItemName = ItemName;
                //if ((deSerializer.Config.MemberFilter & MemberFilter) == MemberFilter)
                //{
                //    TryDeSerialize(deSerializer, ref value);
                //    if (deSerializer.State == DeSerializeState.Success)
                //    {
                //        memberMap.SetMember(MemberMapIndex);
                //        return 1;
                //    }
                //    return 0;
                //}
                //return deSerializer.IgnoreValue();
                deSerializer.ItemName = ItemName;
                TryDeSerialize(deSerializer, ref value);
                if (deSerializer.State == DeSerializeState.Success)
                {
                    memberMap.SetMember(MemberMapIndex);
                    return 1;
                }
                return 0;
            }
        }
        /// <summary>
        /// 未知名称解析委托
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="value">目标数据</param>
        /// <param name="name">节点名称</param>
        internal delegate bool UnknownDeSerialize(XmlDeSerializer deSerializer, ref T value, ref AutoCSer.Memory.Pointer name);
        /// <summary>
        /// 解析委托
        /// </summary>
        internal static readonly XmlDeSerializer.DeSerializeDelegate<T> DefaultDeSerializer;
        /// <summary>
        /// 成员解析器集合
        /// </summary>
        private static readonly TryDeSerializeFilter[] memberDeSerializers;
        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        private static readonly AutoCSer.Memory.Pointer memberSearcher;
        /// <summary>
        /// 默认顺序成员名称数据
        /// </summary>
        private static readonly AutoCSer.Memory.Pointer memberNames;
        /// <summary>
        /// 未知名称节点处理
        /// </summary>
        private static readonly UnknownDeSerialize onUnknownName;
        /// <summary>
        /// XML解析类型配置
        /// </summary>
        private static readonly XmlSerializeAttribute attribute;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;
        /// <summary>
        /// 是否匿名类型
        /// </summary>
        private static readonly bool isAnonymousType;

        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerialize(XmlDeSerializer deSerializer, ref T value)
        {
            if (DefaultDeSerializer == null)
            {
                if (isValueType) DeSerializeMembers(deSerializer, ref value);
                else deSerializeClass(deSerializer, ref value);
            }
            else DefaultDeSerializer(deSerializer, ref value);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void deSerializeClass(XmlDeSerializer deSerializer, ref T value)
        {
            if (value == null)
            {
                if (AutoCSer.Metadata.DefaultConstructor<T>.Type == DefaultConstructorType.None)
                {
                    deSerializer.CheckNoConstructor(ref value, isAnonymousType);
                    if (value == null) return;
                }
                else value = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
            }
            else if (isAnonymousType) deSerializer.SetAnonymousType(value);
            DeSerializeMembers(deSerializer, ref value);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerializeClass(XmlDeSerializer deSerializer, ref T value)
        {
            if (DefaultDeSerializer == null) deSerializeClass(deSerializer, ref value);
            else DefaultDeSerializer(deSerializer, ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void DeSerializeStruct(XmlDeSerializer deSerializer, ref T value)
        {
            if (DefaultDeSerializer == null) DeSerializeMembers(deSerializer, ref value);
            else DefaultDeSerializer(deSerializer, ref value);
        }
        /// <summary>
        /// 数据成员解析
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="value">目标数据</param>
        internal static void DeSerializeMembers(XmlDeSerializer deSerializer, ref T value)
        {
            byte* names = memberNames.Byte;
            DeSerializeConfig config = deSerializer.Config;
            MemberMap memberMap = deSerializer.MemberMap;
            int index = 0;
            if (memberMap == null)
            {
                while (deSerializer.IsName(names, ref index))
                {
                    if (index == -1) return;
                    memberDeSerializers[index].Call(deSerializer, ref value);
                    if (deSerializer.State != DeSerializeState.Success) return;
                    if (deSerializer.IsNameEnd(names) == 0)
                    {
                        if (deSerializer.CheckNameEnd((char*)(names + (sizeof(short) + sizeof(char))), (*(short*)names >> 1) - 2) == 0) return;
                        break;
                    }
                    ++index;
                    names += *(short*)names + sizeof(short);
                }
                AutoCSer.StateSearcher.CharSearcher searcher = new AutoCSer.StateSearcher.CharSearcher(memberSearcher);
                AutoCSer.Memory.Pointer name = default(AutoCSer.Memory.Pointer);
                byte isTagEnd = 0;
                if (onUnknownName == null)
                {
                    do
                    {
                        if ((name.Data = deSerializer.GetName(ref name.ByteSize, ref isTagEnd)) == null) return;
                        if (isTagEnd == 0)
                        {
                            if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                            {
                                if (deSerializer.IgnoreValue() == 0) return;
                            }
                            else if (memberDeSerializers[index].Call(deSerializer, ref value) == 0) return;
                            if (deSerializer.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                        }
                    }
                    while (true);
                }
                else
                {
                    do
                    {
                        if ((name.Data = deSerializer.GetName(ref name.ByteSize, ref isTagEnd)) == null) return;
                        if (isTagEnd == 0)
                        {
                            if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                            {
                                name.ByteSize <<= 1;
                                if (onUnknownName(deSerializer, ref value, ref name))
                                {
                                    if (deSerializer.State != DeSerializeState.Success) return;
                                }
                                else
                                {
                                    if (deSerializer.State == DeSerializeState.Success) deSerializer.State = DeSerializeState.UnknownNameError;
                                    return;
                                }
                            }
                            else if (memberDeSerializers[index].Call(deSerializer, ref value) == 0) return;
                            if (deSerializer.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                        }
                    }
                    while (true);
                }
            }
            else if (object.ReferenceEquals(memberMap.Type, MemberMap<T>.MemberMapType))
            {
                memberMap.Empty();
                deSerializer.MemberMap = null;
                while (deSerializer.IsName(names, ref index))
                {
                    if (index == -1) return;
                    memberDeSerializers[index].Call(deSerializer, memberMap, ref value);
                    if (deSerializer.State != DeSerializeState.Success) return;
                    if (deSerializer.IsNameEnd(names) == 0)
                    {
                        if (deSerializer.CheckNameEnd((char*)(names + (sizeof(short) + sizeof(char))), (*(short*)names >> 1) - 2) == 0) return;
                        break;
                    }
                    ++index;
                    names += *(short*)names + sizeof(short);
                }
                AutoCSer.StateSearcher.CharSearcher searcher = new AutoCSer.StateSearcher.CharSearcher(memberSearcher);
                AutoCSer.Memory.Pointer name = default(AutoCSer.Memory.Pointer);
                byte isTagEnd = 0;
                try
                {
                    if (onUnknownName == null)
                    {
                        do
                        {
                            if ((name.Data = deSerializer.GetName(ref name.ByteSize, ref isTagEnd)) == null) return;
                            if (isTagEnd == 0)
                            {
                                if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                                {
                                    if (deSerializer.IgnoreValue() == 0) return;
                                }
                                else if (memberDeSerializers[index].Call(deSerializer, memberMap, ref value) == 0) return;
                                if (deSerializer.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                            }
                        }
                        while (true);
                    }
                    else
                    {
                        do
                        {
                            if ((name.Data = deSerializer.GetName(ref name.ByteSize, ref isTagEnd)) == null) return;
                            if (isTagEnd == 0)
                            {
                                if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                                {
                                    name.ByteSize <<= 1;
                                    if (onUnknownName(deSerializer, ref value, ref name))
                                    {
                                        if (deSerializer.State != DeSerializeState.Success) return;
                                    }
                                    else
                                    {
                                        if (deSerializer.State == DeSerializeState.Success) deSerializer.State = DeSerializeState.UnknownNameError;
                                        return;
                                    }
                                }
                                else if (memberDeSerializers[index].Call(deSerializer, memberMap, ref value) == 0) return;
                                if (deSerializer.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                            }
                        }
                        while (true);
                    }
                }
                finally { deSerializer.MemberMap = memberMap; }
            }
            else deSerializer.State = DeSerializeState.MemberMap;
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="values">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Array(XmlDeSerializer deSerializer, ref T[] values)
        {
            int count = ArrayIndex(deSerializer, ref values);
            if (count != -1 && count != values.Length) System.Array.Resize(ref values, count);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="values">目标数据</param>
        /// <returns>数据数量,-1表示失败</returns>
        internal static int ArrayIndex(XmlDeSerializer deSerializer, ref T[] values)
        {
            if (values == null) values = EmptyArray<T>.Array;
            string arrayItemName = deSerializer.ArrayItemName;
            AutoCSer.Memory.Pointer name = default(AutoCSer.Memory.Pointer);
            int index = 0;
            byte isTagEnd = 0;
            fixed (char* itemFixed = arrayItemName)
            {
                do
                {
                    if ((name.Data = deSerializer.GetName(ref name.ByteSize, ref isTagEnd)) == null) break;
                    if (isTagEnd == 0)
                    {
                        if (arrayItemName.Length != name.ByteSize || !AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)itemFixed, name.Byte, name.ByteSize << 1))
                        {
                            deSerializer.State = DeSerializeState.NotArrayItem;
                            return -1;
                        }
                        if (index == values.Length)
                        {
                            T value = default(T);
                            if (deSerializer.IsArrayItem(itemFixed, arrayItemName.Length) != 0)
                            {
                                DeSerialize(deSerializer, ref value);
                                if (deSerializer.State != DeSerializeState.Success) return -1;
                                if (deSerializer.CheckNameEnd(itemFixed, name.ByteSize) == 0) break;
                            }
                            values = values.copyNew(index == 0 ? deSerializer.Config.NewArraySize : (index << 1));
                            values[index++] = value;
                        }
                        else
                        {
                            if (deSerializer.IsArrayItem(itemFixed, arrayItemName.Length) != 0)
                            {
                                DeSerialize(deSerializer, ref values[index]);
                                if (deSerializer.State != DeSerializeState.Success) return -1;
                                if (deSerializer.CheckNameEnd(itemFixed, name.ByteSize) == 0) break;
                            }
                            ++index;
                        }
                    }
                    else
                    {
                        if (index == values.Length) values = values.copyNew(index == 0 ? deSerializer.Config.NewArraySize : (index << 1));
                        values[index++] = default(T);
                    }
                }
                while (true);
            }
            return deSerializer.State == DeSerializeState.Success ? index : -1;
        }

        /// <summary>
        /// 不支持基元类型解析
        /// </summary>
        /// <param name="deSerializer">XML解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void notSupport(XmlDeSerializer deSerializer, ref T value)
        {
            deSerializer.State = DeSerializeState.NotSupport;
        }
        /// <summary>
        /// 包装处理
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void unbox(XmlDeSerializer deSerializer, ref T value)
        {
            if (deSerializer.IsValue() != 0) memberDeSerializers[0].TryDeSerialize(deSerializer, ref value);
        }

        static TypeDeSerializer()
        {
            Type type = typeof(T);
            MethodInfo methodInfo = XmlDeSerializer.GetDeSerializeMethod(type);
            if (methodInfo != null)
            {
                DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)Delegate.CreateDelegate(typeof(XmlDeSerializer.DeSerializeDelegate<T>), methodInfo);
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1) DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)GenericType.Get(type.GetElementType()).XmlDeSerializeArrayMethod;
                else DefaultDeSerializer = notSupport;
                return;
            }
            if (type.IsEnum)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false)) DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)EnumGenericType.Get(type).XmlDeSerializeEnumFlagsDelegate;
                else DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)EnumGenericType.Get(type).XmlDeSerializeEnumDelegate;
                return;
            }
            if (type.IsInterface || type.IsPointer || typeof(Delegate).IsAssignableFrom(type))
            {
                DefaultDeSerializer = notSupport;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    Type[] parameterTypes = type.GetGenericArguments();
                    DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)(parameterTypes[0].IsEnum ? StructGenericType.Get(parameterTypes[0]).XmlDeSerializeNullableEnumMethod : StructGenericType.Get(parameterTypes[0]).XmlDeSerializeNullableMethod);
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)GenericType2.Get(type.GetGenericArguments()).XmlDeSerializeKeyValuePairMethod;
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
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomXmlDeSerializer", null, new Type[] { typeof(XmlDeSerializer), type.MakeByRefType() }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)dynamicMethod.CreateDelegate(typeof(XmlDeSerializer.DeSerializeDelegate<T>));
#endif
                }
                else DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)Delegate.CreateDelegate(typeof(XmlDeSerializer.DeSerializeDelegate<T>), methodInfo);
            }
            else
            {
                Type attributeType;
                attribute = type.customAttribute<XmlSerializeAttribute>(out attributeType) ?? XmlSerializer.AllMemberAttribute;
                if ((methodInfo = DeSerializeMethodCache.GetIEnumerableConstructor(type)) != null)
                {
                    DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)Delegate.CreateDelegate(typeof(XmlDeSerializer.DeSerializeDelegate<T>), methodInfo);
                }
                else
                {
                    if (type.IsValueType) isValueType = true;
                    else if (attribute != XmlSerializer.AllMemberAttribute && attributeType != type)
                    {
                        for (Type baseType = type.BaseType; baseType != typeof(object); baseType = baseType.BaseType)
                        {
                            XmlSerializeAttribute baseAttribute = baseType.customAttribute<XmlSerializeAttribute>();
                            if (baseAttribute != null)
                            {
                                if (baseAttribute.IsBaseType)
                                {
                                    methodInfo = DeSerializeMethodCache.BaseDeSerializeMethod.MakeGenericMethod(baseType, type);
                                    DefaultDeSerializer = (XmlDeSerializer.DeSerializeDelegate<T>)Delegate.CreateDelegate(typeof(XmlDeSerializer.DeSerializeDelegate<T>), methodInfo);
                                    return;
                                }
                                break;
                            }
                        }
                    }
                    if (type.IsValueType)
                    {
                        foreach (AutoCSer.Metadata.AttributeMethod attributeMethod in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                        {
                            if (attributeMethod.Method.ReturnType == typeof(bool))
                            {
                                ParameterInfo[] parameters = attributeMethod.Method.GetParameters();
                                if (parameters.Length == 2 && parameters[0].ParameterType == typeof(XmlDeSerializer) && parameters[1].ParameterType == Emit.Pub.PointerSizeRefType)
                                {
                                    if (attributeMethod.GetAttribute<UnknownNameAttribute>() != null)
                                    {
#if NOJIT
                                        onUnknownName = new UnknownDeSerializer(methodInfo).DeSerialize;
#else
                                        DynamicMethod dynamicMethod = new DynamicMethod("XmlUnknownDeSerialize", null, new Type[] { typeof(XmlDeSerializer), type.MakeByRefType(), Emit.Pub.PointerSizeRefType }, type, true);
                                        ILGenerator generator = dynamicMethod.GetILGenerator();
                                        generator.Emit(OpCodes.Ldarg_1);
                                        generator.Emit(OpCodes.Ldarg_0);
                                        generator.Emit(OpCodes.Ldarg_2);
                                        generator.call(methodInfo);
                                        generator.Emit(OpCodes.Ret);
                                        onUnknownName = (UnknownDeSerialize)dynamicMethod.CreateDelegate(typeof(UnknownDeSerialize));
#endif
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Type refType = type.MakeByRefType();
                        foreach (AutoCSer.Metadata.AttributeMethod attributeMethod in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                        {
                            if (attributeMethod.Method.ReturnType == typeof(bool))
                            {
                                ParameterInfo[] parameters = attributeMethod.Method.GetParameters();
                                if (parameters.Length == 3 && parameters[0].ParameterType == typeof(XmlDeSerializer) && parameters[1].ParameterType == refType && parameters[2].ParameterType == Emit.Pub.PointerSizeRefType)
                                {
                                    if (attributeMethod.GetAttribute<UnknownNameAttribute>() != null)
                                    {
                                        onUnknownName = (UnknownDeSerialize)Delegate.CreateDelegate(typeof(UnknownDeSerialize), attributeMethod.Method);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    FieldIndex defaultMember = null;
                    LeftArray<KeyValue<FieldIndex, XmlSerializeMemberAttribute>> fields = SerializeMethodCache.GetFields(MemberIndexGroup<T>.GetFields(attribute.MemberFilters), attribute);
                    LeftArray<PropertyMethod> properties = DeSerializeMethodCache.GetProperties(MemberIndexGroup<T>.GetProperties(attribute.MemberFilters), attribute);
                    bool isBox = false;
                    if (type.IsValueType && fields.Length + properties.Length == 1)
                    {
                        BoxSerializeAttribute boxSerialize = AutoCSer.Metadata.TypeAttribute.GetAttribute<BoxSerializeAttribute>(type);
                        if (boxSerialize != null && boxSerialize.IsXml)
                        {
                            isBox = true;
                            defaultMember = null;
                        }
                    }
                    TryDeSerializeFilter[] deSerializers = new TryDeSerializeFilter[fields.Length + properties.Length + (defaultMember == null ? 0 : 1)];
                    //memberMap.type memberMapType = memberMap<valueType>.TypeInfo;
                    string[] names = isBox ? null : new string[deSerializers.Length];
                    int index = 0, nameLength = 0, maxNameLength = 0;
                    foreach (KeyValue<FieldIndex, XmlSerializeMemberAttribute> member in fields)
                    {
                        TryDeSerializeFilter tryDeSerialize = deSerializers[index] = new TryDeSerializeFilter
                        {
#if NOJIT
                            TryDeSerialize = new FieldDeSerializer(member.Key.Member).DeSerializer(),
#else
                            TryDeSerialize = (XmlDeSerializer.DeSerializeDelegate<T>)DeSerializeMethodCache.CreateDynamicMethod(type, member.Key.Member).CreateDelegate(typeof(XmlDeSerializer.DeSerializeDelegate<T>)),
#endif
                            ItemName = member.Value == null ? null : member.Value.ItemName,
                            MemberMapIndex = member.Key.MemberIndex,
                            //MemberFilter = member.Key.Member.IsPublic ? MemberFilters.PublicInstanceField : MemberFilters.NonPublicInstanceField
                        };
                        if (!isBox)
                        {
                            string name = member.Key.AnonymousName;
                            if (name.Length > maxNameLength) maxNameLength = name.Length;
                            nameLength += (names[index++] = name).Length;
                            if (member.Key == defaultMember)
                            {
                                deSerializers[deSerializers.Length - 1] = tryDeSerialize;
                                names[deSerializers.Length - 1] = string.Empty;
                            }
                        }
                    }
                    foreach (PropertyMethod member in properties)
                    {
                        deSerializers[index] = new TryDeSerializeFilter
                        {
#if NOJIT
                            TryDeSerialize = new PropertyDeSerializer(member.Property.Member).DeSerializer(),
#else
                            TryDeSerialize = (XmlDeSerializer.DeSerializeDelegate<T>)DeSerializeMethodCache.CreateDynamicMethod(type, member.Property.Member, member.Method).CreateDelegate(typeof(XmlDeSerializer.DeSerializeDelegate<T>)),
#endif
                            ItemName = member.Attribute == null ? null : member.Attribute.ItemName,
                            MemberMapIndex = member.Property.MemberIndex,
                            //MemberFilter = member.Method.IsPublic ? MemberFilters.PublicInstanceProperty : MemberFilters.NonPublicInstanceProperty
                        };
                        if (!isBox)
                        {
                            if (member.Property.Member.Name.Length > maxNameLength) maxNameLength = member.Property.Member.Name.Length;
                            nameLength += (names[index++] = member.Property.Member.Name).Length;
                        }
                    }
                    memberDeSerializers = deSerializers;
                    if (isBox) DefaultDeSerializer = unbox;
                    else
                    {
                        if (type.Name[0] == '<') isAnonymousType = true;
                        if (maxNameLength > (short.MaxValue >> 1) - 2 || nameLength == 0) memberNames = Unmanaged.NullByte8;
                        else
                        {
                            memberNames = Unmanaged.GetStaticPointer((nameLength + (names.Length - (defaultMember == null ? 0 : 1)) * 3 + 1) << 1, false);
                            byte* write = memberNames.Byte;
                            foreach (string name in names)
                            {
                                if (name.Length != 0)
                                {
                                    *(short*)write = (short)((name.Length + 2) * sizeof(char));
                                    *(char*)(write + sizeof(short)) = '<';
                                    fixed (char* nameFixed = name) AutoCSer.Extensions.StringExtension.SimpleCopyNotNull(nameFixed, (char*)(write + (sizeof(short) + sizeof(char))), name.Length);
                                    *(char*)(write += (sizeof(short) + sizeof(char)) + (name.Length << 1)) = '>';
                                    write += sizeof(char);
                                }
                            }
                            *(short*)write = 0;
                        }
                        if (type.IsGenericType) memberSearcher = DeSerializeMethodCache.GetGenericDefinitionMemberSearcher(type, names);
                        else memberSearcher = AutoCSer.StateSearcher.CharBuilder.Create(names, true);
                    }
                }
            }
        }
    }
}
