using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class CollectionGenericType2
    {
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeStructCollectionMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeClassCollectionMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructEnumByteCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructEnumSByteCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructEnumUShortCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructEnumShortCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructEnumUIntCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructEnumIntCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructEnumULongCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructEnumLongCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassEnumByteCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassEnumSByteCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassEnumUShortCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassEnumShortCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassEnumUIntCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassEnumIntCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassEnumULongCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassEnumLongCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassCollectionMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="Type1">泛型类型</typeparam>
    /// <typeparam name="Type2">泛型类型</typeparam>
    internal sealed partial class CollectionGenericType2<Type1, Type2> : CollectionGenericType2
         where Type1 : ICollection<Type2>
    {
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerialize(ref Type1 value);
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeStructCollectionMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.structCollection<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeClassCollectionMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.classCollection<Type1, Type2>).Method; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructEnumByteCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structEnumByteCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructEnumSByteCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structEnumSByteCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructEnumUShortCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structEnumUShortCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructEnumShortCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structEnumShortCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructEnumUIntCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structEnumUIntCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructEnumIntCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structEnumIntCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructEnumULongCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structEnumULongCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructEnumLongCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structEnumLongCollection<Type2, Type1>).Method; }
        }        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassEnumByteCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classEnumByteCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassEnumSByteCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classEnumSByteCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassEnumUShortCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classEnumUShortCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassEnumShortCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classEnumShortCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassEnumUIntCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classEnumUIntCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassEnumIntCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classEnumIntCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassEnumULongCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classEnumULongCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassEnumLongCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classEnumLongCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structCollection<Type2, Type1>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassCollectionMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classCollection<Type2, Type1>).Method; }
        }
    }
}
