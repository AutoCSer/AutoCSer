using System;
using System.Threading;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员类型
    /// </summary>
    internal sealed partial class ExtensionType
    {
        /// <summary>
        /// 空类型
        /// </summary>
        internal static readonly ExtensionType Null = new ExtensionType((Type)null);
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; private set; }
        ///// <summary>
        ///// 自定义类型名称
        ///// </summary>
        //private string name;
        /// <summary>
        /// 类型名称
        /// </summary>
        private string typeName;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName
        {
            get
            {
                //if (typeName == null) typeName = name == null ? (Type != null ? Type.name() : null) : name;
                if (typeName == null) typeName = Type != null ? Type.name() : null;
                return typeName;
            }
        }
        /// <summary>
        /// 类型全名
        /// </summary>
        private string fullName;
        /// <summary>
        /// 类型全名
        /// </summary>
        public string FullName
        {
            get
            {
                if (fullName == null) fullName = Type != null ? Type.fullName() : TypeName;
                return fullName;
            }
        }
        /// <summary>
        /// .NET 2.0 委托名称补丁
        /// </summary>
        public string FullName2
        {
            get
            {
#if DOTNET2
                if (Type == typeof(Action)) return "System.Action";
                string name = FullName;
                if (name.StartsWith("AutoCSer.Func<", StringComparison.Ordinal) || name.StartsWith("AutoCSer.Action<", StringComparison.Ordinal))
                {
                    return "System" + name.Substring(8);
                }
                return name;
#else
                return FullName;
#endif
            }
        }
        /// <summary>
        /// 类型名称
        /// </summary>
        private string typeOnlyName;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeOnlyName
        {
            get
            {
                //if (typeOnlyName == null) typeOnlyName = name == null ? (Type != null ? Type.onlyName() : null) : name;
                if (typeOnlyName == null) typeOnlyName = Type != null ? Type.onlyName() : null;
                return typeOnlyName;
            }
        }
        /// <summary>
        /// XML文档注释
        /// </summary>
        private string xmlDocument;
        /// <summary>
        /// XML文档注释
        /// </summary>
        public string XmlDocument
        {
            get
            {
                if (xmlDocument == null)
                {
                    xmlDocument = Type == null ? string.Empty : CodeGenerator.XmlDocument.Get(Type);
                }
                return xmlDocument.Length == 0 ? null : xmlDocument;
            }
        }
        /// <summary>
        /// 是否静态类型
        /// </summary>
        public bool IsStatic
        {
            get { return Type != null && Type.IsAbstract && Type.IsSealed; }
        }
        /// <summary>
        /// 是否值类型(排除可空类型)
        /// </summary>
        public bool IsStruct
        {
            get { return Type.isStruct() && Type.nullableType() == null; }
        }
        ///// <summary>
        ///// 是否引用类型
        ///// </summary>
        //private bool? isNull;
        /// <summary>
        /// 是否引用类型
        /// </summary>
        public bool IsNull
        {
            get
            {
                //return isNull == null ? Type == null || Type.isNull() : (bool)isNull;
                return Type == null || Type.isNull();
            }
        }
        /// <summary>
        /// 是否逻辑类型(包括可空类型)
        /// </summary>
        public bool IsBool
        {
            get { return Type == typeof(bool) || Type == typeof(bool?); }
        }
        /// <summary>
        /// 是否字符串
        /// </summary>
        public bool IsString
        {
            get { return Type == typeof(string); }
        }
        /// <summary>
        /// 是否字符串
        /// </summary>
        public bool IsSubString
        {
            get { return Type == typeof(SubString); }
        }
        /// <summary>
        /// 是否字符类型(包括可空类型)
        /// </summary>
        public bool IsChar
        {
            get { return Type == typeof(char) || Type == typeof(char?); }
        }
        /// <summary>
        /// 是否时间类型(包括可空类型)
        /// </summary>
        public bool IsDateTime
        {
            get { return Type == typeof(DateTime) || Type == typeof(DateTime?); }
        }
        /// <summary>
        /// 是否#!URL
        /// </summary>
        public bool IsHashUrl
        {
            get { return Type == typeof(AutoCSer.WebView.HashUrl); }
        }
        ///// <summary>
        ///// 是否流
        ///// </summary>
        //public bool IsStream
        //{
        //    get { return Type == typeof(Stream); }
        //}
        /// <summary>
        /// 数字类型集合
        /// </summary>
        private static readonly HashSet<Type> numberTypes = new HashSet<Type>(new Type[] { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(char) });
        /// <summary>
        /// 是否数字类型
        /// </summary>
        public bool IsNumber
        {
            get
            {
                return numberTypes.Contains(Type);
            }
        }
        /// <summary>
        /// 数字类型集合
        /// </summary>
        private static readonly HashSet<Type> numberToStringTypes = new HashSet<Type>(new Type[] { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) });
        /// <summary>
        /// 是否数字类型
        /// </summary>
        public bool IsNumberToString
        {
            get
            {
                return numberToStringTypes.Contains(Type);
            }
        }
        /// <summary>
        /// 数组构造信息
        /// </summary>
        internal ConstructorInfo ArrayConstructor { get; private set; }
        /// <summary>
        /// 列表数组构造信息
        /// </summary>
        internal ConstructorInfo IListConstructor { get; private set; }
        /// <summary>
        /// 集合构造信息
        /// </summary>
        internal ConstructorInfo ICollectionConstructor { get; private set; }
        /// <summary>
        /// 可枚举泛型构造信息
        /// </summary>
        internal ConstructorInfo IEnumerableConstructor { get; private set; }
        /// <summary>
        /// 可枚举泛型类型
        /// </summary>
        private ExtensionType enumerableType;
        /// <summary>
        /// 可枚举泛型类型
        /// </summary>
        public ExtensionType EnumerableType
        {
            get
            {
                if (enumerableType == null)
                {
                    if (!IsString)
                    {
                        Type value = Type.getGenericInterface(typeof(IEnumerable<>));
                        if (value != null)
                        {
                            if (Type.IsInterface)
                            {
                                Type interfaceType = Type.GetGenericTypeDefinition();
                                if (interfaceType == typeof(IEnumerable<>) || interfaceType == typeof(ICollection<>)
                                    || interfaceType == typeof(IList<>))
                                {
                                    enumerableArgumentType = value.GetGenericArguments()[0];
                                    enumerableType = value;
                                }
                            }
                            else if (Type.IsArray)
                            {
                                enumerableArgumentType = value.GetGenericArguments()[0];
                                enumerableType = value;
                            }
                            else
                            {
                                Type enumerableArgumentType = value.GetGenericArguments()[0];
                                Type[] parameters = new Type[1];
                                parameters[0] = enumerableArgumentType.MakeArrayType();
                                ArrayConstructor = Type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                                if (ArrayConstructor != null)
                                {
                                    this.enumerableArgumentType = enumerableArgumentType;
                                    enumerableType = value;
                                }
                                else
                                {
                                    parameters[0] = typeof(IList<>).MakeGenericType(enumerableArgumentType);
                                    IListConstructor = Type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                                    if (IListConstructor != null)
                                    {
                                        this.enumerableArgumentType = enumerableArgumentType;
                                        enumerableType = value;
                                    }
                                    else
                                    {
                                        parameters[0] = typeof(ICollection<>).MakeGenericType(enumerableArgumentType);
                                        ICollectionConstructor = Type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                                        if (ICollectionConstructor != null)
                                        {
                                            this.enumerableArgumentType = enumerableArgumentType;
                                            enumerableType = value;
                                        }
                                        else
                                        {
                                            parameters[0] = typeof(IEnumerable<>).MakeGenericType(enumerableArgumentType);
                                            IEnumerableConstructor = Type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameters, null);
                                            if (IEnumerableConstructor != null)
                                            {
                                                this.enumerableArgumentType = enumerableArgumentType;
                                                enumerableType = value;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (enumerableType == null) enumerableType = Null;
                }
                return enumerableType.Type != null ? enumerableType : null;
            }
        }
        /// <summary>
        /// 可枚举泛型参数类型
        /// </summary>
        private ExtensionType enumerableArgumentType;
        /// <summary>
        /// 可枚举泛型参数类型
        /// </summary>
        public ExtensionType EnumerableArgumentType
        {
            get
            {
                return EnumerableType != null ? enumerableArgumentType : null;
            }
        }
        /// <summary>
        /// 可控类型的值类型
        /// </summary>
        private ExtensionType nullType;
        /// <summary>
        /// 可控类型的值类型
        /// </summary>
        public ExtensionType NullType
        {
            get
            {
                if (nullType == null) nullType = (ExtensionType)Type.nullableType();
                return nullType.Type != null ? nullType : null;
            }
        }
        /// <summary>
        /// 非可控类型为null
        /// </summary>
        public ExtensionType NotNullType
        {
            get { return NullType != null ? nullType : this; }
        }
        /// <summary>
        /// 是否泛型等于比较
        /// </summary>
        public bool IsIEquatable
        {
            get
            {
                foreach (Type type in Type.GetInterfaces())
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEquatable<>) && type.GetGenericArguments()[0] == Type) return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        /// <param name="type">类型</param>
        private ExtensionType(Type type)
        {
            this.Type = type;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">成员类型</param>
        /// <returns>类型</returns>
        public static implicit operator Type(ExtensionType value)
        {
            return value != null ? value.Type : null;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员类型</returns>
        public static implicit operator ExtensionType(Type type)
        {
            if (type == null) return Null;
            ExtensionType value;
            Monitor.Enter(typeLock);
            try
            {
                if (!types.TryGetValue(type, out value)) types.Add(type, value = new ExtensionType(type));
            }
            finally { Monitor.Exit(typeLock); }
            return value;
        }
        /// <summary>
        /// AJAX toString重定向类型集合
        /// </summary>
        private static readonly HashSet<Type> ajaxToStringTypes = new HashSet<Type>(new Type[] { typeof(bool), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(char) });
        /// <summary>
        /// 是否AJAX toString重定向类型
        /// </summary>
        public bool IsAjaxToString
        {
            get { return ajaxToStringTypes.Contains(Type.nullableType() ?? Type); }
        }
        /// <summary>
        /// 客户端视图绑定类型
        /// </summary>
        private AutoCSer.WebView.ClientTypeAttribute clientViewType;
        /// <summary>
        /// 客户端视图绑定类型
        /// </summary>
        public string ClientViewTypeName
        {
            get
            {
                if (clientViewType == null)
                {
                    clientViewType = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.WebView.ClientTypeAttribute>(Type, true) ?? AutoCSer.WebView.ClientTypeAttribute.Null;
                }
                return clientViewType.GetClientName(Type);
            }
        }
        /// <summary>
        /// 客户端视图绑定成员名称
        /// </summary>
        public string ClientViewMemberName
        {
            get
            {
                if (clientViewType == null)
                {
                    clientViewType = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.WebView.ClientTypeAttribute>(Type, true) ?? AutoCSer.WebView.ClientTypeAttribute.Null;
                }
                return clientViewType.MemberName;
            }
        }
        /// <summary>
        /// TypeScript 类型集合
        /// </summary>
        private static readonly Dictionary<Type, string> scriptTypes = DictionaryCreator.CreateOnly<Type, string>();
        /// <summary>
        /// TypeScript 类型
        /// </summary>
        public string ScriptType
        {
            get
            {
                string name;
                if (Type.IsArray)
                {
                    name = ((ExtensionType)Type.GetElementType()).ScriptType;
                    if (name != null) return name + "[]";
                }
                else if(scriptTypes.TryGetValue(Type, out name)) return name;
                return null;
            }
        }

        /// <summary>
        /// 成员类型隐式转换集合
        /// </summary>
        private static Dictionary<Type, ExtensionType> types = DictionaryCreator.CreateOnly<Type, ExtensionType>();
        /// <summary>
        /// 隐式转换集合转换锁
        /// </summary>
        private static readonly object typeLock = new object();
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            Monitor.Enter(typeLock);
            try
            {
                if (types.Count != 0) types = DictionaryCreator.CreateOnly<Type, ExtensionType>();
            }
            finally { Monitor.Exit(typeLock); }
        }
        static ExtensionType()
        {
            Pub.ClearCaches += clearCache;

            scriptTypes = DictionaryCreator.CreateOnly<Type, string>();
            scriptTypes.Add(typeof(bool), "boolean");
            scriptTypes.Add(typeof(byte), "number");
            scriptTypes.Add(typeof(sbyte), "number");
            scriptTypes.Add(typeof(ushort), "number");
            scriptTypes.Add(typeof(short), "number");
            scriptTypes.Add(typeof(uint), "number");
            scriptTypes.Add(typeof(int), "number");
            scriptTypes.Add(typeof(ulong), "number");
            scriptTypes.Add(typeof(long), "number");
            scriptTypes.Add(typeof(float), "number");
            scriptTypes.Add(typeof(double), "number");
            scriptTypes.Add(typeof(decimal), "number");
            scriptTypes.Add(typeof(char), "string");
            scriptTypes.Add(typeof(string), "string");
            scriptTypes.Add(typeof(DateTime), "Date");
        }
    }
}
