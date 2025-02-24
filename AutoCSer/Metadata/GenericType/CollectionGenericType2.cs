﻿using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class CollectionGenericType2  : GenericType2Base
    {
        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<TypeKey, CollectionGenericType2> cache = new LockLastDictionary<TypeKey, CollectionGenericType2>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="Type1"></typeparam>
        /// <typeparam name="Type2"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static CollectionGenericType2 create<Type1, Type2>() where Type1 : ICollection<Type2>
        {
            return new CollectionGenericType2<Type1, Type2>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(CollectionGenericType2).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public static CollectionGenericType2 Get(Type type1, Type type2)
        {
            CollectionGenericType2 value;
            TypeKey typeKey = new TypeKey(type1, type2);
            if (!cache.TryGetValue(typeKey, out value))
            {
                try
                {
                    value = new UnionType.CollectionGenericType2 { Object = createMethod.MakeGenericMethod(type1, type2).Invoke(null, null) }.Value;
                    cache.Set(typeKey, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
        ///// <summary>
        ///// 获取泛型类型元数据
        ///// </summary>
        ///// <param name="typeArray"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal static CollectionGenericType2 Get(Type[] typeArray)
        //{
        //    return Get(typeArray[0], typeArray[1]);
        //}
    }
}
