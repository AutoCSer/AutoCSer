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
        internal abstract MethodInfo BinaryDeSerializeNullableMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeNullableArrayMethod { get; }

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
        internal abstract MethodInfo BinarySerializeNullableArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeNullableDelegate { get; }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal sealed partial class StructGenericType<Type> : StructGenericType where Type : struct
    {
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeStructMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.structDeSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeNullableMemberMethod
        {
            get { return ((deSerializeNullable)GenericType.BinaryDeSerializer.nullableMemberDeSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeNullableMethod
        {
            get { return ((deSerializeNullable)GenericType.BinaryDeSerializer.nullableDeSerialize<Type>).Method; }
        }
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeNullableArray(ref Nullable<Type>[] value);
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeNullableArrayMemberMethod
        {
            get { return ((deSerializeNullableArray)GenericType.BinaryDeSerializer.nullableArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeNullableArrayMethod
        {
            get { return ((deSerializeNullableArray)GenericType.BinaryDeSerializer.nullableArray<Type>).Method; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeNullableArrayMemberMethod
        {
            get { return ((Action<Nullable<Type>[]>)GenericType.BinarySerializer.nullableArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeNullableMemberMethod
        {
            get { return ((Action<Nullable<Type>>)GenericType.BinarySerializer.nullableMemberSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.structSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeNullableArrayMethod
        {
            get { return ((Action<Nullable<Type>[]>)GenericType.BinarySerializer.nullableArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeNullableDelegate
        {
            get { return (Action<AutoCSer.BinarySerialize.Serializer, Nullable<Type>>)AutoCSer.BinarySerialize.Serializer.nullableSerialize<Type>; }
        }
    }
}
