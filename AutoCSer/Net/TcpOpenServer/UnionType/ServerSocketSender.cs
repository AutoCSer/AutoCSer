using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpOpenServer.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ServerSocketSender
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// TCP 服务套接字数据发送
        /// </summary>
        [FieldOffset(0)]
        public TcpOpenServer.ServerSocketSender Value;
    }
}
