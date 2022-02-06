using AutoCSer.Memory;
using System;
using System.Collections.Generic;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 类型代码名称生成器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct TypeNameBuilder
    {
        /// <summary>
        /// 类型名称泛型分隔符
        /// </summary>
        internal const char GenericSplit = '`';

        /// <summary>
        /// 名称缓存
        /// </summary>
        public CharStream NameStream;
        /// <summary>
        /// 是否XML注释文档名称
        /// </summary>
        public bool IsXml
        {
            get { return NameStream.Reserve; }
            set { NameStream.Reserve = value; }
        }
        /// <summary>
        /// 获取类型名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        public string GetTypeFullName(Type type)
        {
            if (type.IsArray)
            {
                Pointer buffer = UnmanagedPool.Tiny.GetPointer();
                try
                {
                    using (NameStream = new CharStream(ref buffer))
                    {
                        Array(type, true);
                        return NameStream.ToString();
                    }
                }
                finally { UnmanagedPool.Tiny.PushOnly(ref buffer); }
            }
            if (type.IsGenericType)
            {
                Pointer buffer = UnmanagedPool.Tiny.GetPointer();
                try
                {
                    using (NameStream = new CharStream(ref buffer))
                    {
                        GenericFullName(type);
                        return NameStream.ToString();
                    }
                }
                finally { UnmanagedPool.Tiny.PushOnly(ref buffer); }
            }
            Type reflectedType = type.ReflectedType;
            if (reflectedType == null) return type.Namespace + "." + type.Name;
            else
            {
                Pointer buffer = UnmanagedPool.Tiny.GetPointer();
                try
                {
                    using (NameStream = new CharStream(ref buffer))
                    {
                        this.ReflectedType(type, reflectedType);
                        return NameStream.ToString();
                    }
                }
                finally { UnmanagedPool.Tiny.PushOnly(ref buffer); }
            }
        }
        /// <summary>
        /// 数组处理
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="isFullName">是否全称</param>
        internal unsafe void Array(Type type, bool isFullName)
        {
            Pointer buffer = UnmanagedPool.Tiny.GetPointer();
            try
            {
                int* currentRank = buffer.Int, endRank = (int*)buffer.End;
                do
                {
                    if (currentRank == endRank) throw new IndexOutOfRangeException();
                    *currentRank++ = type.GetArrayRank();
                }
                while ((type = type.GetElementType()).IsArray);
                if (isFullName) getFullName(type);
                else getNameNoArray(type);
                while (currentRank != buffer.Int)
                {
                    NameStream.Write('[');
                    int rank = *--currentRank;
                    if (--rank != 0) AutoCSer.Extensions.NumberExtension.ToString(rank, NameStream);
                    NameStream.Write(']');
                }
            }
            finally { UnmanagedPool.Tiny.PushOnly(ref buffer); }
        }
        /// <summary>
        /// 任意类型处理
        /// </summary>
        /// <param name="type">类型</param>
        private void getFullName(Type type)
        {
            string value;
            if (!IsXml && TypeNames.TryGetValue(type, out value)) NameStream.SimpleWrite(value);
            else if (type.IsGenericParameter) NameStream.SimpleWrite(type.Name);
            else if (type.IsArray) Array(type, true);
            else if (type.IsGenericType) GenericFullName(type);
            else
            {
                Type reflectedType = type.ReflectedType;
                if (reflectedType == null)
                {
                    NameStream.Write(type.Namespace);
                    NameStream.Write('.');
                    NameStream.SimpleWrite(type.Name);
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
            if (!IsXml && TypeNames.TryGetValue(type, out value)) NameStream.SimpleWrite(value);
            else if (type.IsGenericParameter) NameStream.SimpleWrite(type.Name);
            else if (type.IsGenericType) GenericName(type);
            else NameStream.SimpleWrite(type.Name);
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
                NameStream.Write(name, 0, splitIndex);
                genericParameter(type);
                return;
            }
            if (splitIndex == -1)
            {
                NameStream.SimpleWrite(name);
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
            NameStream.Write(name, 0, splitIndex);
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
                    if (IsXml)
                    {
                        NameStream.Write("System.Nullable{");
                        getFullName(type.GetGenericArguments()[0]);
                        NameStream.Write('}');
                    }
                    else
                    {
                        getFullName(type.GetGenericArguments()[0]);
                        NameStream.Write('?');
                    }
                    return;
                }
                string name = type.Name;
                NameStream.Write(type.Namespace);
                NameStream.Write('.');
                NameStream.Write(name, 0, name.IndexOf(GenericSplit));
                genericParameter(type);
                return;
            }
            LeftArray<Type> reflectedTypeList = new LeftArray<Type>(sizeof(int));
            do
            {
                reflectedTypeList.Add(reflectedType);
                reflectedType = reflectedType.ReflectedType;
            }
            while (reflectedType != null);
            Type[] reflectedTypeArray = reflectedTypeList.Array;
            int reflectedTypeIndex = reflectedTypeList.Length - 1;
            reflectedType = reflectedTypeArray[reflectedTypeIndex];
            NameStream.Write(reflectedType.Namespace);
            Type[] parameterTypes = type.GetGenericArguments();
            int parameterIndex = 0;
            bool isType = true;
            do
            {
                NameStream.Write('.');
                if (reflectedType.IsGenericType)
                {
                    string name = reflectedType.Name;
                    int splitIndex = name.IndexOf(GenericSplit);
                    if (splitIndex != -1)
                    {
                        NameStream.Write(name, 0, splitIndex);
                        int parameterCount = reflectedType.GetGenericArguments().Length;
                        genericParameter(parameterTypes, parameterIndex, parameterCount);
                        parameterIndex = parameterCount;
                    }
                    else NameStream.SimpleWrite(name);
                }
                else NameStream.SimpleWrite(reflectedType.Name);
                if (reflectedTypeIndex == 0)
                {
                    if (isType)
                    {
                        reflectedType = type;
                        isType = false;
                    }
                    else return;
                }
                else reflectedType = reflectedTypeArray[--reflectedTypeIndex];
            }
            while (true);
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
            LeftArray<Type> reflectedTypeList = new LeftArray<Type>(sizeof(int));
            do
            {
                reflectedTypeList.Add(reflectedType);
                reflectedType = reflectedType.ReflectedType;
            }
            while (reflectedType != null);
            Type[] reflectedTypeArray = reflectedTypeList.Array;
            int reflectedTypeIndex = reflectedTypeList.Length - 1;
            reflectedType = reflectedTypeArray[reflectedTypeIndex];
            NameStream.Write(reflectedType.Namespace);
            do
            {
                NameStream.Write('.');
                NameStream.SimpleWrite(reflectedType.Name);
                if (reflectedTypeIndex == 0)
                {
                    NameStream.Write('.');
                    NameStream.SimpleWrite(type.Name);
                    return;
                }
                else reflectedType = reflectedTypeArray[--reflectedTypeIndex];
            }
            while (true);
        }

        /// <summary>
        /// 根据类型获取代码名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>代码名称</returns>
        public static string GetFullName(Type type)
        {
            string value;
            if (TypeNames.TryGetValue(type, out value)) return value;
            if (type.IsGenericParameter) return type.Name;
            return new TypeNameBuilder().GetTypeFullName(type);
        }
        /// <summary>
        /// 类型代码名称集合
        /// </summary>
        internal static readonly Dictionary<HashType, string> TypeNames;

        static TypeNameBuilder()
        {
            TypeNames = AutoCSer.DictionaryCreator<HashType>.Create<string>();
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
