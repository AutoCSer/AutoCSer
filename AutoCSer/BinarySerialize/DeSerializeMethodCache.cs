using System;
using System.Reflection;
using System.Collections.Generic;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    internal static partial class DeSerializeMethodCache
    {
        /// <summary>
        /// 对象序列化函数信息
        /// </summary>
        private static readonly MethodInfo structDeSerializeMethod = typeof(DeSerializer).GetMethod("structDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 序列化接口函数信息
        /// </summary>
        private static readonly MethodInfo memberClassDeSerializeMethod = typeof(DeSerializer).GetMethod("memberClassDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumByteMemberMethod = typeof(DeSerializer).GetMethod("enumByteMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumSByteMemberMethod = typeof(DeSerializer).GetMethod("enumSByteMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumShortMemberMethod = typeof(DeSerializer).GetMethod("enumShortMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumUShortMemberMethod = typeof(DeSerializer).GetMethod("enumUShortMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumIntMethod = typeof(DeSerializer).GetMethod("EnumInt", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumUIntMethod = typeof(DeSerializer).GetMethod("EnumUInt", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumLongMethod = typeof(DeSerializer).GetMethod("EnumLong", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumULongMethod = typeof(DeSerializer).GetMethod("EnumULong", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 字典序列化函数信息
        /// </summary>
        private static readonly MethodInfo dictionaryMemberMethod = typeof(DeSerializer).GetMethod("dictionaryMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 对象序列化函数信息
        /// </summary>
        private static readonly MethodInfo nullableMemberDeSerializeMethod = typeof(DeSerializer).GetMethod("nullableMemberDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 字典序列化函数信息
        /// </summary>
        private static readonly MethodInfo sortedDictionaryMemberMethod = typeof(DeSerializer).GetMethod("sortedDictionaryMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 字典序列化函数信息
        /// </summary>
        private static readonly MethodInfo sortedListMemberMethod = typeof(DeSerializer).GetMethod("sortedListMember", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo nullableArrayMemberMethod = typeof(DeSerializer).GetMethod("nullableArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo structArrayMemberMethod = typeof(DeSerializer).GetMethod("structArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo arrayMemberMethod = typeof(DeSerializer).GetMethod("arrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumByteArrayMemberMethod = typeof(DeSerializer).GetMethod("enumByteArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumSByteArrayMemberMethod = typeof(DeSerializer).GetMethod("enumSByteArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumShortArrayMemberMethod = typeof(DeSerializer).GetMethod("enumShortArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumUShortArrayMemberMethod = typeof(DeSerializer).GetMethod("enumUShortArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumIntArrayMemberMethod = typeof(DeSerializer).GetMethod("enumIntArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumUIntArrayMemberMethod = typeof(DeSerializer).GetMethod("enumUIntArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumLongArrayMemberMethod = typeof(DeSerializer).GetMethod("enumLongArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumULongArrayMemberMethod = typeof(DeSerializer).GetMethod("enumULongArrayMember", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 未知类型反序列化调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> memberMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 未知类型枚举反序列化委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>未知类型反序列化委托调用函数信息</returns>
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
                        if (enumType == typeof(uint)) method = enumUIntArrayMemberMethod;
                        else if (enumType == typeof(byte)) method = enumByteArrayMemberMethod;
                        else if (enumType == typeof(ulong)) method = enumULongArrayMemberMethod;
                        else if (enumType == typeof(ushort)) method = enumUShortArrayMemberMethod;
                        else if (enumType == typeof(long)) method = enumLongArrayMemberMethod;
                        else if (enumType == typeof(short)) method = enumShortArrayMemberMethod;
                        else if (enumType == typeof(sbyte)) method = enumSByteArrayMemberMethod;
                        else method = enumIntArrayMemberMethod;
                        method = method.MakeGenericMethod(elementType);
                    }
                    else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        method = nullableArrayMemberMethod.MakeGenericMethod(elementType.GetGenericArguments());
                    }
                    else method = structArrayMemberMethod.MakeGenericMethod(elementType);
                }
                else method = arrayMemberMethod.MakeGenericMethod(elementType);
            }
            else if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(uint)) method = enumUIntMethod;
                else if (enumType == typeof(byte)) method = enumByteMemberMethod;
                else if (enumType == typeof(ulong)) method = enumULongMethod;
                else if (enumType == typeof(ushort)) method = enumUShortMemberMethod;
                else if (enumType == typeof(long)) method = enumLongMethod;
                else if (enumType == typeof(short)) method = enumShortMemberMethod;
                else if (enumType == typeof(sbyte)) method = enumSByteMemberMethod;
                else method = enumIntMethod;
                method = method.MakeGenericMethod(type);
            }
            else
            {
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    if (genericType == typeof(Dictionary<,>))
                    {
                        method = dictionaryMemberMethod.MakeGenericMethod(type.GetGenericArguments());
                    }
                    else if (genericType == typeof(Nullable<>))
                    {
                        method = nullableMemberDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments());
                    }
                    else if (genericType == typeof(KeyValuePair<,>))
                    {
                        method = KeyValuePairDeSerializeMethod.MakeGenericMethod(type.GetGenericArguments());
                    }
                    else if (genericType == typeof(SortedDictionary<,>))
                    {
                        method = sortedDictionaryMemberMethod.MakeGenericMethod(type.GetGenericArguments());
                    }
                    else if (genericType == typeof(SortedList<,>))
                    {
                        method = sortedListMemberMethod.MakeGenericMethod(type.GetGenericArguments());
                    }
                }
                if (method == null)
                {
                    if (type.IsValueType) method = structDeSerializeMethod.MakeGenericMethod(type);
                    else method = memberClassDeSerializeMethod.MakeGenericMethod(type);
                }
            }
            memberMethods.Set(type, method);
            return method;
        }

        /// <summary>
        /// 基类反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo BaseSerializeMethod = typeof(DeSerializer).GetMethod("baseSerialize", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 数组对象序列化函数信息
        /// </summary>
        internal static readonly MethodInfo LeftArrayDeSerializeMethod = typeof(DeSerializer).GetMethod("leftArrayDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 字典序列化函数信息
        /// </summary>
        internal static readonly MethodInfo DictionaryDeSerializeMethod = typeof(DeSerializer).GetMethod("dictionaryDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 对象序列化函数信息
        /// </summary>
        internal static readonly MethodInfo NullableDeSerializeMethod = typeof(DeSerializer).GetMethod("nullableDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 字典序列化函数信息
        /// </summary>
        internal static readonly MethodInfo KeyValuePairDeSerializeMethod = typeof(DeSerializer).GetMethod("keyValuePairDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 字典序列化函数信息
        /// </summary>
        internal static readonly MethodInfo SortedDictionaryDeSerializeMethod = typeof(DeSerializer).GetMethod("sortedDictionaryDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 字典序列化函数信息
        /// </summary>
        internal static readonly MethodInfo SortedListDeSerializeMethod = typeof(DeSerializer).GetMethod("sortedListDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 集合反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo StructCollectionMethod = typeof(DeSerializer).GetMethod("structCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 集合反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo ClassCollectionMethod = typeof(DeSerializer).GetMethod("classCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 集合反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo StructDictionaryDeSerializeMethod = typeof(DeSerializer).GetMethod("structDictionaryDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 集合反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo ClassDictionaryDeSerializeMethod = typeof(DeSerializer).GetMethod("classDictionaryDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo EnumByteArrayMethod = typeof(DeSerializer).GetMethod("enumByteArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo EnumSByteArrayMethod = typeof(DeSerializer).GetMethod("enumSByteArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo EnumShortArrayMethod = typeof(DeSerializer).GetMethod("enumShortArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo EnumUShortArrayMethod = typeof(DeSerializer).GetMethod("enumUShortArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo EnumIntArrayMethod = typeof(DeSerializer).GetMethod("enumIntArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo EnumUIntArrayMethod = typeof(DeSerializer).GetMethod("enumUIntArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo EnumLongArrayMethod = typeof(DeSerializer).GetMethod("enumLongArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo EnumULongArrayMethod = typeof(DeSerializer).GetMethod("enumULongArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo NullableArrayMethod = typeof(DeSerializer).GetMethod("nullableArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo StructArrayMethod = typeof(DeSerializer).GetMethod("structArray", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 数组反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo ArrayMethod = typeof(DeSerializer).GetMethod("array", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// 真实类型序列化函数集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, Func<DeSerializer, object, object>> realDeSerializers = new AutoCSer.Threading.LockDictionary<Type, Func<DeSerializer, object, object>>();
        /// <summary>
        /// 基类反序列化函数信息
        /// </summary>
        private static readonly MethodInfo realTypeObjectMethod = typeof(DeSerializer).GetMethod("realTypeObject", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取真实类型序列化函数
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>真实类型序列化函数</returns>
        public static Func<DeSerializer, object, object> GetRealDeSerializer(Type type)
        {
            Func<DeSerializer, object, object> method;
            if (realDeSerializers.TryGetValue(type, out method)) return method;
            method = (Func<DeSerializer, object, object>)Delegate.CreateDelegate(typeof(Func<DeSerializer, object, object>), realTypeObjectMethod.MakeGenericMethod(type));
            realDeSerializers.Set(type, method);
            return method;
        }
    
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            memberMethods.Clear();
            realDeSerializers.Clear();
        }
        static DeSerializeMethodCache()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
