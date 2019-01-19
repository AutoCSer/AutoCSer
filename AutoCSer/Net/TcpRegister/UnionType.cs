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
        /// TCP 内部注册写服务配置
        /// </summary>
        [FieldOffset(0)]
        public Config Config;
    }
}
