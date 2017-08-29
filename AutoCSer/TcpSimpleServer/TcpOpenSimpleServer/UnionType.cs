using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpOpenSimpleServer
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
        /// TCP 服务端
        /// </summary>
        [FieldOffset(0)]
        public Server Server;
        /// <summary>
        /// TCP 服务配置
        /// </summary>
        [FieldOffset(0)]
        public ServerAttribute ServerAttribute;
    }
}
