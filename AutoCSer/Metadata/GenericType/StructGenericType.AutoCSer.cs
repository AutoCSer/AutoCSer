using System;
using System.Reflection;
using AutoCSer.Threading;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal abstract partial class StructGenericType
    {
        /// <summary>
        /// 简单序列化预编译
        /// </summary>
        internal abstract void SimpleSerializeCompile();
        /// <summary>
        /// 简单反序列化预编译
        /// </summary>
        internal abstract void SimpleDeSerializeCompile();

        /// <summary>
        /// 扩展项目数据对象
        /// </summary>
        internal object Expand;
    }
    /// <summary>
    /// 结构体泛型类型元数据
    /// </summary>
    internal sealed partial class StructGenericType<Type> : StructGenericType where Type : struct
    {
        /// <summary>
        /// 简单序列化预编译
        /// </summary>
        internal override void SimpleSerializeCompile()
        {
            AutoCSer.Net.SimpleSerialize.TypeSerializer<Type>.Compile();
        }
        /// <summary>
        /// 简单反序列化预编译
        /// </summary>
        internal override void SimpleDeSerializeCompile()
        {
            AutoCSer.Net.SimpleSerialize.TypeDeSerializer<Type>.Compile();
        }
    }
}
