using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace AutoCSer.TestCase.WebPerformance
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
        /// 套接字异步事件对象
        /// </summary>
        [FieldOffset(0)]
        public SocketAsyncEventArgs SocketAsyncEventArgs;
    }
}
