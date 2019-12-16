using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace AutoCSer.Net.TcpRegister
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
        /// 客户端信息
        /// </summary>
        [FieldOffset(0)]
        public ClientInfo ClientInfo;
    }
}
