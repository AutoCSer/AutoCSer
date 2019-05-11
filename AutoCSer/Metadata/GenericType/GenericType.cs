using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType
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
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="Type">泛型类型</typeparam>
    internal sealed partial class GenericType<Type> : GenericType
    {
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerialize(ref Type value);
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeArray(ref Type[] value);

        /// <summary>
        /// 数组复制
        /// </summary>
        internal override Delegate MemberCopyArrayDelegate
        {
            get { return (AutoCSer.MemberCopy.Copyer<Type[]>.copyer)AutoCSer.MemberCopy.Copyer<Type>.copyArray; }
        }
        /// <summary>
        /// 数组复制
        /// </summary>
        internal override Delegate MemberMapCopyArrayDelegate
        {
            get { return (AutoCSer.MemberCopy.Copyer<Type[]>.memberMapCopyer)AutoCSer.MemberCopy.Copyer<Type>.copyArray; }
        }
    }
}
