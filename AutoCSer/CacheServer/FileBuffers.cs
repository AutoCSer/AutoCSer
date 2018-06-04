using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 文件缓冲区
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct FileBuffers
    {
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 压缩数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull CompressionBuffer;
        /// <summary>
        /// 压缩数据
        /// </summary>
        internal SubArray<byte> CompressionData;
        /// <summary>
        /// 释放数据缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            Buffer.Free();
            CompressionBuffer.TryFree();
        }
    }
}
