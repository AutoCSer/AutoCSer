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
        internal abstract MethodInfo BinaryDeSerializeStructDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeClassDictionaryMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeDictionaryMemberMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeStructDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeClassDictionaryMethod { get; }
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
        internal override MethodInfo BinaryDeSerializeStructDictionaryMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.structDictionaryDeSerialize<Type1, Type2, Type3>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeClassDictionaryMethod
        {
            get { return ((deSerialize)GenericType.BinaryDeSerializer.classDictionaryDeSerialize<Type1, Type2, Type3>).Method; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeDictionaryMemberMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.dictionaryMember<Type1, Type2, Type3>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeDictionaryMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.dictionarySerialize<Type1, Type2, Type3>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeStructDictionaryMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.structDictionary<Type1, Type2, Type3>).Method; }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeClassDictionaryMethod
        {
            get { return ((Action<Type1>)GenericType.BinarySerializer.classDictionary<Type1, Type2, Type3>).Method; }
        }
    }
}
