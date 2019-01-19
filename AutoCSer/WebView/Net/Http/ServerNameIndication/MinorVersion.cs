using System;

namespace AutoCSer.Net.Http.ServerNameIndication
{
    /// <summary>
    /// TLS 版本（大版本为 3） Byte[1]
    /// </summary>
    internal enum MinorVersion : byte
    {
        /// <summary>
        /// SSL 3
        /// </summary>
        SSL3 = 0,
        /// <summary>
        /// TLS 1.0
        /// </summary>
        TLS1 = 1,
        /// <summary>
        /// TLS 1.1
        /// </summary>
        TLS11 = 2,
        /// <summary>
        /// TLS 1.2
        /// </summary>
        TLS12 = 3,
    }
}
