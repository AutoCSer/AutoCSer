using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.TcpInternalServer
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
        /// TCP 内部服务端
        /// </summary>
        [FieldOffset(0)]
        public Server Server;
        /// <summary>
        /// TCP 内部服务配置
        /// </summary>
        [FieldOffset(0)]
        public ServerAttribute ServerAttribute;
        /// <summary>
        /// TCP 内部服务套接字数据发送
        /// </summary>
        [FieldOffset(0)]
        public ServerSocketSender ServerSocketSender;
        /// <summary>
        /// TCP 内部服务端套接字任务处理配置
        /// </summary>
        [FieldOffset(0)]
        public ServerSocketTaskConfig ServerSocketTaskConfig;
    }
}
