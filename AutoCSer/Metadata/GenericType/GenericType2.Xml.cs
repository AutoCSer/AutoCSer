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
        internal abstract Delegate XmlDeSerializeKeyValuePairMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlDeSerializeListConstructorMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlDeSerializeCollectionConstructorMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlDeSerializeEnumerableConstructorMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlDeSerializeArrayConstructorMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType2<Type1, Type2> : GenericType2
    {
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override Delegate XmlDeSerializeKeyValuePairMethod
        {
            get { return (AutoCSer.XmlDeSerializer.DeSerializeDelegate<KeyValuePair<Type1, Type2>>)XmlDeSerializer.KeyValuePairDeSerialize<Type1, Type2>; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlDeSerializeListConstructorMethod
        {
            get { return ((AutoCSer.XmlDeSerializer.DeSerializeDelegate<Type1>)XmlDeSerializer.ListConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlDeSerializeCollectionConstructorMethod
        {
            get { return ((AutoCSer.XmlDeSerializer.DeSerializeDelegate<Type1>)XmlDeSerializer.CollectionConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlDeSerializeEnumerableConstructorMethod
        {
            get { return ((AutoCSer.XmlDeSerializer.DeSerializeDelegate<Type1>)XmlDeSerializer.EnumerableConstructor<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlDeSerializeArrayConstructorMethod
        {
            get { return ((AutoCSer.XmlDeSerializer.DeSerializeDelegate<Type1>)XmlDeSerializer.ArrayConstructor<Type1, Type2>).Method; }
        }
    }
}
