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
        internal abstract MethodInfo BinaryDeSerializeDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeKeyValuePairMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeSortedDictionaryMethod { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinaryDeSerializeSortedListMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeKeyValuePairMethod { get; }
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
            get { return ((deSerializeDictionary)GenericType.BinaryDeSerializer.dictionaryMember<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeDictionaryMethod
        {
            get { return ((deSerializeDictionary)GenericType.BinaryDeSerializer.dictionaryDeSerialize<Type1, Type2>).Method; }
        }

        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeSortedDictionary(ref SortedDictionary<Type1, Type2> value);
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeSortedDictionaryMemberMethod
        {
            get { return ((deSerializeSortedDictionary)GenericType.BinaryDeSerializer.sortedDictionaryMember<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeSortedDictionaryMethod
        {
            get { return ((deSerializeSortedDictionary)GenericType.BinaryDeSerializer.sortedDictionaryDeSerialize<Type1, Type2>).Method; }
        }

        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeSortedList(ref SortedList<Type1, Type2> value);
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeSortedListMemberMethod
        {
            get { return ((deSerializeSortedList)GenericType.BinaryDeSerializer.sortedListMember<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeSortedListMethod
        {
            get { return ((deSerializeSortedList)GenericType.BinaryDeSerializer.sortedListDeSerialize<Type1, Type2>).Method; }
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeKeyValuePairMethod
        {
            get { return ((deSerializeKeyValuePair)GenericType.BinaryDeSerializer.keyValuePairDeSerialize<Type1, Type2>).Method; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeKeyValuePairMethod
        {
            get { return ((Action<KeyValuePair<Type1, Type2>>)GenericType.BinarySerializer.keyValuePairSerialize<Type1, Type2>).Method; }
        }
    }
}
