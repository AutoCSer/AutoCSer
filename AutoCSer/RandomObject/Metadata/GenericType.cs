using System;
using System.Reflection;
using AutoCSer.Threading;
using System.Collections.Generic;

namespace AutoCSer.RandomObject.Metadata
{
    /// <summary>
    /// 引用泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType
    {
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract MethodInfo CreateMethod { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract MethodInfo CreateMemberMethod { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateArrayDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateArrayNullDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateLeftArrayDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateListArrayDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateListArrayNullDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateListDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateListNullDelegate { get; }

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
        /// 创建随机对象
        /// </summary>
        internal override MethodInfo CreateMethod
        {
            get { return ((Func<AutoCSer.RandomObject.Config, Type>)AutoCSer.RandomObject.MethodCache.create<Type>).Method; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="config"></param>
        private delegate void createMember(ref Type value, AutoCSer.RandomObject.Config config);
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override MethodInfo CreateMemberMethod
        {
            get { return ((createMember)AutoCSer.RandomObject.MethodCache.createMember<Type>).Method; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateArrayDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, Type[]>)AutoCSer.RandomObject.MethodCache.createArray<Type>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateArrayNullDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, Type[]>)AutoCSer.RandomObject.MethodCache.createArrayNull<Type>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateLeftArrayDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, LeftArray<Type>>)AutoCSer.RandomObject.MethodCache.createLeftArray<Type>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateListArrayDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, ListArray<Type>>)AutoCSer.RandomObject.MethodCache.createListArray<Type>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateListArrayNullDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, ListArray<Type>>)AutoCSer.RandomObject.MethodCache.createListArrayNull<Type>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateListDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, List<Type>>)AutoCSer.RandomObject.MethodCache.createList<Type>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateListNullDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, List<Type>>)AutoCSer.RandomObject.MethodCache.createListNull<Type>; }
        }
    }
}

