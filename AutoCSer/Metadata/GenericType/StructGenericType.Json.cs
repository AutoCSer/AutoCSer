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
        internal abstract MethodInfo JsonParseStructMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseNullableEnumMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseNullableMethod { get; }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeNullableMethod { get; }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal sealed partial class StructGenericType<Type> : StructGenericType where Type : struct
    {
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseStructMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.structParse<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseNullableEnumMethod
        {
            get { return ((deSerializeNullable)GenericType.JsonParser.nullableEnumParse<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseNullableMethod
        {
            get { return ((deSerializeNullable)GenericType.JsonParser.nullableParse<Type>).Method; }
        }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeNullableMethod
        {
            get { return ((Action<Nullable<Type>>)GenericType.JsonSerializer.nullableSerialize<Type>).Method; }
        }
    }
}
