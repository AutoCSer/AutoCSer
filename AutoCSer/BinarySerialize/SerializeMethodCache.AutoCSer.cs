using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    internal static partial class SerializeMethodCache
    {
        /// <summary>
        /// 数组序列化函数信息
        /// </summary>
        internal static readonly MethodInfo SubArraySerializeMethod = typeof(Serializer).GetMethod("subArraySerialize", BindingFlags.Instance | BindingFlags.NonPublic);
    }
}
