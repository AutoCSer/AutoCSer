using System;

namespace AutoCSer.SubBuffer
{
    /// <summary>
    /// 缓冲区字节大小
    /// </summary>
    public enum Size
    {
        /// <summary>
        /// 256B
        /// </summary>
        Byte256 = 256,
        /// <summary>
        /// 512B
        /// </summary>
        Byte512 = 512,
        /// <summary>
        /// 1KB
        /// </summary>
        Kilobyte = 1 << 10,
        /// <summary>
        /// 2KB
        /// </summary>
        Kilobyte2 = 2 << 10,
        /// <summary>
        /// 4KB
        /// </summary>
        Kilobyte4 = 4 << 10,
        /// <summary>
        /// 8KB
        /// </summary>
        Kilobyte8 = 8 << 10,
        /// <summary>
        /// 16KB
        /// </summary>
        Kilobyte16 = 16 << 10,
        /// <summary>
        /// 32KB
        /// </summary>
        Kilobyte32 = 32 << 10,
        /// <summary>
        /// 64KB
        /// </summary>
        Kilobyte64 = 64 << 10,
        /// <summary>
        /// 128KB
        /// </summary>
        Kilobyte128 = 128 << 10
    }
}
