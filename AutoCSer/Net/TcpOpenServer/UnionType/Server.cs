using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpOpenServer.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct Server
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// TCP 服务端
        /// </summary>
        [FieldOffset(0)]
        public TcpOpenServer.Server Value;
    }
}
