using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.RawSocketListener
{
    /// <summary>
    /// 数据缓冲区计数
    /// </summary>
    internal sealed class BufferCount
    {
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal AutoCSer.SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 当前计数
        /// </summary>
        internal int Count;
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        internal BufferCount()
        {
            Listener.BufferPool.Get(ref Buffer);
            Count = 1;
        }
        /// <summary>
        /// 释放计数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            if (Interlocked.Decrement(ref Count) == 0) Buffer.Free();
        }
    }
}
