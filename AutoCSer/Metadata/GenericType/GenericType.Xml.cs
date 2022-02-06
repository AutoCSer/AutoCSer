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
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlDeSerializeTypeMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract Delegate XmlDeSerializeArrayMethod { get; }

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
        internal abstract Delegate XmlSerializeStructArrayMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract Delegate XmlSerializeArrayMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlDeSerializeTypeMethod
        {
            get { return ((AutoCSer.XmlDeSerializer.DeSerializeDelegate<T>)XmlDeSerializer.TypeDeSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override Delegate XmlDeSerializeArrayMethod
        {
            get { return (AutoCSer.XmlDeSerializer.DeSerializeDelegate<T[]>)XmlDeSerializer.Array<T>; }
        }

        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeClassMethod
        {
            get { return ((Action<XmlSerializer, T>)XmlSerializer.ClassSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeStructMethod
        {
            get { return ((Action<XmlSerializer, T>)XmlSerializer.StructSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeEnumToStringMethod
        {
            get { return ((Action<XmlSerializer, T>)XmlSerializer.EnumToString<T>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override Delegate XmlSerializeStructArrayMethod
        {
            get { return (Action<XmlSerializer, T[]>)XmlSerializer.StructArray<T>; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override Delegate XmlSerializeArrayMethod
        {
            get { return (Action<XmlSerializer, T[]>)XmlSerializer.Array<T>; }
        }
    }
}
