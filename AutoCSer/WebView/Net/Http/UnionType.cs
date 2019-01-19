using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// HTTP 配置
        /// </summary>
        [FieldOffset(0)]
        public Config Config;
        /// <summary>
        /// HTTP 服务
        /// </summary>
        [FieldOffset(0)]
        public Server Server;
        /// <summary>
        /// 基于安全连接的 HTTP 服务
        /// </summary>
        [FieldOffset(0)]
        public SslServer SslServer;
    }
}
