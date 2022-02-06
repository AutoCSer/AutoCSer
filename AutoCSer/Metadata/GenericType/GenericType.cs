using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType : GenericTypeBase
    {
        /// <summary>
        /// 数组复制
        /// </summary>
        internal abstract Delegate MemberCopyArrayDelegate { get; }
        /// <summary>
        /// 数组复制
        /// </summary>
        internal abstract Delegate MemberMapCopyArrayDelegate { get; }

        /// <summary>
        /// 获取默认空值
        /// </summary>
        /// <returns></returns>
        internal abstract object GetDefaultObject();

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
        public static GenericType Get(Type type)
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
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    internal sealed partial class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 数组复制
        /// </summary>
        internal override Delegate MemberCopyArrayDelegate
        {
            get { return (AutoCSer.MemberCopy.Copyer<T[]>.copyer)AutoCSer.MemberCopy.Copyer<T>.copyArray; }
        }
        /// <summary>
        /// 数组复制
        /// </summary>
        internal override Delegate MemberMapCopyArrayDelegate
        {
            get { return (AutoCSer.MemberCopy.Copyer<T[]>.memberMapCopyer)AutoCSer.MemberCopy.Copyer<T>.copyArray; }
        }

        /// <summary>
        /// 获取默认空值
        /// </summary>
        /// <returns></returns>
        internal override object GetDefaultObject()
        {
            return DefaultConstructor<T>.Default();
        }
    }
}
