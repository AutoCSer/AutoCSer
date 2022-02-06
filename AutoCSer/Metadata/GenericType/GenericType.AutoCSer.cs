using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType
    {
        /// <summary>
        /// 二进制序列化预编译
        /// </summary>
        internal abstract void BinarySerializeCompile();
        /// <summary>
        /// 二进制反序列化预编译
        /// </summary>
        internal abstract void BinaryDeSerializeCompile();

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeSerializeSubArrayMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeSubArrayMethod { get; }

        /// <summary>
        /// JSON 序列化预编译
        /// </summary>
        internal abstract void JsonSerializeCompile();
        /// <summary>
        /// JSON 反序列化预编译
        /// </summary>
        internal abstract void JsonDeSerializeCompile();
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType<T> : GenericType
    {
        /// <summary>
        /// 二进制序列化预编译
        /// </summary>
        internal override void BinarySerializeCompile()
        {
            AutoCSer.BinarySerialize.TypeSerializer<T>.Compile();
        }
        /// <summary>
        /// 二进制反序列化预编译
        /// </summary>
        internal override void BinaryDeSerializeCompile()
        {
            AutoCSer.BinarySerialize.TypeDeSerializer<T>.Compile();
        }

        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeSerializeSubArrayMethod
        {
            get { return (AutoCSer.BinaryDeSerializer.DeSerializeDelegate<SubArray<T>>)BinaryDeSerializer.SubArrayDeSerialize<T>; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinarySerializeSubArrayMethod
        {
            get { return (Action<BinarySerializer, SubArray<T>>)BinarySerializer.SubArraySerialize<T>; }
        }

        /// <summary>
        /// JSON 序列化预编译
        /// </summary>
        internal override void JsonSerializeCompile()
        {
            AutoCSer.Json.TypeSerializer<T>.Compile();
        }
        /// <summary>
        /// JSON 反序列化预编译
        /// </summary>
        internal override void JsonDeSerializeCompile()
        {
            AutoCSer.Json.TypeDeSerializer<T>.Compile();
        }
    }
}
