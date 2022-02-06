using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType2
    {
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeDictionaryMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeSortedDictionaryMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeSortedListMemberMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeKeyValuePairMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeSortedDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeSortedListMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeKeyValuePairMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType2<Type1, Type2> : GenericType2
    {
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeDictionaryMemberMethod
        {
            get { return ((AutoCSer.BinaryDeSerializer.DeSerializeDelegate<Dictionary<Type1, Type2>>)BinaryDeSerializer.DictionaryMember<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeDictionaryMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<Dictionary<Type1, Type2>>)BinaryDeSerializer.DictionaryDeSerialize<Type1, Type2>; }
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeSortedDictionaryMemberMethod
        {
            get { return ((AutoCSer.BinaryDeSerializer.DeSerializeDelegate<SortedDictionary<Type1, Type2>>)BinaryDeSerializer.SortedDictionaryMember<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeSortedDictionaryMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<SortedDictionary<Type1, Type2>>)BinaryDeSerializer.SortedDictionaryDeSerialize<Type1, Type2>; }
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeSortedListMemberMethod
        {
            get { return ((AutoCSer.BinaryDeSerializer.DeSerializeDelegate<SortedList<Type1, Type2>>)BinaryDeSerializer.SortedListMember<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeSortedListMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<SortedList<Type1, Type2>>)BinaryDeSerializer.SortedListDeSerialize<Type1, Type2>; }
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeKeyValuePairMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<KeyValuePair<Type1, Type2>>)BinaryDeSerializer.KeyValuePairDeSerialize<Type1, Type2>; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeKeyValuePairMethod
        {
            get { return (Action<BinarySerializer, KeyValuePair<Type1, Type2>>)BinarySerializer.keyValuePairSerialize<Type1, Type2>; }
        }
    }
}
