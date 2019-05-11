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
        /// XML 反数据序列化 实例
        /// </summary>
        internal static readonly AutoCSer.Xml.Parser XmlParser = new AutoCSer.Xml.Parser();
        /// <summary>
        /// XML 数据序列化 实例
        /// </summary>
        internal static readonly AutoCSer.Xml.Serializer XmlSerializer = new AutoCSer.Xml.Serializer();

        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumByteMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumSByteMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumUShortMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumShortMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumUIntMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumIntMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseEnumULongMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseEnumLongMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumByteFlagsMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumSByteFlagsMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumUShortFlagsMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumShortFlagsMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumUIntFlagsMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo XmlParseEnumIntFlagsMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseEnumULongFlagsMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseEnumLongFlagsMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseTypeMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseArrayMethod { get; }

        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeClassMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeStructMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeEnumToStringMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeStructArrayMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeArrayMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType<Type> : GenericType
    {
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumByteMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumByte<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumSByteMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumSByte<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumUShortMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumUShort<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumShortMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumShort<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumUIntMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumUInt<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumIntMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumInt<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseEnumULongMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumULong<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseEnumLongMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumLong<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumByteFlagsMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumByteFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumSByteFlagsMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumSByteFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumUShortFlagsMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumUShortFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumShortFlagsMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumShortFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumUIntFlagsMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumUIntFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo XmlParseEnumIntFlagsMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumIntFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseEnumULongFlagsMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumULongFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseEnumLongFlagsMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.enumLongFlags<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseTypeMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.typeParse<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseArrayMethod
        {
            get { return ((deSerializeArray)GenericType.XmlParser.array<Type>).Method; }
        }

        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeClassMethod
        {
            get { return ((Action<Type>)GenericType.XmlSerializer.classSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeStructMethod
        {
            get { return ((Action<Type>)GenericType.XmlSerializer.structSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeEnumToStringMethod
        {
            get { return ((Action<Type>)GenericType.XmlSerializer.enumToString<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeStructArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.XmlSerializer.structArray<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.XmlSerializer.array<Type>).Method; }
        }
    }
}
