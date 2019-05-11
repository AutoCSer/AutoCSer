using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.FieldEquals.Metadata
{
    /// <summary>
    /// 引用泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType
    {
        /// <summary>
        /// 数据比较
        /// </summary>
        internal abstract Delegate ArrayDelegate { get; }
        /// <summary>
        /// 数据比较
        /// </summary>
        internal abstract Delegate LeftArrayDelegate { get; }
        /// <summary>
        /// 数据比较
        /// </summary>
        internal abstract Delegate ListArrayDelegate { get; }

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<Type, GenericType> cache = new LockLastDictionary<Type, GenericType>();
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static GenericType create<Type>()
        {
            return new GenericType<Type>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(GenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(Type type)
        {
            GenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType { Value = createMethod.MakeGenericMethod(type).Invoke(null, null) }.GenericType;
                    cache.Set(type, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    /// <typeparam name="Type">泛型类型</typeparam>
    internal sealed partial class GenericType<Type> : GenericType
    {
        /// <summary>
        /// 数据比较
        /// </summary>
        internal override Delegate ArrayDelegate
        {
            get { return (Func<Type[], Type[], bool>)AutoCSer.FieldEquals.MethodCache.array<Type>; }
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        internal override Delegate LeftArrayDelegate
        {
            get { return (Func<LeftArray<Type>, LeftArray<Type>, bool>)AutoCSer.FieldEquals.MethodCache.leftArray<Type>; }
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        internal override Delegate ListArrayDelegate
        {
            get { return (Func<ListArray<Type>, ListArray<Type>, bool>)AutoCSer.FieldEquals.MethodCache.listArray<Type>; }
        }
    }
}

