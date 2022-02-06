using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType2
    {
        /// <summary>
        /// 获取键值对类型
        /// </summary>
        internal abstract Type KeyValuePairType { get; }

        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract Delegate JsonDeSerializeDictionaryMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract Delegate JsonDeSerializeKeyValuePairMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonDeSerializeListConstructorMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonDeSerializeCollectionConstructorMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonDeSerializeEnumerableConstructorMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonDeSerializeArrayConstructorMethod { get; }
        
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Delegate JsonSerializeDictionaryMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Delegate JsonSerializeKeyValuePairMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType2<Type1, Type2> : GenericType2
    {
        /// <summary>
        /// 获取键值对类型
        /// </summary>
        internal override Type KeyValuePairType
        {
            get { return typeof(KeyValuePair<Type1, Type2>); }
        }

        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override Delegate JsonDeSerializeDictionaryMethod
        {
            get { return (JsonDeSerializer.DeSerializeDelegate<Dictionary<Type1, Type2>>)JsonDeSerializer.Dictionary<Type1, Type2>; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override Delegate JsonDeSerializeKeyValuePairMethod
        {
            get { return (JsonDeSerializer.DeSerializeDelegate<KeyValuePair<Type1, Type2>>)JsonDeSerializer.KeyValuePair<Type1, Type2>; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonDeSerializeListConstructorMethod
        {
            get { return ((JsonDeSerializer.DeSerializeDelegate<Type1>)JsonDeSerializer.ListConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonDeSerializeCollectionConstructorMethod
        {
            get { return ((JsonDeSerializer.DeSerializeDelegate<Type1>)JsonDeSerializer.CollectionConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonDeSerializeEnumerableConstructorMethod
        {
            get { return ((JsonDeSerializer.DeSerializeDelegate<Type1>)JsonDeSerializer.EnumerableConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonDeSerializeArrayConstructorMethod
        {
            get { return ((JsonDeSerializer.DeSerializeDelegate<Type1>)JsonDeSerializer.ArrayConstructor<Type1, Type2>).Method; }
        }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Delegate JsonSerializeDictionaryMethod
        {
            get { return (Action<JsonSerializer, Dictionary<Type1, Type2>>)JsonSerializer.Dictionary<Type1, Type2>; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Delegate JsonSerializeKeyValuePairMethod
        {
            get { return (Action<JsonSerializer, KeyValuePair<Type1, Type2>>)JsonSerializer.KeyValuePairSerialize<Type1, Type2>; }
        }
    }
}
