using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpServer.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct CommandPoolTimeout
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// TCP 客户端命令池计数超时事件
        /// </summary>
        [FieldOffset(0)]
        public CommandPool.Timeout Value;
    }
}
