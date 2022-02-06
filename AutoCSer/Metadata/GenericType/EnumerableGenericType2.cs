using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class EnumerableGenericType2 : GenericType2Base
    {
        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<TypeKey, EnumerableGenericType2> cache = new LockLastDictionary<TypeKey, EnumerableGenericType2>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="Type1"></typeparam>
        /// <typeparam name="Type2"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static EnumerableGenericType2 create<Type1, Type2>() where Type1 : IEnumerable<Type2>
        {
            return new EnumerableGenericType2<Type1, Type2>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(EnumerableGenericType2).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public static EnumerableGenericType2 Get(Type type1, Type type2)
        {
            EnumerableGenericType2 value;
            TypeKey typeKey = new TypeKey(type1, type2);
            if (!cache.TryGetValue(typeKey, out value))
            {
                try
                {
                    value = new UnionType.EnumerableGenericType2 { Object = createMethod.MakeGenericMethod(type1, type2).Invoke(null, null) }.Value;
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
        //internal static EnumerableGenericType2 Get(Type[] typeArray)
        //{
        //    return Get(typeArray[0], typeArray[1]);
        //}
    }
}
