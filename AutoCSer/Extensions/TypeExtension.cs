using AutoCSer.Memory;
using AutoCSer.Reflection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// 根据类型获取代码名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>代码名称</returns>
        public static string fullName(this Type type)
        {
            return TypeNameBuilder.GetFullName(type);
        }
        /// <summary>
        /// 获取可空类型的值类型
        /// </summary>
        /// <param name="type">可空类型</param>
        /// <returns>值类型,失败返回null</returns>
        internal static Type nullableType(this Type type)
        {
            if (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetGenericArguments()[0];
            }
            return null;
        }
        /// <summary>
        /// 判断类型是否可空类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否可空类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool isNull(this Type type)
        {
            return type != null && (!type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
        /// <summary>
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="T">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <returns>自定义属性</returns>
        internal static T customAttribute<T>(this Type type)
            where T : Attribute
        {
            foreach (T attribute in type.GetCustomAttributes(typeof(T), false)) return attribute;
            return null;
        }
        /// <summary>
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="T">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="declaringType">自定义属性申明类型</param>
        /// <returns>自定义属性</returns>
        internal static T customAttribute<T>(this Type type, out Type declaringType)
            where T : Attribute
        {
            while (type != null && type != typeof(object))
            {
                foreach (T attribute in type.GetCustomAttributes(typeof(T), false))
                {
                    declaringType = type;
                    return (T)attribute;
                }
                type = type.BaseType;
            }
            declaringType = null;
            return null;
        }
        /// <summary>
        /// 生成类型名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="nameStream">名称缓存</param>
        /// <param name="isXml">是否 XML 注释文档名称</param>
        internal static void buildName(this Type type, CharStream nameStream, bool isXml)
        {
            AutoCSer.Reflection.TypeNameBuilder nameBuilder = new AutoCSer.Reflection.TypeNameBuilder { NameStream = nameStream, IsXml = isXml };
            if (type.IsArray) nameBuilder.Array(type, true);
            else if (type.IsGenericType) nameBuilder.GenericFullName(type);
            else
            {
                Type reflectedType = type.ReflectedType;
                if (reflectedType == null)
                {
                    nameStream.SimpleWrite(type.Namespace);
                    nameStream.Write('.');
                    nameStream.SimpleWrite(type.Name);
                }
                else nameBuilder.ReflectedType(type, reflectedType);
            }
        }
        ///// <summary>
        ///// 生成 XML 注释文档名称
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public static string buildXmlName(this Type type)
        //{
        //    using (CharStream nameStream = new CharStream())
        //    {
        //        buildName(type, nameStream, true);
        //        return nameStream.ToString();
        //    }
        //}

        /// <summary>
        /// 不需要需要初始化的类型集合
        /// </summary>
        private static readonly HashSet<HashType> noInitobjTypes = new HashSet<HashType>(new HashType[]
        {
            typeof(bool), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(char),
            typeof(bool?), typeof(byte?), typeof(sbyte?), typeof(short?), typeof(ushort?), typeof(int?), typeof(uint?), typeof(long?), typeof(ulong?), typeof(float?), typeof(double?), typeof(decimal?), typeof(char?)
        });//, typeof(string)
        /// <summary>
        /// 是否需要初始化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool isInitobj(this Type type)
        {
            if (type.IsEnum || noInitobjTypes.Contains(type)) return false;
            return true;
        }
        /// <summary>
        /// 类型是否不支持序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool isSerializeNotSupport(this Type type)
        {
            return type.IsInterface || type.IsPointer || typeof(Delegate).IsAssignableFrom(type);
        }
    }
}
