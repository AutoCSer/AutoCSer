using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    //public static partial class TypeExtension
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
    }
}
