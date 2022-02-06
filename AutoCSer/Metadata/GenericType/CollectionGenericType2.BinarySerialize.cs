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
        internal abstract Delegate BinaryDeSerializeStructCollectionMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeClassCollectionMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructEnumByteCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructEnumSByteCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructEnumUShortCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructEnumShortCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructEnumUIntCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructEnumIntCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructEnumULongCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructEnumLongCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassEnumByteCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassEnumSByteCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassEnumUShortCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassEnumShortCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassEnumUIntCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassEnumIntCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassEnumULongCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassEnumLongCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructCollectionMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassCollectionMethod { get; }
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
        /// 获取当前类型
        /// </summary>
        internal override TypeKey CurrentType { get { return new TypeKey(typeof(Type1), typeof(Type2)); } }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeStructCollectionMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<Type1>)BinaryDeSerializer.StructCollection<Type1, Type2>; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeClassCollectionMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<Type1>)BinaryDeSerializer.ClassCollection<Type1, Type2>; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructEnumByteCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructEnumByteCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructEnumSByteCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructEnumSByteCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructEnumUShortCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructEnumUShortCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructEnumShortCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructEnumShortCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructEnumUIntCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructEnumUIntCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructEnumIntCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructEnumIntCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructEnumULongCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructEnumULongCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructEnumLongCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructEnumLongCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassEnumByteCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassEnumByteCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassEnumSByteCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassEnumSByteCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassEnumUShortCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassEnumUShortCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassEnumShortCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassEnumShortCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassEnumUIntCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassEnumUIntCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassEnumIntCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassEnumIntCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassEnumULongCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassEnumULongCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassEnumLongCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassEnumLongCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructCollection<Type2, Type1>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassCollectionMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassCollection<Type2, Type1>; }
        }
    }
}
