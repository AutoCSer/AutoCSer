using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class DictionaryGenericType3
    {
        /// <summary>
        /// 类型关键字
        /// </summary>
        internal struct TypeKey : IEquatable<TypeKey>
        {
            /// <summary>
            /// 泛型类型
            /// </summary>
            internal Type Type1;
            /// <summary>
            /// 泛型类型
            /// </summary>
            internal Type Type2;
            /// <summary>
            /// 泛型类型
            /// </summary>
            internal Type Type3;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(TypeKey other)
            {
                return Type1 == other.Type1 && Type2 == other.Type2 && Type3 == other.Type3;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return Type1.GetHashCode() ^ Type2.GetHashCode() ^ Type3.GetHashCode();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return Equals((TypeKey)obj);
            }
        }
        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockEquatableLastDictionary<TypeKey, DictionaryGenericType3> cache = new LockEquatableLastDictionary<TypeKey, DictionaryGenericType3>();
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="Type1"></typeparam>
        /// <typeparam name="Type2"></typeparam>
        /// <typeparam name="Type3"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static DictionaryGenericType3 create<Type1, Type2, Type3>() where Type1 : IDictionary<Type2, Type3>
        {
            return new DictionaryGenericType3<Type1, Type2, Type3>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(DictionaryGenericType3).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <param name="type3"></param>
        /// <returns></returns>
        public static DictionaryGenericType3 Get(Type type1, Type type2, Type type3)
        {
            DictionaryGenericType3 value;
            TypeKey typeKey = new TypeKey { Type1 = type1, Type2 = type2, Type3 = type3 };
            if (!cache.TryGetValue(ref typeKey, out value))
            {
                value = new UnionType { Value = createMethod.MakeGenericMethod(type1, type2, type3).Invoke(null, null) }.DictionaryGenericType3;
                cache.Set(ref typeKey, value);
            }
            return value;
        }
        ///// <summary>
        ///// 获取泛型类型元数据
        ///// </summary>
        ///// <param name="typeArray"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal static DictionaryGenericType3 Get(Type[] typeArray)
        //{
        //    return Get(typeArray[0], typeArray[1], typeArray[2]);
        //}
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="Type1">泛型类型</typeparam>
    /// <typeparam name="Type2">泛型类型</typeparam>
    /// <typeparam name="Type3">泛型类型</typeparam>
    internal sealed partial class DictionaryGenericType3<Type1, Type2, Type3> : DictionaryGenericType3
       where Type1 : IDictionary<Type2, Type3>
    {
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerialize(ref Type1 value);
    }
}
