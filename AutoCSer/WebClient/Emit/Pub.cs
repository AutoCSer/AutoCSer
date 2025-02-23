﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.WebClient.Emit
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Pub
    {
        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="memberAttribute"></typeparam>
        /// <param name="memberFilter"></param>
        /// <param name="isAllMember"></param>
        /// <returns>字段成员集合</returns>
        internal static LeftArray<FieldIndex> GetFields<valueType, memberAttribute>(MemberFilters memberFilter, bool isAllMember)
            where memberAttribute : IgnoreMemberAttribute
        {
            FieldIndex[] fieldIndexs = AutoCSer.Metadata.MemberIndexGroup<valueType>.GetFields(memberFilter);
            LeftArray<FieldIndex> fields = new LeftArray<FieldIndex>(fieldIndexs.Length);
            foreach (FieldIndex field in fieldIndexs)
            {
                Type type = field.Member.FieldType;
                if (!type.IsPointer && (!type.IsArray || type.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(type) && !typeof(Delegate).IsAssignableFrom(type))
                {
                    memberAttribute attribute = field.GetAttribute<memberAttribute>(true);
                    if (isAllMember ? (attribute == null || attribute.IsSetup) : (attribute != null && attribute.IsSetup))
                    {
                        fields.Add(field);
                    }
                }
            }
            return fields;
        }

        /// <summary>
        /// 数值转换调用函数信息集合
        /// </summary>
        private static readonly Dictionary<HashType, MethodInfo> numberToStringMethods;
        /// <summary>
        /// 获取数值转换委托调用函数信息
        /// </summary>
        /// <param name="type">数值类型</param>
        /// <returns>数值转换委托调用函数信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static MethodInfo GetNumberToStringMethod(Type type)
        {
            MethodInfo method;
            return numberToStringMethods.TryGetValue(type, out method) ? method : null;
        }
        /// <summary>
        /// 字符串转换调用函数信息集合
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> toStringMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
        /// <summary>
        /// 字符串转换调用函数信息访问锁
        /// </summary>
        private static readonly object toStringMethodLock = new object();
        /// <summary>
        /// 获取字符串转换委托调用函数信息
        /// </summary>
        /// <param name="type">数值类型</param>
        /// <returns>字符串转换委托调用函数信息</returns>
        internal static MethodInfo GetToStringMethod(Type type)
        {
            type.ToString();
            MethodInfo method;
            Monitor.Enter(toStringMethodLock);
            if (toStringMethods.TryGetValue(type, out method))
            {
                Monitor.Exit(toStringMethodLock);
                return method;
            }
            try
            {
                method = type.GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public, null, EmptyArray<Type>.Array, null);
                toStringMethods.Add(type, method);
            }
            finally { Monitor.Exit(toStringMethodLock); }
            return method;
        }
        /// <summary>
        /// 添加表单函数信息
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void add(NameValueCollection nameValueCollection, string name, string value)
        {
            nameValueCollection.Add(name, value);
        }
        /// <summary>
        /// 添加表单函数信息
        /// </summary>
        internal static readonly MethodInfo NameValueCollectionAddMethod = ((Action<NameValueCollection, string, string>)add).Method;

        static Pub()
        {
            numberToStringMethods = DictionaryCreator<HashType>.Create<MethodInfo>();
            numberToStringMethods.Add(typeof(int), ((Func<int, string>)AutoCSer.Extensions.NumberExtension.toString).Method);
            numberToStringMethods.Add(typeof(long), ((Func<long, string>)AutoCSer.Extensions.NumberExtension.toString).Method);
            numberToStringMethods.Add(typeof(byte), ((Func<byte, string>)AutoCSer.Extensions.NumberExtension.toString).Method);
            numberToStringMethods.Add(typeof(uint), ((Func<uint, string>)AutoCSer.Extensions.NumberExtension.toString).Method);
            numberToStringMethods.Add(typeof(ulong), ((Func<ulong, string>)AutoCSer.Extensions.NumberExtension.toString).Method);
            numberToStringMethods.Add(typeof(sbyte), ((Func<sbyte, string>)AutoCSer.Extensions.NumberExtension.toString).Method);
            numberToStringMethods.Add(typeof(short), ((Func<short, string>)AutoCSer.Extensions.NumberExtension.toString).Method);
            numberToStringMethods.Add(typeof(ushort), ((Func<ushort, string>)AutoCSer.Extensions.NumberExtension.toString).Method);
        }
    }
}
