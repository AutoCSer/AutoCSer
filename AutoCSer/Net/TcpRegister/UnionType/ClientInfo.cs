using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpRegister.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ClientInfo
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 客户端信息
        /// </summary>
        [FieldOffset(0)]
        public TcpRegister.ClientInfo Value;
    }
}
