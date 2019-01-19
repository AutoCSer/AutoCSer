using System;
using System.Security.Authentication;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 配置
    /// </summary>
    public partial class Config
    {
        /// <summary>
        /// 默认 SSL 协议 Tls
        /// </summary>
        internal const SslProtocols SslProtocol = SslProtocols.Tls;
    }
}
