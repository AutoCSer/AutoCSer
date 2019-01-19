using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    public static partial class TypeExtension
    {
        /// <summary>
        /// 类型名称泛型分隔符
        /// </summary>
        internal const char GenericSplit = '`';
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
            public unsafe string GetFullName(Type type)
            {
                if (type.IsArray)
                {
                    byte* buffer = AutoCSer.UnmanagedPool.Tiny.Get();
                    try
                    {
                        using (NameStream = new CharStream((char*)buffer, AutoCSer.UnmanagedPool.TinySize >> 1))
                        {
                            Array(type, true);
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
                            GenericFullName(type);
                            return NameStream.ToString();
                        }
                    }
                    finally { AutoCSer.UnmanagedPool.Tiny.Push(buffer); }
                }
                Type reflectedType = type.ReflectedType;
                if (reflectedType == null) return type.Namespace + "." + type.Name;
                else
                {
                    byte* buffer = AutoCSer.UnmanagedPool.Tiny.Get();
                    try
                    {
                        using (NameStream = new CharStream((char*)buffer, AutoCSer.UnmanagedPool.TinySize >> 1))
                        {
                            this.ReflectedType(type, reflectedType);
                            return NameStream.ToString();
                        }
                    }
                    finally { AutoCSer.UnmanagedPool.Tiny.Push(buffer); }
                }
            }
            /// <summary>
            /// 数组处理
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="isFullName">是否全称</param>
            internal unsafe void Array(Type type, bool isFullName)
            {
                byte* buffer = AutoCSer.UnmanagedPool.Tiny.Get();
                try
                {
                    int* currentRank = (int*)buffer, endRank = (int*)(buffer + AutoCSer.UnmanagedPool.TinySize);
                    do
                    {
                        if (currentRank == endRank) throw new IndexOutOfRangeException();
                        *currentRank++ = type.GetArrayRank();
                    }
                    while ((type = type.GetElementType()).IsArray);
                    if (isFullName) getFullName(type);
                    else getNameNoArray(type);
                    while (currentRank != buffer)
                    {
                        NameStream.Write('[');
                        int rank = *--currentRank;
                        if (--rank != 0) Number.ToString(rank, NameStream);
                        NameStream.Write(']');
                    }
                }
                finally { AutoCSer.UnmanagedPool.Tiny.Push(buffer); }
            }
            /// <summary>
            /// 任意类型处理
            /// </summary>
            /// <param name="type">类型</param>
            private void getFullName(Type type)
            {
                string value;
                if (!IsXml && TypeNames.TryGetValue(type, out value)) NameStream.WriteNotNull(value);
                else if (type.IsGenericParameter) NameStream.SimpleWriteNotNull(type.Name);
                else if (type.IsArray) Array(type, true);
                else if (type.IsGenericType) GenericFullName(type);
                else
                {
                    Type reflectedType = type.ReflectedType;
                    if (reflectedType == null)
                    {
                        NameStream.WriteNotNull(type.Namespace);
                        NameStream.Write('.');
                        NameStream.SimpleWriteNotNull(type.Name);
                    }
                    else this.ReflectedType(type, reflectedType);
                }
            }
            /// <summary>
            /// 任意类型处理
            /// </summary>
            /// <param name="type">类型</param>
            private void getNameNoArray(Type type)
            {
                string value;
                if (!IsXml && TypeNames.TryGetValue(type, out value)) NameStream.WriteNotNull(value);
                else if (type.IsGenericParameter) NameStream.SimpleWriteNotNull(type.Name);
                else if (type.IsGenericType) GenericName(type);
                else NameStream.SimpleWriteNotNull(type.Name);
            }
            /// <summary>
            /// 泛型处理
            /// </summary>
            /// <param name="type">类型</param>
            internal void GenericName(Type type)
            {
                string name = type.Name;
                int splitIndex = name.IndexOf(GenericSplit);
                Type reflectedType = type.ReflectedType;
                if (reflectedType == null)
                {
                    NameStream.UnsafeWrite(name, 0, splitIndex);
                    genericParameter(type);
                    return;
                }
                if (splitIndex == -1)
                {
                    NameStream.WriteNotNull(name);
                    return;
                }
                Type[] parameterTypes = type.GetGenericArguments();
                int parameterIndex = 0;
                do
                {
                    if (reflectedType.IsGenericType)
                    {
                        int parameterCount = reflectedType.GetGenericArguments().Length;
                        if (parameterCount != parameterTypes.Length)
                        {
                            parameterIndex = parameterCount;
                            break;
                        }
                    }
                    reflectedType = reflectedType.ReflectedType;
                }
                while (reflectedType != null);
                NameStream.UnsafeWrite(name, 0, splitIndex);
                genericParameter(parameterTypes, parameterIndex, parameterTypes.Length);
            }
            /// <summary>
            /// 泛型处理
            /// </summary>
            /// <param name="type">类型</param>
            internal void GenericFullName(Type type)
            {
                Type reflectedType = type.ReflectedType;
                if (reflectedType == null)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        getFullName(type.GetGenericArguments()[0]);
                        NameStream.Write('?');
                        return;
                    }
                    string name = type.Name;
                    NameStream.WriteNotNull(type.Namespace);
                    NameStream.Write('.');
                    NameStream.UnsafeWrite(name, 0, name.IndexOf(GenericSplit));
                    genericParameter(type);
                    return;
                }
                LeftArray<Type> reflectedTypeList = default(LeftArray<Type>);
                do
                {
                    reflectedTypeList.Add(reflectedType);
                    reflectedType = reflectedType.ReflectedType;
                }
                while (reflectedType != null);
                Type[] reflectedTypeArray = reflectedTypeList.Array;
                int reflectedTypeIndex = reflectedTypeList.Length - 1;
                reflectedType = reflectedTypeArray[reflectedTypeIndex];
                NameStream.WriteNotNull(reflectedType.Namespace);
                Type[] parameterTypes = type.GetGenericArguments();
                int parameterIndex = 0;
                do
                {
                    NameStream.Write('.');
                    if (reflectedType.IsGenericType)
                    {
                        string name = reflectedType.Name;
                        int splitIndex = name.IndexOf(GenericSplit);
                        if (splitIndex != -1)
                        {
                            NameStream.UnsafeWrite(name, 0, splitIndex);
                            int parameterCount = reflectedType.GetGenericArguments().Length;
                            genericParameter(parameterTypes, parameterIndex, parameterCount);
                            parameterIndex = parameterCount;
                        }
                        else NameStream.WriteNotNull(name);
                    }
                    else NameStream.SimpleWriteNotNull(reflectedType.Name);
                    if (reflectedTypeIndex == 0)
                    {
                        reflectedType = type;
                        type = null;
                    }
                    else reflectedType = reflectedTypeArray[--reflectedTypeIndex];
                }
                while (reflectedType != null);
            }
            /// <summary>
            /// 泛型参数处理
            /// </summary>
            /// <param name="type">类型</param>
            private void genericParameter(Type type)
            {
                NameStream.Write(IsXml ? '{' : '<');
                int index = 0;
                foreach (Type parameter in type.GetGenericArguments())
                {
                    if (index != 0) NameStream.Write(',');
                    getFullName(parameter);
                    ++index;
                }
                NameStream.Write(IsXml ? '}' : '>');
            }
            /// <summary>
            /// 泛型参数处理
            /// </summary>
            /// <param name="parameterTypes">参数类型集合</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            private void genericParameter(Type[] parameterTypes, int startIndex, int endIndex)
            {
                NameStream.Write(IsXml ? '{' : '<');
                for (getFullName(parameterTypes[startIndex]); ++startIndex != endIndex; getFullName(parameterTypes[startIndex])) NameStream.Write(',');
                NameStream.Write(IsXml ? '}' : '>');
            }
            /// <summary>
            /// 嵌套类型处理
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="reflectedType">上层类型</param>
            internal void ReflectedType(Type type, Type reflectedType)
            {
                LeftArray<Type> reflectedTypeList = default(LeftArray<Type>);
                do
                {
                    reflectedTypeList.Add(reflectedType);
                    reflectedType = reflectedType.ReflectedType;
                }
                while (reflectedType != null);
                Type[] reflectedTypeArray = reflectedTypeList.Array;
                int reflectedTypeIndex = reflectedTypeList.Length - 1;
                reflectedType = reflectedTypeArray[reflectedTypeIndex];
                NameStream.WriteNotNull(reflectedType.Namespace);
                do
                {
                    NameStream.Write('.');
                    NameStream.SimpleWriteNotNull(reflectedType.Name);
                    if (reflectedTypeIndex == 0)
                    {
                        reflectedType = type;
                        type = null;
                    }
                    else reflectedType = reflectedTypeArray[--reflectedTypeIndex];
                }
                while (reflectedType != null);
            }

            /// <summary>
            /// 类型名称集合
            /// </summary>
            internal static readonly Dictionary<Type, string> TypeNames;

            static NameBuilder()
            {
                TypeNames = AutoCSer.DictionaryCreator.CreateOnly<Type, string>();
                TypeNames.Add(typeof(bool), "bool");
                TypeNames.Add(typeof(byte), "byte");
                TypeNames.Add(typeof(sbyte), "sbyte");
                TypeNames.Add(typeof(short), "short");
                TypeNames.Add(typeof(ushort), "ushort");
                TypeNames.Add(typeof(int), "int");
                TypeNames.Add(typeof(uint), "uint");
                TypeNames.Add(typeof(long), "long");
                TypeNames.Add(typeof(ulong), "ulong");
                TypeNames.Add(typeof(float), "float");
                TypeNames.Add(typeof(double), "double");
                TypeNames.Add(typeof(decimal), "decimal");
                TypeNames.Add(typeof(char), "char");
                TypeNames.Add(typeof(string), "string");
                TypeNames.Add(typeof(object), "object");
                TypeNames.Add(typeof(void), "void");
            }
        }
        /// <summary>
        /// 根据类型获取可用名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        public static string fullName(this Type type)
        {
            if (type == null) return null;
            string value;
            if (NameBuilder.TypeNames.TryGetValue(type, out value)) return value;
            if (type.IsGenericParameter) return type.Name;
            return new NameBuilder().GetFullName(type);
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
        /// 不需要需要初始化的类型集合
        /// </summary>
        private static readonly HashSet<Type> noInitobjTypes = new HashSet<Type>(new Type[]
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
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="declaringType">自定义属性申明类型</param>
        /// <returns>自定义属性</returns>
        internal static attributeType customAttribute<attributeType>(this Type type, out Type declaringType)
            where attributeType : Attribute
        {
            while (type != null && type != typeof(object))
            {
                foreach (attributeType attribute in type.GetCustomAttributes(typeof(attributeType), false))
                {
                    declaringType = type;
                    return (attributeType)attribute;
                }
                type = type.BaseType;
            }
            declaringType = null;
            return null;
        }
        /// <summary>
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <returns>自定义属性</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static attributeType customAttribute<attributeType>(this Type type)
            where attributeType : Attribute
        {
            foreach (attributeType attribute in type.GetCustomAttributes(typeof(attributeType), false)) return attribute;
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
    }
}
