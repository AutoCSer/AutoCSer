﻿using System;
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
        internal abstract Delegate JsonSerializeStructEnumerableMethod { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Delegate JsonSerializeEnumerableMethod { get; }
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
        /// 获取当前类型
        /// </summary>
        internal override TypeKey CurrentType { get { return new TypeKey(typeof(Type1), typeof(Type2)); } }

        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Delegate JsonSerializeStructEnumerableMethod
        {
            get { return (Action<JsonSerializer, Type1>)JsonSerializer.StructEnumerable<Type1, Type2>; }
        }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Delegate JsonSerializeEnumerableMethod
        {
            get { return (Action<JsonSerializer, Type1>)JsonSerializer.Enumerable<Type1, Type2>; }
        }
    }
}
