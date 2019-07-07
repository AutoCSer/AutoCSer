using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 数据缓冲区计数
    /// </summary>
    internal sealed class BufferCount
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        internal const SubBuffer.Size BufferSize = SubBuffer.Size.Kilobyte128;
        /// <summary>
        /// 缓冲区池
        /// </summary>
        private static readonly AutoCSer.SubBuffer.Pool bufferPool = AutoCSer.SubBuffer.Pool.GetPool(BufferSize);

        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal AutoCSer.SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 当前计数
        /// </summary>
        internal int Count;
        /// <summary>
        /// 当前位置
        /// </summary>
        private int index;
        /// <summary>
        /// 可用大小
        /// </summary>
        internal int Size;
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        internal BufferCount()
        {
            bufferPool.Get(ref Buffer);
            index = Buffer.StartIndex;
            Size = (int)BufferSize;
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
        /// <summary>
        /// 获取数据缓冲区
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Buffer get(int size)
        {
            if (this.Size >= size)
            {
                Buffer buffer = new Buffer(this, index, size);
                index += size;
                this.Size -= size;
                Interlocked.Increment(ref Count);
                return buffer;
            }
            return null;
        }
        /// <summary>
        /// 获取数据缓冲区
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal BufferCount Get(ref SubArray<byte> data)
        {
            if (Size >= data.Length)
            {
                data.Set(Buffer.Buffer, index, data.Length);
                index += data.Length;
                Size -= data.Length;
                Interlocked.Increment(ref Count);
                return this;
            }
            return null;
        }

        /// <summary>
        /// 当前分配缓冲区
        /// </summary>
        private static BufferCount bufferCount = new BufferCount();
        /// <summary>
        /// 当前分配缓冲区访问锁
        /// </summary>
        private static int bufferCountLock;
        /// <summary>
        /// 当前分配缓冲区创建访问锁
        /// </summary>
        private static readonly object newBufferCountLock = new object();
        /// <summary>
        /// 获取数据缓冲区
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal static Buffer GetBuffer(int size)
        {
            if (size > (int)BufferSize) return new Buffer(size);
            Buffer buffer;
            while (System.Threading.Interlocked.CompareExchange(ref bufferCountLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.CacheServerGetBuffer);
            try
            {
                buffer = bufferCount.get(size);
            }
            finally { System.Threading.Interlocked.Exchange(ref bufferCountLock, 0); }

            if (buffer == null)
            {
                byte step = 0xff;
                BufferCount newBufferCount = null, oldBufferCount;
                Monitor.Enter(newBufferCountLock);
                oldBufferCount = bufferCount;
                try
                {
                    while (System.Threading.Interlocked.CompareExchange(ref bufferCountLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.CacheServerGetBuffer);
                    step = 0;
                    buffer = oldBufferCount.get(size);
                    System.Threading.Interlocked.Exchange(ref bufferCountLock, 0);
                    step = 0xff;

                    if (buffer == null)
                    {
                        newBufferCount = new BufferCount();
                        step = 1;
                        buffer = newBufferCount.get(size);
                        if (newBufferCount.Size > oldBufferCount.Size)
                        {
                            step = 0xff;
                            bufferCount = newBufferCount;
                            oldBufferCount.Free();
                        }
                    }
                }
                finally
                {
                    switch (step)
                    {
                        case 0: System.Threading.Interlocked.Exchange(ref bufferCountLock, 0); break;
                        case 1: newBufferCount.Free(); break;
                    }
                    Monitor.Exit(newBufferCountLock);
                }
            }
            return buffer;
        }
    }
}
