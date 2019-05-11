using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;
using AutoCSer.BinarySerialize;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType
    {
        /// <summary>
        /// 二进制反数据序列化 实例
        /// </summary>
        internal static readonly AutoCSer.BinarySerialize.DeSerializer BinaryDeSerializer = new AutoCSer.BinarySerialize.DeSerializer();
        /// <summary>
        /// 二进制数据序列化 实例
        /// </summary>
        internal static readonly AutoCSer.BinarySerialize.Serializer BinarySerializer = new AutoCSer.BinarySerialize.Serializer();

        /// <summary>
        /// 是否支持循环引用处理
        /// </summary>
        internal abstract bool BinarySerializeIsReferenceMember { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Func<DeSerializer, object, object> BinaryDeSerializeRealTypeObjectDelegate { get; }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumByteMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumSByteMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumShortMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumUShortMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumIntMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumUIntMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumLongMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumULongMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumByteArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumSByteArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumShortArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumUShortArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumIntArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumUIntArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumLongArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumULongArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeStructArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeLeftArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumByteArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumSByteArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumUShortArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumShortArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumUIntArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumIntArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumULongArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeEnumLongArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeStructArrayMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Action<AutoCSer.BinarySerialize.Serializer, object> BinarySerializeRealTypeObjectDelegate { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumByteArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumSByteArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumUShortArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumShortArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumUIntArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumIntArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumULongArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumLongArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumByteMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumSByteMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumUShortMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumShortMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumUIntMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumIntMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumULongMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumLongMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumByteArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumSByteArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumUShortArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumShortArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumUIntArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumIntArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumULongArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeEnumLongArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeLeftArrayMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType<Type> : GenericType
    {
        /// <summary>
        /// 是否支持循环引用处理
        /// </summary>
        internal override bool BinarySerializeIsReferenceMember
        {
            get { return TypeSerializer<Type>.IsReferenceMember; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Func<DeSerializer, object, object> BinaryDeSerializeRealTypeObjectDelegate
        {
            get { return AutoCSer.BinarySerialize.DeSerializer.realTypeObject<Type>; }
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumByteMemberMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.enumByteMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumSByteMemberMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.enumSByteMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumShortMemberMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.enumShortMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumUShortMemberMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.enumUShortMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumIntMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.EnumInt<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumUIntMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.EnumUInt<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumLongMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.EnumLong<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumULongMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.EnumULong<Type>).Method; }
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumByteArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumByteArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumSByteArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumSByteArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumShortArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumShortArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumUShortArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumUShortArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumIntArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumIntArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumUIntArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumUIntArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumLongArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumLongArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumULongArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumULongArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeStructArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.structArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumByteArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumByteArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumSByteArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumSByteArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumUShortArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumUShortArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumShortArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumShortArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumUIntArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumUIntArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumIntArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumIntArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumULongArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumULongArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeEnumLongArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.enumLongArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeStructArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.structArray<Type>).Method; }
        }

        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeLeftArray(ref LeftArray<Type> value);
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeLeftArrayMethod
        {
            get { return ((deSerializeLeftArray)GenericType.BinaryDeSerializer.leftArrayDeSerialize<Type>).Method; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Action<AutoCSer.BinarySerialize.Serializer, object> BinarySerializeRealTypeObjectDelegate
        {
            get { return (Action<AutoCSer.BinarySerialize.Serializer, object>)AutoCSer.BinarySerialize.Serializer.realTypeObject<Type>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumByteArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumByteArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumSByteArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumSByteArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumUShortArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumUShortArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumShortArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumShortArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumUIntArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumUIntArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumIntArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumIntArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumULongArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumULongArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumLongArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumLongArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.structArrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumByteMemberMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.enumByteMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumSByteMemberMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.enumSByteMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumUShortMemberMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.enumUShortMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumShortMemberMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.enumShortMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumUIntMemberMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.enumUIntMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumIntMemberMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.enumIntMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumULongMemberMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.enumULongMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumLongMemberMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.enumLongMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumByteArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumByteArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumSByteArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumSByteArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumUShortArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumUShortArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumShortArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumShortArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumUIntArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumUIntArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumIntArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumIntArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumULongArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumULongArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeEnumLongArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.enumLongArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.structArray<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeLeftArrayMethod
        {
            get { return ((Action<LeftArray<Type>>)GenericType.BinarySerializer.leftArraySerialize<Type>).Method; }
        }
    }
}
