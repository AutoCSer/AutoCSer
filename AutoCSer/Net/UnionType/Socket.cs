using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Socket
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 套接字
        /// </summary>
        [FieldOffset(0)]
        public System.Net.Sockets.Socket Value;
    }
}
