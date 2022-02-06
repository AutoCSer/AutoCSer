using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Http.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct SslServer
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 基于安全连接的 HTTP 服务
        /// </summary>
        [FieldOffset(0)]
        public Http.SslServer Value;
    }
}
