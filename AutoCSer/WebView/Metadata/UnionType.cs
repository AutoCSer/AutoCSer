using System;
using System.Runtime.InteropServices;

namespace AutoCSer.WebView.Metadata
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public GenericType GenericType;
    }
}
