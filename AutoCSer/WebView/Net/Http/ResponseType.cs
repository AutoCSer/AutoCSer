using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 响应输出类型
    /// </summary>
    internal enum ResponseType : byte
    {
        /// <summary>
        /// 字节数组
        /// </summary>
        ByteArray,
        /// <summary>
        /// 字节子数组
        /// </summary>
        SubByteArray,
        /// <summary>
        /// 缓冲区
        /// </summary>
        SubBuffer,
        /// <summary>
        /// 文件
        /// </summary>
        File,
    }
}
