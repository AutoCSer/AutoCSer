using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType2
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
            /// 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(TypeKey other)
            {
                return Type1 == other.Type1 && Type2 == other.Type2;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return Type1.GetHashCode() ^ Type2.GetHashCode();
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
        private static readonly AutoCSer.Threading.LockEquatableLastDictionary<TypeKey, GenericType2> cache = new LockEquatableLastDictionary<TypeKey, GenericType2>();
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
            TypeKey typeKey = new TypeKey { Type1 = type1, Type2 = type2 };
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
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeDictionary(ref Dictionary<Type1, Type2> value);
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeKeyValuePair(ref KeyValuePair<Type1, Type2> value);
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerialize1(ref Type1 value);
    }
}
