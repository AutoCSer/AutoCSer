using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class EnumerableGenericType2
    {
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeStructEnumerableMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract MethodInfo JsonSerializeEnumerableMethod { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="Type1">泛型类型</typeparam>
    /// <typeparam name="Type2">泛型类型</typeparam>
    internal sealed partial class EnumerableGenericType2<Type1, Type2> : EnumerableGenericType2
         where Type1 : IEnumerable<Type2>
    {
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeStructEnumerableMethod
        {
            get { return ((Action<Type1>)GenericType.JsonSerializer.structEnumerable<Type1, Type2>).Method; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override MethodInfo JsonSerializeEnumerableMethod
        {
            get { return ((Action<Type1>)GenericType.JsonSerializer.enumerable<Type1, Type2>).Method; }
        }
    }
}
