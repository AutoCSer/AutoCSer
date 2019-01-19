using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

namespace fastCSharp.Metadata
{
    /// <summary>
    /// 成员类型
    /// </summary>
    internal class MemberType
    {
        /// <summary>
        /// 空类型
        /// </summary>
        internal static readonly MemberType Null = new MemberType((Type)null);
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; protected set; }
        /// <summary>
        /// 是否字符串
        /// </summary>
        public bool IsString
        {
            get { return Type == typeof(string); }
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
        private MemberType enumerableType;
        /// <summary>
        /// 可枚举泛型类型
        /// </summary>
        public MemberType EnumerableType
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
        private MemberType enumerableArgumentType;
        /// <summary>
        /// 可枚举泛型参数类型
        /// </summary>
        public MemberType EnumerableArgumentType
        {
            get
            {
                return EnumerableType != null ? enumerableArgumentType : null;
            }
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        /// <param name="type">类型</param>
        internal MemberType(Type type)
        {
            this.Type = type;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">成员类型</param>
        /// <returns>类型</returns>
        public static implicit operator Type(MemberType value)
        {
            return value != null ? value.Type : null;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员类型</returns>
        public static implicit operator MemberType(Type type)
        {
            if (type == null) return Null;
            MemberType value;
            Monitor.Enter(typeLock);
            try
            {
                if (!types.TryGetValue(type, out value)) types.Add(type, value = new MemberType(type));
            }
            finally { Monitor.Exit(typeLock); }
            return value;
        }

        /// <summary>
        /// 成员类型隐式转换集合
        /// </summary>
        private static readonly Dictionary<Type, MemberType> types = DictionaryCreator.CreateOnly<Type, MemberType>();
        /// <summary>
        /// 隐式转换集合转换锁
        /// </summary>
        private static readonly object typeLock = new object();

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(fastCSharp.MethodImplAttribute.AggressiveInlining)]
        private static void clearCache(int count)
        {
            Monitor.Enter(typeLock);
            types.Clear();
            Monitor.Exit(typeLock);
        }
        static MemberType()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
