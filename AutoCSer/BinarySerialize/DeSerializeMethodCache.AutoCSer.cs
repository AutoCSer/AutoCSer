using System;
using System.Reflection;
using System.Collections.Generic;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    internal static partial class DeSerializeMethodCache
    {
        /// <summary>
        /// 数组对象序列化函数信息
        /// </summary>
        internal static readonly MethodInfo SubArrayDeSerializeMethod = typeof(DeSerializer).GetMethod("subArrayDeSerialize", BindingFlags.Instance | BindingFlags.NonPublic);
    }
}
