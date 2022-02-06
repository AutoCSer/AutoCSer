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
        internal abstract Delegate XmlSerializeStructStructEnumerableMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract Delegate XmlSerializeStructClassEnumerableMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract Delegate XmlSerializeClassStructEnumerableMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract Delegate XmlSerializeClassClassEnumerableMethod { get; }
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
        internal override Delegate XmlSerializeStructStructEnumerableMethod
        {
            get { return (Action<XmlSerializer, Type1>)XmlSerializer.StructStructEnumerable<Type1, Type2>; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override Delegate XmlSerializeStructClassEnumerableMethod
        {
            get { return (Action<XmlSerializer, Type1>)XmlSerializer.StructClassEnumerable<Type1, Type2>; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override Delegate XmlSerializeClassStructEnumerableMethod
        {
            get { return (Action<XmlSerializer, Type1>)XmlSerializer.ClassStructEnumerable<Type1, Type2>; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override Delegate XmlSerializeClassClassEnumerableMethod
        {
            get { return (Action<XmlSerializer, Type1>)XmlSerializer.ClassClassEnumerable<Type1, Type2>; }
        }
    }
}
