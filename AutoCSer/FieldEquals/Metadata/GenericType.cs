using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.FieldEquals.Metadata
{
    /// <summary>
    /// 引用泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType : AutoCSer.Metadata.GenericTypeBase
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
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, GenericType> cache = new LockLastDictionary<HashType, GenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
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
        public static GenericType Get(HashType type)
        {
            GenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType.GenericType { Object = createMethod.MakeGenericMethod(type).Invoke(null, null) }.Value;
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
    /// <typeparam name="T">泛型类型</typeparam>
    internal sealed partial class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 数据比较
        /// </summary>
        internal override Delegate ArrayDelegate
        {
            get { return (Func<T[], T[], bool>)AutoCSer.FieldEquals.MethodCache.array<T>; }
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        internal override Delegate LeftArrayDelegate
        {
            get { return (Func<LeftArray<T>, LeftArray<T>, bool>)AutoCSer.FieldEquals.MethodCache.leftArray<T>; }
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        internal override Delegate ListArrayDelegate
        {
            get { return (Func<ListArray<T>, ListArray<T>, bool>)AutoCSer.FieldEquals.MethodCache.listArray<T>; }
        }
    }
}

