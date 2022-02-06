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
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeStructMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeNullableMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeNullableArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeNullableMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeNullableArrayMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeNullableArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeNullableMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeNullableArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeNullableDelegate { get; }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal sealed partial class StructGenericType<T> : StructGenericType where T : struct
    {
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeStructMethod
        {
            get { return ((AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T>)BinaryDeSerializer.StructDeSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeNullableMemberMethod
        {
            get { return ((AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T?>)BinaryDeSerializer.NullableMemberDeSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeNullableMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T?>)BinaryDeSerializer.NullableDeSerialize<T>; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeNullableArrayMemberMethod
        {
            get { return ((AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T?[]>)BinaryDeSerializer.NullableArrayMember<T>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeNullableArrayMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T?[]>)BinaryDeSerializer.NullableArray<T>; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeNullableArrayMemberMethod
        {
            get { return ((Action<BinarySerializer, Nullable<T>[]>)BinarySerializer.NullableArrayMember<T>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeNullableMemberMethod
        {
            get { return ((Action<BinarySerializer, Nullable<T>>)BinarySerializer.NullableMemberSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructMethod
        {
            get { return ((Action<BinarySerializer, T>)BinarySerializer.StructSerialize<T>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeNullableArrayMethod
        {
            get { return (Action<BinarySerializer, Nullable<T>[]>)BinarySerializer.NullableArray<T>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeNullableDelegate
        {
            get { return (Action<BinarySerializer, Nullable<T>>)BinarySerializer.NullableSerialize<T>; }
        }
    }
}
