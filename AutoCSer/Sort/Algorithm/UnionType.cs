using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 文件日志
        /// </summary>
        [FieldOffset(0)]
        public long[] LongArray;
    }
}
