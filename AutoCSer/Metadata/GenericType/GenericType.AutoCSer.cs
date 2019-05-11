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
        internal abstract MethodInfo BinaryDeSerializeSubArrayMethod { get; }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo BinarySerializeSubArrayMethod { get; }

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
    internal sealed partial class GenericType<Type> : GenericType
    {
        /// <summary>
        /// 二进制序列化预编译
        /// </summary>
        internal override void BinarySerializeCompile()
        {
            AutoCSer.BinarySerialize.TypeSerializer<Type>.Compile();
        }
        /// <summary>
        /// 二进制反序列化预编译
        /// </summary>
        internal override void BinaryDeSerializeCompile()
        {
            AutoCSer.BinarySerialize.TypeDeSerializer<Type>.Compile();
        }

        /// <summary>
        /// 反序列化委托
        /// </summary>
        /// <param name="value">目标数据</param>
        internal delegate void deSerializeSubArray(ref SubArray<Type> value);
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinaryDeSerializeSubArrayMethod
        {
            get { return ((deSerializeSubArray)GenericType.BinaryDeSerializer.subArrayDeSerialize<Type>).Method; }
        }

        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo BinarySerializeSubArrayMethod
        {
            get { return ((Action<SubArray<Type>>)GenericType.BinarySerializer.subArraySerialize<Type>).Method; }
        }

        /// <summary>
        /// JSON 序列化预编译
        /// </summary>
        internal override void JsonSerializeCompile()
        {
            AutoCSer.Json.TypeSerializer<Type>.Compile();
        }
        /// <summary>
        /// JSON 反序列化预编译
        /// </summary>
        internal override void JsonDeSerializeCompile()
        {
            AutoCSer.Json.TypeParser<Type>.Compile();
        }
    }
}
