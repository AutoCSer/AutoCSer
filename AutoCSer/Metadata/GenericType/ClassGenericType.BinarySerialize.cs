using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 引用泛型类型元数据
    /// </summary>
    internal abstract partial class ClassGenericType
    {
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeMemberClassMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeArrayMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeArrayMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeMemberClassMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeArrayMethod { get; }
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    /// <typeparam name="Type">泛型类型</typeparam>
    internal sealed partial class ClassGenericType<Type> : ClassGenericType where Type : class
    {
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerialize(ref Type value);
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeMemberClassMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.memberClassDeSerialize<Type>).Method; }
        }
        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeArray(ref Type[] value);
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeArrayMemberMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.arrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeArrayMethod
        {
            get { return ((deSerializeArray)GenericType.BinaryDeSerializer.array<Type>).Method; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeArrayMemberMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.arrayMember<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeMemberClassMethod
        {
            get { return ((Action<Type>)GenericType.BinarySerializer.MemberClassSerialize<Type>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeArrayMethod
        {
            get { return ((Action<Type[]>)GenericType.BinarySerializer.array<Type>).Method; }
        }
    }
}

