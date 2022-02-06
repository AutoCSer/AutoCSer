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
        /// 是否支持循环引用处理
        /// </summary>
        internal abstract bool BinarySerializeIsReferenceMember { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Func<BinaryDeSerializer, object, object> BinaryDeSerializeRealTypeObjectDelegate { get; }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeStructArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeLeftArrayMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeStructArrayMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Action<AutoCSer.BinarySerializer, object> BinarySerializeRealTypeObjectDelegate { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructArrayMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeLeftArrayMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType<T> : GenericType
    {
        /// <summary>
        /// 是否支持循环引用处理
        /// </summary>
        internal override bool BinarySerializeIsReferenceMember
        {
            get { return TypeSerializer<T>.IsReferenceMember; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Func<BinaryDeSerializer, object, object> BinaryDeSerializeRealTypeObjectDelegate
        {
            get { return AutoCSer.BinaryDeSerializer.realTypeObject<T>; }
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeStructArrayMemberMethod
        {
            get { return ((AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)BinaryDeSerializer.StructArrayMember<T>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeStructArrayMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<T[]>)BinaryDeSerializer.StructArray<T>; }
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeLeftArrayMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<LeftArray<T>>)BinaryDeSerializer.LeftArrayDeSerialize<T>; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Action<AutoCSer.BinarySerializer, object> BinarySerializeRealTypeObjectDelegate
        {
            get { return (Action<AutoCSer.BinarySerializer, object>)AutoCSer.BinarySerializer.realTypeObject<T>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructArrayMemberMethod
        {
            get { return ((Action<BinarySerializer, T[]>)BinarySerializer.StructArrayMember<T>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructArrayMethod
        {
            get { return (Action<BinarySerializer, T[]>)BinarySerializer.StructArray<T>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeLeftArrayMethod
        {
            get { return (Action<BinarySerializer, LeftArray<T>>)BinarySerializer.LeftArraySerialize<T>; }
        }
    }
}
