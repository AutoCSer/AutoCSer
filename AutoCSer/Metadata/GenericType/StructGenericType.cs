using System;
using System.Reflection;
using AutoCSer.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal abstract partial class StructGenericType : AutoCSer.Metadata.GenericTypeBase
    {
        /// <summary>
        /// 获取可空数据函数
        /// </summary>
        internal abstract MethodInfo GetNullableValueMethod { get; }
        /// <summary>
        /// 获取是否存在数据函数
        /// </summary>
        internal abstract MethodInfo GetNullableHasValueMethod { get; }

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
    /// <typeparam name="T">泛型类型</typeparam>
    internal sealed partial class StructGenericType<T> : StructGenericType where T : struct
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 获取可空数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static T getNullableValue(T? value)
        {
            return value.Value;
        }
        /// <summary>
        /// 获取可空数据函数
        /// </summary>
        internal override MethodInfo GetNullableValueMethod { get { return ((Func<T?, T>)getNullableValue).Method; } }
        /// <summary>
        /// 获取是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool getNullableHasValue(T? value)
        {
            return value.HasValue;
        }
        /// <summary>
        /// 获取是否存在数据函数
        /// </summary>
        internal override MethodInfo GetNullableHasValueMethod { get { return ((Func<T?, bool>)getNullableHasValue).Method; } }
    }
}
