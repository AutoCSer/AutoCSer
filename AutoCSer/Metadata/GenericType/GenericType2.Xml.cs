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
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseKeyValuePairMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseListConstructorMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseCollectionConstructorMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseEnumerableConstructorMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseArrayConstructorMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType2<Type1, Type2> : GenericType2
    {
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseKeyValuePairMethod
        {
            get { return ((deSerializeKeyValuePair)GenericType.XmlParser.keyValuePairParse<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseListConstructorMethod
        {
            get { return ((deSerialize1)GenericType.XmlParser.listConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseCollectionConstructorMethod
        {
            get { return ((deSerialize1)GenericType.XmlParser.collectionConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseEnumerableConstructorMethod
        {
            get { return ((deSerialize1)GenericType.XmlParser.enumerableConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseArrayConstructorMethod
        {
            get { return ((deSerialize1)GenericType.XmlParser.arrayConstructor<Type1, Type2>).Method; }
        }
    }
}
