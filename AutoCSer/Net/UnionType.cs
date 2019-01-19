using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace AutoCSer.Net
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
        /// 套接字
        /// </summary>
        [FieldOffset(0)]
        public Socket Socket;
#if !DOTNET2
        /// <summary>
        /// 套接字异步事件对象
        /// </summary>
        [FieldOffset(0)]
        public SocketAsyncEventArgs SocketAsyncEventArgs;
#endif
    }
}
