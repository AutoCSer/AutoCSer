using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    internal static partial class SerializeMethodCache
    {
        /// <summary>
        /// 未知类型序列化调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> memberMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumByteArrayMemberMethod = typeof(Serializer).GetMethod("enumByteArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumSByteArrayMemberMethod = typeof(Serializer).GetMethod("enumSByteArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumShortArrayMemberMethod = typeof(Serializer).GetMethod("enumShortArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumUShortArrayMemberMethod = typeof(Serializer).GetMethod("enumUShortArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumIntArrayMemberMethod = typeof(Serializer).GetMethod("enumIntArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumUIntArrayMemberMethod = typeof(Serializer).GetMethod("enumUIntArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumLongArrayMemberMethod = typeof(Serializer).GetMethod("enumLongArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumULongArrayMemberMethod = typeof(Serializer).GetMethod("enumULongArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo nullableArrayMemberMethod = typeof(Serializer).GetMethod("nullableArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo structArrayMemberMethod = typeof(Serializer).GetMethod("structArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //private static readonly MethodInfo arrayMemberMethod = typeof(Serializer).GetMethod("arrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumByteMemberMethod = typeof(Serializer).GetMethod("enumByteMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumSByteMemberMethod = typeof(Serializer).GetMethod("enumSByteMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumShortMemberMethod = typeof(Serializer).GetMethod("enumShortMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumUShortMemberMethod = typeof(Serializer).GetMethod("enumUShortMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumIntMemberMethod = typeof(Serializer).GetMethod("enumIntMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumUIntMemberMethod = typeof(Serializer).GetMethod("enumUIntMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumLongMemberMethod = typeof(Serializer).GetMethod("enumLongMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 枚举转换函数信息
        ///// </summary>
        //private static readonly MethodInfo enumULongMemberMethod = typeof(Serializer).GetMethod("enumULongMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典序列化函数信息
        ///// </summary>
        //private static readonly MethodInfo dictionaryMemberMethod = typeof(Serializer).GetMethod("dictionaryMember", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 对象序列化函数信息
        ///// </summary>
        //private static readonly MethodInfo nullableMemberSerializeMethod = typeof(Serializer).GetMethod("nullableMemberSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 对象序列化函数信息
        ///// </summary>
        //private static readonly MethodInfo structSerializeMethod = typeof(Serializer).GetMethod("structSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 引用类型成员序列化函数信息
        ///// </summary>
        //private static readonly MethodInfo memberClassSerializeMethod = typeof(Serializer).GetMethod("MemberClassSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 未知类型枚举序列化委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>未知类型序列化委托调用函数信息</returns>
        public static MethodInfo GetMember(Type type)
        {
            MethodInfo method;
            if (memberMethods.TryGetValue(type, out method)) return method;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                if (elementType.IsValueType)
                {
                    if (elementType.IsEnum)
                    {
                        Type enumType = System.Enum.GetUnderlyingType(elementType);
                        if (enumType == typeof(uint)) method = GenericType.Get(elementType).BinarySerializeEnumUIntArrayMemberMethod;// enumUIntArrayMemberMethod;
                        else if (enumType == typeof(byte)) method = GenericType.Get(elementType).BinarySerializeEnumByteArrayMemberMethod;// enumByteArrayMemberMethod;
                        else if (enumType == typeof(ulong)) method = GenericType.Get(elementType).BinarySerializeEnumULongArrayMemberMethod;// enumULongArrayMemberMethod;
                        else if (enumType == typeof(ushort)) method = GenericType.Get(elementType).BinarySerializeEnumUShortArrayMemberMethod;// enumUShortArrayMemberMethod;
                        else if (enumType == typeof(long)) method = GenericType.Get(elementType).BinarySerializeEnumLongArrayMemberMethod;// enumLongArrayMemberMethod;
                        else if (enumType == typeof(short)) method = GenericType.Get(elementType).BinarySerializeEnumShortArrayMemberMethod;// enumShortArrayMemberMethod;
                        else if (enumType == typeof(sbyte)) method = GenericType.Get(elementType).BinarySerializeEnumSByteArrayMemberMethod;// enumSByteArrayMemberMethod;
                        else method = GenericType.Get(elementType).BinarySerializeEnumIntArrayMemberMethod;// enumIntArrayMemberMethod;
                        //method = method.MakeGenericMethod(elementType);
                    }
                    else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        //method = nullableArrayMemberMethod.MakeGenericMethod(elementType.GetGenericArguments());
                        method = StructGenericType.Get(elementType.GetGenericArguments()[0]).BinarySerializeNullableArrayMemberMethod;
                    }
                    //else method = structArrayMemberMethod.MakeGenericMethod(elementType);
                    else method = GenericType.Get(elementType).BinarySerializeStructArrayMemberMethod;
                }
                //else method = arrayMemberMethod.MakeGenericMethod(elementType);
                else method = ClassGenericType.Get(elementType).BinarySerializeArrayMemberMethod;
            }
            else if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(uint)) method = GenericType.Get(type).BinarySerializeEnumUIntMemberMethod;// enumUIntMemberMethod;
                else if (enumType == typeof(byte)) method = GenericType.Get(type).BinarySerializeEnumByteMemberMethod;// enumByteMemberMethod;
                else if (enumType == typeof(ulong)) method = GenericType.Get(type).BinarySerializeEnumULongMemberMethod;// enumULongMemberMethod;
                else if (enumType == typeof(ushort)) method = GenericType.Get(type).BinarySerializeEnumUShortMemberMethod;// enumUShortMemberMethod;
                else if (enumType == typeof(long)) method = GenericType.Get(type).BinarySerializeEnumLongMemberMethod;// enumLongMemberMethod;
                else if (enumType == typeof(short)) method = GenericType.Get(type).BinarySerializeEnumShortMemberMethod;// enumShortMemberMethod;
                else if (enumType == typeof(sbyte)) method = GenericType.Get(type).BinarySerializeEnumSByteMemberMethod;// enumSByteMemberMethod;
                else method = GenericType.Get(type).BinarySerializeEnumIntMemberMethod;// enumIntMemberMethod;
                //method = method.MakeGenericMethod(type);
            }
            else
            {
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    if (genericType == typeof(Dictionary<,>) || genericType == typeof(SortedDictionary<,>) || genericType == typeof(SortedList<,>))
                    {
                        Type[] parameterTypes = type.GetGenericArguments();
                        //method = dictionaryMemberMethod.MakeGenericMethod(type, parameterTypes[0], parameterTypes[1]);
                        method = DictionaryGenericType3.Get(type, parameterTypes[0], parameterTypes[1]).BinarySerializeDictionaryMemberMethod;
                    }
                    else if (genericType == typeof(Nullable<>))
                    {
                        //method = nullableMemberSerializeMethod.MakeGenericMethod(type.GetGenericArguments());
                        method = StructGenericType.Get(type.GetGenericArguments()[0]).BinarySerializeNullableMemberMethod;
                    }
                    else if (genericType == typeof(KeyValuePair<,>))
                    {
                        //method = KeyValuePairSerializeMethod.MakeGenericMethod(type.GetGenericArguments());
                        method = GenericType2.Get(type.GetGenericArguments()).BinarySerializeKeyValuePairMethod;
                    }
                }
                if (method == null)
                {
                    //if (type.IsValueType) method = structSerializeMethod.MakeGenericMethod(type);
                    //else method = memberClassSerializeMethod.MakeGenericMethod(type);
                    if (type.IsValueType) method = StructGenericType.Get(type).BinarySerializeStructMethod;
                    else method = ClassGenericType.Get(type).BinarySerializeMemberClassMethod;
                }
            }
            memberMethods.Set(type, method);
            return method;
        }

        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fieldIndexs"></param>
        /// <param name="isJson"></param>
        /// <param name="memberCountVerify"></param>
        /// <returns>字段成员集合</returns>
        public static Fields<FieldSize> GetFields(FieldIndex[] fieldIndexs, bool isJson, out int memberCountVerify)
        {
            LeftArray<FieldSize> fixedFields = new LeftArray<FieldSize>(fieldIndexs.Length), fields = new LeftArray<FieldSize>(fieldIndexs.Length);
            LeftArray<FieldIndex> jsonFields = new LeftArray<FieldIndex>();
            int fixedSize = 0;
            foreach (FieldIndex field in fieldIndexs)
            {
                Type fieldType = field.Member.FieldType;
                if (!fieldType.IsPointer && (!fieldType.IsArray || fieldType.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(fieldType))
                {
                    SerializeMemberAttribute memberAttribute = field.GetAttribute<SerializeMemberAttribute>(true);
                    if (memberAttribute == null || (memberAttribute.IsSetup && !memberAttribute.IsRemove))
                    {
                        if (memberAttribute != null && memberAttribute.GetIsJson) jsonFields.Add(field);
                        else
                        {
                            FieldSize value = new FieldSize(field);
                            if (value.FixedSize == 0) fields.Add(value);
                            else
                            {
                                fixedFields.Add(value);
                                fixedSize += value.FixedSize;
                            }
                        }
                    }
                }
            }
            return new Fields<FieldSize>(ref fixedFields, ref fields, ref jsonFields, fixedSize, isJson, out memberCountVerify);
        }
        /// <summary>
        /// 对象序列化函数信息
        /// </summary>
        internal static readonly MethodInfo BaseSerializeMethod = typeof(Serializer).GetMethod("baseSerialize", BindingFlags.Static | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo EnumByteArrayMethod = typeof(Serializer).GetMethod("enumByteArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo EnumSByteArrayMethod = typeof(Serializer).GetMethod("enumSByteArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo EnumShortArrayMethod = typeof(Serializer).GetMethod("enumShortArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo EnumUShortArrayMethod = typeof(Serializer).GetMethod("enumUShortArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo EnumIntArrayMethod = typeof(Serializer).GetMethod("enumIntArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo EnumUIntArrayMethod = typeof(Serializer).GetMethod("enumUIntArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo EnumLongArrayMethod = typeof(Serializer).GetMethod("enumLongArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo EnumULongArrayMethod = typeof(Serializer).GetMethod("enumULongArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo NullableArrayMethod = typeof(Serializer).GetMethod("nullableArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructArrayMethod = typeof(Serializer).GetMethod("structArray", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组转换函数信息
        ///// </summary>
        //internal static readonly MethodInfo ArrayMethod = typeof(Serializer).GetMethod("array", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 数组序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo LeftArraySerializeMethod = typeof(Serializer).GetMethod("leftArraySerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo DictionarySerializeMethod = typeof(Serializer).GetMethod("dictionarySerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 对象序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo NullableSerializeMethod = typeof(Serializer).GetMethod("nullableSerialize", BindingFlags.Static | BindingFlags.NonPublic);
        ///// <summary>
        ///// 字典序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo KeyValuePairSerializeMethod = typeof(Serializer).GetMethod("keyValuePairSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructDictionaryMethod = typeof(Serializer).GetMethod("structDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassDictionaryMethod = typeof(Serializer).GetMethod("classDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructEnumByteCollectionMethod = typeof(Serializer).GetMethod("structEnumByteCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassEnumByteCollectionMethod = typeof(Serializer).GetMethod("classEnumByteCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructEnumSByteCollectionMethod = typeof(Serializer).GetMethod("structEnumSByteCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassEnumSByteCollectionMethod = typeof(Serializer).GetMethod("classEnumSByteCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructEnumShortCollectionMethod = typeof(Serializer).GetMethod("structEnumShortCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassEnumShortCollectionMethod = typeof(Serializer).GetMethod("classEnumShortCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructEnumUShortCollectionMethod = typeof(Serializer).GetMethod("structEnumUShortCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassEnumUShortCollectionMethod = typeof(Serializer).GetMethod("classEnumUShortCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructEnumIntCollectionMethod = typeof(Serializer).GetMethod("structEnumIntCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassEnumIntCollectionMethod = typeof(Serializer).GetMethod("classEnumIntCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructEnumUIntCollectionMethod = typeof(Serializer).GetMethod("structEnumUIntCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassEnumUIntCollectionMethod = typeof(Serializer).GetMethod("classEnumUIntCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructEnumLongCollectionMethod = typeof(Serializer).GetMethod("structEnumLongCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassEnumLongCollectionMethod = typeof(Serializer).GetMethod("classEnumLongCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructEnumULongCollectionMethod = typeof(Serializer).GetMethod("structEnumULongCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassEnumULongCollectionMethod = typeof(Serializer).GetMethod("classEnumULongCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo StructCollectionMethod = typeof(Serializer).GetMethod("structCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 集合序列化函数信息
        ///// </summary>
        //internal static readonly MethodInfo ClassCollectionMethod = typeof(Serializer).GetMethod("classCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        ///// <summary>
        ///// 真实类型序列化函数集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, Action<Serializer, object>> realSerializers = new AutoCSer.Threading.LockDictionary<Type, Action<Serializer, object>>();
        ///// <summary>
        ///// 真实类型序列化函数信息
        ///// </summary>
        //private static readonly MethodInfo realTypeObjectMethod = typeof(Serializer).GetMethod("realTypeObject", BindingFlags.Static | BindingFlags.NonPublic);
        ///// <summary>
        ///// 获取真实类型序列化函数
        ///// </summary>
        ///// <param name="type">数组类型</param>
        ///// <returns>真实类型序列化函数</returns>
        //public static Action<Serializer, object> GetRealSerializer(Type type)
        //{
        //    Action<Serializer, object> method;
        //    if (realSerializers.TryGetValue(type, out method)) return method;
        //    method = (Action<Serializer, object>)Delegate.CreateDelegate(typeof(Action<Serializer, object>), realTypeObjectMethod.MakeGenericMethod(type));
        //    realSerializers.Set(type, method);
        //    return method;
        //}

        /// <summary>
        /// 获取自定义序列化函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isSerializer"></param>
        /// <returns></returns>
        public static MethodInfo GetCustom(Type type, bool isSerializer)
        {
            MethodInfo serializeMethod = null, deSerializeMethod = null;
            if (type.IsValueType)
            {
                foreach (AutoCSer.Metadata.AttributeMethod method in AutoCSer.Metadata.AttributeMethod.Get(type))
                {
                    if (method.Method.ReturnType == typeof(void) && method.GetAttribute<SerializeCustomAttribute>() != null)
                    {
                        ParameterInfo[] parameters = method.Method.GetParameters();
                        if (parameters.Length == 1)
                        {
                            if (parameters[0].ParameterType == typeof(Serializer))
                            {
                                if (deSerializeMethod != null) return isSerializer ? method.Method : deSerializeMethod;
                                serializeMethod = method.Method;
                            }
                            else if (parameters[0].ParameterType == typeof(DeSerializer))
                            {
                                if (serializeMethod != null) return isSerializer ? serializeMethod : method.Method;
                                deSerializeMethod = method.Method;
                            }
                        }
                    }
                }
            }
            else
            {
                Type refType = type.MakeByRefType();
                foreach (AutoCSer.Metadata.AttributeMethod method in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                {
                    if (method.Method.ReturnType == typeof(void) && method.GetAttribute<SerializeCustomAttribute>() != null)
                    {
                        ParameterInfo[] parameters = method.Method.GetParameters();
                        if (parameters.Length == 2)
                        {
                            if (parameters[0].ParameterType == typeof(Serializer))
                            {
                                if (parameters[1].ParameterType == type)
                                {
                                    if (deSerializeMethod != null) return isSerializer ? method.Method : deSerializeMethod;
                                    serializeMethod = method.Method;
                                }
                            }
                            else if (parameters[0].ParameterType == typeof(DeSerializer))
                            {
                                if (parameters[1].ParameterType == refType)
                                {
                                    if (serializeMethod != null) return isSerializer ? serializeMethod : method.Method;
                                    deSerializeMethod = method.Method;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        ///// <summary>
        ///// 是否支持循环引用处理
        ///// </summary>
        ///// <typeparam name="valueType"></typeparam>
        ///// <returns></returns>
        //private static bool isReferenceMember<valueType>()
        //{
        //    return TypeSerializer<valueType>.IsReferenceMember;
        //}
        ///// <summary>
        ///// 是否支持循环引用处理函数信息
        ///// </summary>
        //private static readonly MethodInfo isReferenceMemberMethod = typeof(SerializeMethodCache).GetMethod("isReferenceMember", BindingFlags.Static | BindingFlags.NonPublic);
        ///// <summary>
        ///// 是否支持循环引用处理集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<Type, bool> isReferenceMembers = new AutoCSer.Threading.LockDictionary<Type, bool>();
        /// <summary>
        /// 是否支持循环引用处理
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsReferenceMember(Type type)
        {
            return GenericType.Get(type).BinarySerializeIsReferenceMember;
            //bool isReferenceMember;
            //if (isReferenceMembers.TryGetValue(type, out isReferenceMember)) return isReferenceMember;
            //isReferenceMembers.Set(type, isReferenceMember = (bool)isReferenceMemberMethod.MakeGenericMethod(type).Invoke(null, null));
            //return isReferenceMember;
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            memberMethods.Clear();
            //realSerializers.Clear();
            //isReferenceMembers.Clear();
        }
        static SerializeMethodCache()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
