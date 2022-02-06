using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class DictionaryGenericType3
    {
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeStructDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeClassDictionaryMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeDictionaryMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeStructDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeClassDictionaryMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class DictionaryGenericType3<Type1, Type2, Type3> : DictionaryGenericType3
       where Type1 : IDictionary<Type2, Type3>
    {
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeStructDictionaryMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<Type1>)BinaryDeSerializer.StructDictionaryDeSerialize<Type1, Type2, Type3>; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeClassDictionaryMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<Type1>)BinaryDeSerializer.ClassDictionaryDeSerialize<Type1, Type2, Type3>; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeDictionaryMemberMethod
        {
            get { return ((Action<BinarySerializer, Type1>)BinarySerializer.DictionaryMember<Type1, Type2, Type3>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeDictionaryMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.DictionarySerialize<Type1, Type2, Type3>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeStructDictionaryMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.StructDictionary<Type1, Type2, Type3>; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeClassDictionaryMethod
        {
            get { return (Action<BinarySerializer, Type1>)BinarySerializer.ClassDictionary<Type1, Type2, Type3>; }
        }
    }
}
