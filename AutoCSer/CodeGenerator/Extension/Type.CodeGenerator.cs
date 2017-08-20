using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    internal static partial class TypeCodeGenerator
    {
        /// <summary>
        /// 类型名称生成器
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal partial struct NameBuilder
        {
            /// <summary>
            /// 名称缓存
            /// </summary>
            public CharStream NameStream;
            /// <summary>
            /// 是否XML注释文档名称
            /// </summary>
            public bool IsXml;
            /// <summary>
            /// 获取类型名称
            /// </summary>
            /// <param name="type">类型</param>
            /// <returns>类型名称</returns>
            public unsafe string GetName(Type type)
            {
                if (type.IsArray)
                {
                    byte* buffer = AutoCSer.UnmanagedPool.Tiny.Get();
                    try
                    {
                        using (NameStream = new CharStream((char*)buffer, AutoCSer.UnmanagedPool.TinySize >> 1))
                        {
                            new TypeExtension.NameBuilder { NameStream = NameStream }.Array(type, false);
                            return NameStream.ToString();
                        }
                    }
                    finally { AutoCSer.UnmanagedPool.Tiny.Push(buffer); }
                }
                if (type.IsGenericType)
                {
                    byte* buffer = AutoCSer.UnmanagedPool.Tiny.Get();
                    try
                    {
                        using (NameStream = new CharStream((char*)buffer, AutoCSer.UnmanagedPool.TinySize >> 1))
                        {
                            new TypeExtension.NameBuilder { NameStream = NameStream }.GenericName(type);
                            return NameStream.ToString();
                        }
                    }
                    finally { AutoCSer.UnmanagedPool.Tiny.Push(buffer); }
                }
                return type.Name;
            }
            /// <summary>
            /// 获取XML注释文档名称
            /// </summary>
            /// <param name="type"></param>
            public void Xml(Type type)
            {
                TypeExtension.NameBuilder nameBuilder = new TypeExtension.NameBuilder { NameStream = NameStream, IsXml = IsXml };
                if (type.IsArray) nameBuilder.Array(type, true);
                else if (type.IsGenericType) nameBuilder.GenericFullName(type);
                else
                {
                    Type reflectedType = type.ReflectedType;
                    if (reflectedType == null)
                    {
                        NameStream.SimpleWriteNotNull(type.Namespace);
                        NameStream.Write('.');
                        NameStream.SimpleWriteNotNull(type.Name);
                    }
                    else nameBuilder.ReflectedType(type, reflectedType);
                }
            }
        }
        /// <summary>
        /// 根据类型获取可用名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        internal static string name(this Type type)
        {
            if (type == null) return null;
            string value;
            if (TypeExtension.NameBuilder.TypeNames.TryGetValue(type, out value)) return value;
            if (type.IsGenericParameter) return type.Name;
            return new NameBuilder().GetName(type);
        }
        /// <summary>
        /// 根据类型获取可用名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        internal static string onlyName(this Type type)
        {
            string value;
            if (TypeExtension.NameBuilder.TypeNames.TryGetValue(type, out value)) return value;
            value = type.Name;
            if (type.IsGenericTypeDefinition)
            {
                int index = value.IndexOf(TypeExtension.GenericSplit);
                if (index != -1) value = value.Substring(0, index);
            }
            return value;
        }
        /// <summary>
        /// 是否值类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool isStruct(this Type type)
        {
            return type != null && type.IsValueType && !type.IsEnum;
        }
        /// <summary>
        /// 访问控制符
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>访问控制符</returns>
        internal static string getAccessDefinition(this Type type)
        {
            if (type != null)
            {
                if (type.IsNested)
                {
                    if (type.IsNestedPublic) return "public";
                    if (type.IsNestedPrivate) return "private";
                    if (type.IsNestedAssembly) return "internal";
                }
                else return type.IsPublic ? "public" : "internal";
            }
            return null;
        }
        /// <summary>
        /// 获取泛型接口类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="interfaceType">泛型接口类型定义</param>
        /// <returns>泛型接口类型,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Type getGenericInterface(this Type type, Type interfaceType)
        {
            foreach (Type nextType in getGenericInterfaces(type, interfaceType)) return nextType;
            return null;
        }
        /// <summary>
        /// 获取泛型接口类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="interfaceType">泛型接口类型定义</param>
        /// <returns>泛型接口类型,失败返回null</returns>
        internal static IEnumerable<Type> getGenericInterfaces(this Type type, Type interfaceType)
        {
            if (type != null && interfaceType != null && interfaceType.IsInterface)
            {
                if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == interfaceType) yield return type;
                foreach (Type nextType in type.GetInterfaces())
                {
                    if (nextType.IsGenericType && nextType.GetGenericTypeDefinition() == interfaceType) yield return nextType;
                }
            }
        }
        /// <summary>
        /// 是否继承泛型定义
        /// </summary>
        /// <param name="genericDefinition"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool isAssignableFromGenericDefinition(this Type genericDefinition, Type type)
        {
            for (Type baseType = type; baseType != null; baseType = baseType.BaseType)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericDefinition)
                {
                    Type[] arguments = baseType.GetGenericArguments();
                    return arguments.Length == 1 && arguments[0] == type;
                }
            }
            return false;
        }
    }
}
