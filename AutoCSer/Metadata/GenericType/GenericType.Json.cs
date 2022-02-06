using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType
    {
        /// <summary>
        /// JSON 自定义反序列化不支持类型
        /// </summary>
        internal abstract Delegate JsonDeSerializeNotSupportDelegate { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonDeSerializeTypeMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract Delegate JsonDeSerializeArrayMethod { get; }
        
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Action<AutoCSer.JsonSerializer, object> JsonSerializeObjectDelegate { get; }
        /// <summary>
        /// JSON 自定义序列化不支持类型
        /// </summary>
        internal abstract Delegate JsonSerializeNotSupportDelegate { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeEnumToStringMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeStructSerializeMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeClassSerializeMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Delegate JsonSerializeArrayMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Delegate JsonSerializeStringDictionaryMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType<T> : GenericType
    {
        /// <summary>
        /// JSON 自定义反序列化不支持类型
        /// </summary>
        internal override Delegate JsonDeSerializeNotSupportDelegate { get { return (AutoCSer.JsonDeSerializer.DeSerializeDelegate<T>)AutoCSer.JsonDeSerializer.NotSupport<T>; } }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonDeSerializeTypeMethod
        {
            get { return ((JsonDeSerializer.DeSerializeDelegate<T>)JsonDeSerializer.TypeDeSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override Delegate JsonDeSerializeArrayMethod
        {
            get { return (JsonDeSerializer.DeSerializeDelegate<T[]>)JsonDeSerializer.Array<T>; }
        }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Action<AutoCSer.JsonSerializer, object> JsonSerializeObjectDelegate
        {
            get { return (Action<AutoCSer.JsonSerializer, object>)AutoCSer.JsonSerializer.SerializeObject<T>; }
        }
        /// <summary>
        /// JSON 自定义序列化不支持类型
        /// </summary>
        internal override Delegate JsonSerializeNotSupportDelegate { get { return (Action<AutoCSer.JsonSerializer, T>)AutoCSer.JsonSerializer.NotSupport<T>; } }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeEnumToStringMethod
        {
            get { return ((Action<JsonSerializer, T>)JsonSerializer.EnumToString<T>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeStructSerializeMethod
        {
            get { return ((Action<JsonSerializer, T>)JsonSerializer.StructSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeClassSerializeMethod
        {
            get { return ((Action<JsonSerializer, T>)JsonSerializer.ClassSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Delegate JsonSerializeArrayMethod
        {
            get { return (Action<JsonSerializer, T[]>)JsonSerializer.Array<T>; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Delegate JsonSerializeStringDictionaryMethod
        {
            get { return (Action<JsonSerializer, Dictionary<string, T>>)JsonSerializer.StringDictionary<T>; }
        }
    }
}
