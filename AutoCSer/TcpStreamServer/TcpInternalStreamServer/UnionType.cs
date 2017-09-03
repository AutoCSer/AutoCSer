using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpInternalStreamServer
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
        /// TCP 内部服务客户端套接字数据发送
        /// </summary>
        [FieldOffset(0)]
        public ClientSocketSender ClientSocketSender;
        /// <summary>
        /// TCP 内部服务配置
        /// </summary>
        [FieldOffset(0)]
        public ServerAttribute ServerAttribute;
    }
}
