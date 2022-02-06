using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.Expand.Metadata
{
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal abstract class StructGenericType : AutoCSer.Metadata.GenericTypeBase
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
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, StructGenericType> cache = new LockLastDictionary<HashType, StructGenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static StructGenericType create<T>() where T : struct
        {
            return new StructGenericType<T>();
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
        public static StructGenericType Get(HashType type)
        {
            StructGenericType value;
            if (!cache.TryGetValue(type, out value))
            {
                try
                {
                    value = new UnionType.StructGenericType { Object = createMethod.MakeGenericMethod(type).Invoke(null, null) }.Value;
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
    /// <typeparam name="T"></typeparam>
    internal sealed class StructGenericType<T> : StructGenericType
        where T : struct
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamStructJoinCharDelegate
        {
            get { return (Func<T[], char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.structJoinChar<T>; }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamStructSubArrayJoinCharDelegate
        {
            get { return (Func<SubArray<T>, char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.structSubArrayJoinChar<T>; }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamNullableJoinCharDelegate
        {
            get { return (Func<Nullable<T>[], char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.nullableJoinChar<T>; }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal override Delegate NumberToCharStreamNullableSubArrayJoinCharDelegate
        {
            get { return (Func<SubArray<Nullable<T>>, char, string, string>)AutoCSer.NumberToCharStream.JoinMethod.nullableSubArrayJoinChar<T>; }
        }
    }
}
