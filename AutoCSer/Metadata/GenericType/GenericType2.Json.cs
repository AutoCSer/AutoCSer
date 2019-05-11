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
        internal abstract MethodInfo JsonParseDictionaryMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseKeyValuePairMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseListConstructorMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseCollectionConstructorMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseEnumerableConstructorMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseArrayConstructorMethod { get; }
        
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeDictionaryMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeKeyValuePairMethod { get; }
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
        internal override MethodInfo JsonParseDictionaryMethod
        {
            get { return ((deSerializeDictionary)GenericType.JsonParser.dictionary<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseKeyValuePairMethod
        {
            get { return ((deSerializeKeyValuePair)GenericType.JsonParser.keyValuePairParse<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseListConstructorMethod
        {
            get { return ((deSerialize1)GenericType.JsonParser.listConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseCollectionConstructorMethod
        {
            get { return ((deSerialize1)GenericType.JsonParser.collectionConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseEnumerableConstructorMethod
        {
            get { return ((deSerialize1)GenericType.JsonParser.enumerableConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseArrayConstructorMethod
        {
            get { return ((deSerialize1)GenericType.JsonParser.arrayConstructor<Type1, Type2>).Method; }
        }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeDictionaryMethod
        {
            get { return ((Action<Dictionary<Type1, Type2>>)GenericType.JsonSerializer.dictionary<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeKeyValuePairMethod
        {
            get { return ((Action<KeyValuePair<Type1, Type2>>)GenericType.JsonSerializer.keyValuePairSerialize<Type1, Type2>).Method; }
        }
    }
}
