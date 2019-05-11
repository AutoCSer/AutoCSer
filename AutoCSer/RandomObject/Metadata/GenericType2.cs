using System;
using AutoCSer.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.RandomObject.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType2
    {
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateEnumerableConstructorDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateEnumerableConstructorNullDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateDictionaryDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateDictionaryNullDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateSortedDictionaryDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateSortedDictionaryNullDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateSortedListDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateSortedListNullDelegate { get; }

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockEquatableLastDictionary<AutoCSer.Metadata.GenericType2.TypeKey, GenericType2> cache = new LockEquatableLastDictionary<AutoCSer.Metadata.GenericType2.TypeKey, GenericType2>();
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="Type1"></typeparam>
        /// <typeparam name="Type2"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static GenericType2 create<Type1, Type2>()
        {
            return new GenericType2<Type1, Type2>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(GenericType2).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public static GenericType2 Get(Type type1, Type type2)
        {
            GenericType2 value;
            AutoCSer.Metadata.GenericType2.TypeKey typeKey = new AutoCSer.Metadata.GenericType2.TypeKey { Type1 = type1, Type2 = type2 };
            if (!cache.TryGetValue(ref typeKey, out value))
            {
                value = new UnionType { Value = createMethod.MakeGenericMethod(type1, type2).Invoke(null, null) }.GenericType2;
                cache.Set(ref typeKey, value);
            }
            return value;
        }
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="typeArray"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static GenericType2 Get(Type[] typeArray)
        {
            return Get(typeArray[0], typeArray[1]);
        }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="Type1">泛型类型</typeparam>
    /// <typeparam name="Type2">泛型类型</typeparam>
    internal sealed partial class GenericType2<Type1, Type2> : GenericType2
    {
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateEnumerableConstructorDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, Type1>)AutoCSer.RandomObject.MethodCache.createEnumerableConstructor<Type1, Type2>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateEnumerableConstructorNullDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, Type1>)AutoCSer.RandomObject.MethodCache.createEnumerableConstructorNull<Type1, Type2>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateDictionaryDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, Dictionary<Type1, Type2>>)AutoCSer.RandomObject.MethodCache.createDictionary<Type1, Type2>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateDictionaryNullDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, Dictionary<Type1, Type2>>)AutoCSer.RandomObject.MethodCache.createDictionaryNull<Type1, Type2>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateSortedDictionaryDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, SortedDictionary<Type1, Type2>>)AutoCSer.RandomObject.MethodCache.createSortedDictionary<Type1, Type2>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateSortedDictionaryNullDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, SortedDictionary<Type1, Type2>>)AutoCSer.RandomObject.MethodCache.createSortedDictionaryNull<Type1, Type2>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateSortedListDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, SortedList<Type1, Type2>>)AutoCSer.RandomObject.MethodCache.createSortedList<Type1, Type2>; }
        }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateSortedListNullDelegate
        {
            get { return (Func<AutoCSer.RandomObject.Config, SortedList<Type1, Type2>>)AutoCSer.RandomObject.MethodCache.createSortedListNull<Type1, Type2>; }
        }
    }
}
