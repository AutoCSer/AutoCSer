using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpStreamServer
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
        /// TCP 客户端命令
        /// </summary>
        [FieldOffset(0)]
        public ClientCommand.Command ClientCommand;
    }
}
