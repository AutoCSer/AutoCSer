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
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonDeSerializeStructMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract Delegate JsonDeSerializeNullableEnumMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract Delegate JsonDeSerializeNullableMethod { get; }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Delegate JsonSerializeNullableMethod { get; }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Delegate JsonSerializeStructArrayMethod { get; }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal sealed partial class StructGenericType<T> : StructGenericType where T : struct
    {
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonDeSerializeStructMethod
        {
            get { return ((JsonDeSerializer.DeSerializeDelegate<T>)JsonDeSerializer.StructDeSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override Delegate JsonDeSerializeNullableEnumMethod
        {
            get { return (JsonDeSerializer.DeSerializeDelegate<T?>)JsonDeSerializer.NullableEnumDeSerialize<T>; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override Delegate JsonDeSerializeNullableMethod
        {
            get { return (JsonDeSerializer.DeSerializeDelegate<T?>)JsonDeSerializer.NullableDeSerialize<T>; }
        }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Delegate JsonSerializeNullableMethod
        {
            get { return (Action<JsonSerializer, Nullable<T>>)JsonSerializer.NullableSerialize<T>; }
        }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Delegate JsonSerializeStructArrayMethod
        {
            get { return (Action<JsonSerializer, T[]>)JsonSerializer.StructArray<T>; }
        }
    }
}
