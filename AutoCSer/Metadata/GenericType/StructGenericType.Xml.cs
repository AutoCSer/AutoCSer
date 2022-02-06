using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal abstract partial class StructGenericType
    {
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlDeSerializeStructMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract Delegate XmlDeSerializeNullableMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract Delegate XmlDeSerializeNullableEnumMethod { get; }
        
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeIsOutputNullableMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract Delegate XmlSerializeNullableMethod { get; }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal sealed partial class StructGenericType<T> : StructGenericType where T : struct
    {
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlDeSerializeStructMethod
        {
            get { return ((AutoCSer.XmlDeSerializer.DeSerializeDelegate<T>)XmlDeSerializer.Struct<T>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override Delegate XmlDeSerializeNullableMethod
        {
            get { return (AutoCSer.XmlDeSerializer.DeSerializeDelegate<Nullable<T>>)XmlDeSerializer.Nullable<T>; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override Delegate XmlDeSerializeNullableEnumMethod
        {
            get { return (AutoCSer.XmlDeSerializer.DeSerializeDelegate<Nullable<T>>)XmlDeSerializer.NullableEnum<T>; }
        }

        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeIsOutputNullableMethod
        {
            get { return ((Func<XmlSerializer, Nullable<T>, bool>)XmlSerializer.IsOutputNullable<T>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override Delegate XmlSerializeNullableMethod
        {
            get { return (Action<XmlSerializer, Nullable<T>>)XmlSerializer.NullableSerialize<T>; }
        }
    }
}
