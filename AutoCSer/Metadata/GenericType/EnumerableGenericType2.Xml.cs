using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class EnumerableGenericType2
    {
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeStructStructEnumerableMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeStructClassEnumerableMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeClassStructEnumerableMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeClassClassEnumerableMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class EnumerableGenericType2<Type1, Type2> : EnumerableGenericType2
         where Type1 : IEnumerable<Type2>
    {
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeStructStructEnumerableMethod
        {
            get { return ((Action<Type1>)GenericType.XmlSerializer.structStructEnumerable<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeStructClassEnumerableMethod
        {
            get { return ((Action<Type1>)GenericType.XmlSerializer.structClassEnumerable<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeClassStructEnumerableMethod
        {
            get { return ((Action<Type1>)GenericType.XmlSerializer.classStructEnumerable<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeClassClassEnumerableMethod
        {
            get { return ((Action<Type1>)GenericType.XmlSerializer.classClassEnumerable<Type1, Type2>).Method; }
        }
    }
}
