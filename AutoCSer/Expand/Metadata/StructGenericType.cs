using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.Expand.Metadata
{
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal abstract class StructGenericType
    {
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal abstract Delegate NumberToCharStreamStructJoinCharDelegate { get; }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal abstract Delegate NumberToCharStreamStructSubArrayJoinCharDelegate { get; }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal abstract Delegate NumberToCharStreamNullableJoinCharDelegate { get; }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal abstract Delegate NumberToCharStreamNullableSubArrayJoinCharDelegate { get; }

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<Type, StructGenericType> cache = new LockLastDictionary<Type, StructGenericType>();
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static StructGenericType create<Type>() where Type : struct
        {
            return new StructGenericType<Type>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(StructGenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StructGenericType Get(Type type)
        {
            StructGenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType { Value = createMethod.MakeGenericMethod(type).Invoke(null, null) }.StructGenericType;
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
    /// <typeparam name="Type"></typeparam>
    internal sealed class StructGenericType<Type> : StructGenericType where Type : struct
    {
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamStructJoinCharDelegate
        {
            get { return (Func<Type[], char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.structJoinChar<Type>; }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamStructSubArrayJoinCharDelegate
        {
            get { return (Func<SubArray<Type>, char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.structSubArrayJoinChar<Type>; }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamNullableJoinCharDelegate
        {
            get { return (Func<Nullable<Type>[], char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.nullableJoinChar<Type>; }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamNullableSubArrayJoinCharDelegate
        {
            get { return (Func<SubArray<Nullable<Type>>, char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.nullableSubArrayJoinChar<Type>; }
        }
    }
}
