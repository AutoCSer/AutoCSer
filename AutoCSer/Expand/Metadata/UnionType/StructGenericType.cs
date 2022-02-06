using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Expand.Metadata.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct StructGenericType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 结构体泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public Metadata.StructGenericType Value;
    }
}
