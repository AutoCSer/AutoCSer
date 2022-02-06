using System;
using System.Runtime.InteropServices;

namespace AutoCSer.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct LongArray
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 文件日志
        /// </summary>
        [FieldOffset(0)]
        public long[] Value;
    }
}
