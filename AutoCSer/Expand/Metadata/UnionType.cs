using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Expand.Metadata
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal partial struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 引用类型泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public ClassGenericType ClassGenericType;
        /// <summary>
        /// 结构体泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public StructGenericType StructGenericType;
    }
}
