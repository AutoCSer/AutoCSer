using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Metadata.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ClassGenericType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 引用泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public Metadata.ClassGenericType Value;
    }
}
