using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    internal static partial class DeSerializeMethodCache
    {
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
                        method = EnumGenericType.Get(elementType).BinaryDeSerializeEnumArrayMemberDelegate.Method;
                    }
                    else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        method = StructGenericType.Get(elementType.GetGenericArguments()[0]).BinaryDeSerializeNullableArrayMemberMethod;
                    }
                    else method = GenericType.Get(elementType).BinaryDeSerializeStructArrayMemberMethod;
                }
                else method = ClassGenericType.Get(elementType).BinaryDeSerializeArrayMemberMethod;
            }
            else if (type.IsEnum)
            {
                method = EnumGenericType.Get(type).BinaryDeSerializeEnumMemberDelegate.Method;
            }
            else
            {
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    if (genericType == typeof(Dictionary<,>))
                    {
                        method = GenericType2.Get(type.GetGenericArguments()).BinaryDeSerializeDictionaryMemberMethod;
                    }
                    else if (genericType == typeof(Nullable<>))
                    {
                        method = StructGenericType.Get(type.GetGenericArguments()[0]).BinaryDeSerializeNullableMemberMethod;
                    }
                    else if (genericType == typeof(KeyValuePair<,>))
                    {
                        method = GenericType2.Get(type.GetGenericArguments()).BinaryDeSerializeKeyValuePairMethod.Method;
                    }
                    else if (genericType == typeof(SortedDictionary<,>))
                    {
                        method = GenericType2.Get(type.GetGenericArguments()).BinaryDeSerializeSortedDictionaryMemberMethod;
                    }
                    else if (genericType == typeof(SortedList<,>))
                    {
                        method = GenericType2.Get(type.GetGenericArguments()).BinaryDeSerializeSortedListMemberMethod;
                    }
                }
                if (method == null)
                {
                    if (type.IsValueType) method = StructGenericType.Get(type).BinaryDeSerializeStructMethod;// structDeSerializeMethod.MakeGenericMethod(type);
                    else method = ClassGenericType.Get(type).BinaryDeSerializeMemberClassMethod;// memberClassDeSerializeMethod.MakeGenericMethod(type);
                }
            }
            memberMethods.Set(type, method);
            return method;
        }

        /// <summary>
        /// 基类反序列化函数信息
        /// </summary>
        internal static readonly MethodInfo BaseSerializeMethod = typeof(BinaryDeSerializer).GetMethod("baseSerialize", BindingFlags.Static | BindingFlags.NonPublic);

        static DeSerializeMethodCache()
        {
            AutoCSer.Memory.Common.AddClearCache(memberMethods.Clear, typeof(DeSerializeMethodCache), 60 * 60);
        }
    }
}
