using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal abstract partial class StructGenericType
    {
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
    /// <typeparam name="Type">泛型类型</typeparam>
    internal sealed partial class StructGenericType<Type> : StructGenericType where Type : struct
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
        internal delegate void deSerializeNullable(ref Nullable<Type> value);
    }
}
