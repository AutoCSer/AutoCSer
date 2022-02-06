using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpInternalServer.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ClientSocketSender
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// TCP 内部服务客户端套接字数据发送
        /// </summary>
        [FieldOffset(0)]
        public TcpInternalServer.ClientSocketSender Value;
    }
}
