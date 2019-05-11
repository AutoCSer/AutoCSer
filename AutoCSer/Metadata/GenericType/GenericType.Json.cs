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
        /// JSON 反数据序列化 实例
        /// </summary>
        internal static readonly AutoCSer.Json.Parser JsonParser = new AutoCSer.Json.Parser();
        /// <summary>
        /// JSON 数据序列化 实例
        /// </summary>
        internal static readonly AutoCSer.Json.Serializer JsonSerializer = new AutoCSer.Json.Serializer();

        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumByteMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumSByteMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumUShortMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumShortMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumUIntMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumIntMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseEnumULongMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseEnumLongMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumByteFlagsMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumSByteFlagsMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumUShortFlagsMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumShortFlagsMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumUIntFlagsMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo JsonParseEnumIntFlagsMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseEnumULongFlagsMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseEnumLongFlagsMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseTypeMethod { get; }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonParseArrayMethod { get; }
        
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Action<AutoCSer.Json.Serializer, object> JsonSerializeObjectDelegate { get; }
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
        internal abstract MethodInfo JsonSerializeArrayMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeStringDictionaryMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType<Type> : GenericType
    {
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumByteMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumByte<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumSByteMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumSByte<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumUShortMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumUShort<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumShortMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumShort<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumUIntMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumUInt<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumIntMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumInt<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseEnumULongMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumULong<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseEnumLongMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumLong<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumByteFlagsMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumByteFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumSByteFlagsMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumSByteFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumUShortFlagsMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumUShortFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumShortFlagsMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumShortFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumUIntFlagsMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumUIntFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo JsonParseEnumIntFlagsMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumIntFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseEnumULongFlagsMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumULongFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseEnumLongFlagsMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.enumLongFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseTypeMethod
        {
            get { return ((deSerialize)GenericType.JsonParser.typeParse<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 反序列化函数信息
        /// </summary>
        internal override MethodInfo JsonParseArrayMethod
        {
            get { return ((deSerializeArray)GenericType.JsonParser.array<Type>).Method; }
        }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Action<AutoCSer.Json.Serializer, object> JsonSerializeObjectDelegate
        {
            get { return (Action<AutoCSer.Json.Serializer, object>)AutoCSer.Json.Serializer.serializeObject<Type>; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeEnumToStringMethod
        {
            get { return ((Action<Type>)GenericType.JsonSerializer.EnumToString<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeStructSerializeMethod
        {
            get { return ((Action<Type>)GenericType.JsonSerializer.structSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeClassSerializeMethod
        {
            get { return ((Action<Type>)GenericType.JsonSerializer.classSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.JsonSerializer.array<Type>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeStringDictionaryMethod
        {
            get { return ((Action<Dictionary<string, Type>>)GenericType.JsonSerializer.stringDictionary<Type>).Method; }
        }
    }
}
