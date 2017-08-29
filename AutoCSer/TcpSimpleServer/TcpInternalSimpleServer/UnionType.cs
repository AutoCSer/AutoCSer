using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpInternalSimpleServer
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
        /// TCP 内部服务配置
        /// </summary>
        [FieldOffset(0)]
        public ServerAttribute ServerAttribute;
        /// <summary>
        /// TCP 内部服务端
        /// </summary>
        [FieldOffset(0)]
        public Server Server;
    }
}
