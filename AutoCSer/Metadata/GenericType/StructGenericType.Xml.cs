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
        internal abstract MethodInfo XmlParseStructMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseNullableMethod { get; }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlParseNullableEnumMethod { get; }
        
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeIsOutputNullableMethod { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract MethodInfo XmlSerializeNullableMethod { get; }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal sealed partial class StructGenericType<Type> : StructGenericType where Type : struct
    {
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseStructMethod
        {
            get { return ((deSerialize)GenericType.XmlParser.structParse<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseNullableMethod
        {
            get { return ((deSerializeNullable)GenericType.XmlParser.nullableParse<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 反序列化函数信息
        /// </summary>
        internal override MethodInfo XmlParseNullableEnumMethod
        {
            get { return ((deSerializeNullable)GenericType.XmlParser.nullableEnumParse<Type>).Method; }
        }

        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeIsOutputNullableMethod
        {
            get { return ((Func<Nullable<Type>, bool>)GenericType.XmlSerializer.isOutputNullable<Type>).Method; }
        }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override MethodInfo XmlSerializeNullableMethod
        {
            get { return ((Action<Nullable<Type>>)GenericType.XmlSerializer.nullableSerialize<Type>).Method; }
        }
    }
}
