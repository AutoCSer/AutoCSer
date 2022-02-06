using System;
using System.Threading;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.SubBuffer
{
    /// <summary>
    /// 缓冲区池
    /// </summary>
    internal unsafe sealed class Pool
    {
        /// <summary>
        /// 最小缓冲区字节大小二进制位数
        /// </summary>
        private const int minBufferSizeBits = 8;
        /// <summary>
        /// 缓冲区字节大小二进制位数
        /// </summary>
        private const int bufferSizeBits = 17;
        /// <summary>
        /// 128KB 避免 GC 压缩
        /// </summary>
        private const int bufferSize = 1 << bufferSizeBits;

        /// <summary>
        /// 缓冲区数组访问锁
        /// </summary>
        private readonly object bufferLock = new object();
        /// <summary>
        /// 缓冲区数组
        /// </summary>
        internal byte[][] Buffers;
        /// <summary>
        /// 空闲缓冲区起始位置
        /// </summary>
        private byte* freeStart;
        /// <summary>
        /// 当前空闲缓冲区
        /// </summary>
        private uint* freeCurrent;
        /// <summary>
        /// 空闲缓冲区结束为止
        /// </summary>
        private byte* freeEnd;
        /// <summary>
        /// 备用空闲缓冲区
        /// </summary>
        private byte* backFree;
        /// <summary>
        /// 创建空闲缓冲区访问锁
        /// </summary>
        private readonly object createFreeLock = new object();
        /// <summary>
        /// 空闲缓冲区访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock freeBufferLock;
        /// <summary>
        /// 当前缓冲区索引
        /// </summary>
        private int bufferIndex;
        /// <summary>
        /// 缓冲区数量
        /// </summary>
        private uint bufferCount;
        /// <summary>
        /// 缓冲区字节大小
        /// </summary>
        internal readonly int Size;
        /// <summary>
        /// 缓冲区字节大小二进制位数
        /// </summary>
        private readonly int sizeBits;
        /// <summary>
        /// 缓冲区分块数量二进制位数
        /// </summary>
        private readonly int arrayBits;
        /// <summary>
        /// 缓冲区分块数量
        /// </summary>
        private readonly uint arrayBufferCount;
        /// <summary>
        /// 缓冲区分块数量 and 值
        /// </summary>
        internal readonly uint ArrayIndexMark;
        /// <summary>
        /// 缓冲区池
        /// </summary>
        /// <param name="sizeBits">缓冲区字节大小二进制位数</param>
        private Pool(int sizeBits)
        {
            arrayBits = bufferSizeBits - sizeBits;
            Size = 1 << sizeBits;
            arrayBufferCount = 1U << arrayBits;
            this.sizeBits = sizeBits;
            ArrayIndexMark = arrayBufferCount - 1;
            Buffers = new byte[256][];
            *(byte**)(freeStart = UnmanagedPool.Default.GetPointer().Byte) = null;
            freeEnd = freeStart + UnmanagedPool.DefaultSize;
            freeCurrent = (uint*)(freeStart += sizeof(byte**));
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <returns>缓冲区</returns>
        internal void Get(ref PoolBuffer buffer)
        {
            freeBufferLock.EnterYield();
            if (freeStart != freeCurrent)
            {
                uint freeIndex = *--freeCurrent;
                if (freeStart == freeCurrent)
                {
                    byte* free = freeStart - sizeof(byte**);
                    if (*(byte**)free == null) freeBufferLock.Exit();
                    else
                    {
                        freeStart = *(byte**)free;
                        freeEnd = freeStart + (UnmanagedPool.DefaultSize - sizeof(byte**));
                        freeCurrent = (uint*)freeEnd;
                        if (backFree == null)
                        {
                            freeBufferLock.Exit();
                            Monitor.Enter(createFreeLock);
                            freeBufferLock.EnterYield();
                            if (backFree == null)
                            {
                                backFree = free;
                                freeBufferLock.Exit();
                                Monitor.Exit(createFreeLock);
                            }
                            else
                            {
                                freeBufferLock.Exit();
                                Monitor.Exit(createFreeLock);
                                UnmanagedPool.Default.Push(free);
                            }
                        }
                        else
                        {
                            freeBufferLock.Exit();
                            UnmanagedPool.Default.Push(free);
                        }
                    }
                }
                else freeBufferLock.Exit();
                buffer.Set(this, freeIndex);
                return;
            }
            freeBufferLock.Exit();
            uint index = (uint)System.Threading.Interlocked.Increment(ref bufferIndex) - 1;
            if (index >= bufferCount)
            {
                Monitor.Enter(bufferLock);
                try
                {
                    while (index >= bufferCount)
                    {
                        int arrayIndex = (int)(bufferCount >> arrayBits);
                        if (arrayIndex == Buffers.Length) Buffers = Buffers.copyNew(arrayIndex << 1);
                        Buffers[arrayIndex] = new byte[bufferSize];
                        bufferCount += arrayBufferCount;
                    }
                }
                finally { Monitor.Exit(bufferLock); }
            }
            buffer.Set(this, index);
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Get(ref PoolBufferFull buffer)
        {
            Get(ref buffer.PoolBuffer);
            uint index = buffer.PoolBuffer.Index;
            buffer.Set(Buffers[(int)(index >> arrayBits)], (int)(index & ArrayIndexMark) << sizeBits);
        }
        /// <summary>
        /// 添加缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        internal void Push(ref PoolBuffer buffer)
        {
            if (buffer.Pool == this)
            {
            START:
                freeBufferLock.EnterYield();
                if (freeCurrent == freeEnd)
                {
                    if (backFree == null)
                    {
                        freeBufferLock.Exit();
                        Monitor.Enter(createFreeLock);
                        if (backFree != null)
                        {
                            Monitor.Exit(createFreeLock);
                            goto START;
                        }
                        byte* newBackFree;
                        try
                        {
                            newBackFree = UnmanagedPool.Default.GetPointer().Byte;
                        }
                        finally { Monitor.Exit(createFreeLock); }
                        freeBufferLock.EnterYield();
                        backFree = newBackFree;
                        if (freeCurrent != freeEnd) goto PUSH;
                    }
                    *(byte**)backFree = freeStart;
                    freeEnd = backFree + UnmanagedPool.DefaultSize;
                    freeCurrent = (uint*)(freeStart = backFree + sizeof(byte**));
                    backFree = null;
                }
            PUSH:
                *freeCurrent++ = buffer.Index;
                freeBufferLock.Exit();
                buffer.Pool = null;
            }
        }
        /// <summary>
        /// 添加缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Push(ref PoolBufferFull buffer)
        {
            Push(ref buffer.PoolBuffer);
            buffer.Buffer = null;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        private void clear()
        {
            //XXX new Pool ?
        }

        /// <summary>
        /// 缓冲区池
        /// </summary>
        private static readonly Pool[] pools = new Pool[bufferSizeBits - minBufferSizeBits + 1];
        /// <summary>
        /// 缓冲区池访问锁
        /// </summary>
        private static readonly object poolLock = new object();
        /// <summary>
        /// 获取缓冲区池
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Pool GetPool(AutoCSer.Memory.BufferSize size)
        {
            int bits = ((uint)(int)size).DeBruijnLog2(), index = bits - minBufferSizeBits;
            if ((uint)index >= pools.Length) throw new IndexOutOfRangeException(size.ToString());
            return getPool(bits, index);
        }
        /// <summary>
        /// 获取缓冲区池
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static Pool getPool(int bits, int index)
        {
            Pool pool = pools[index];
            if (pool == null)
            {
                Monitor.Enter(poolLock);
                try
                {
                    if ((pool = pools[index]) == null) pools[index] = pool = new Pool(bits);
                }
                finally { Monitor.Exit(poolLock); }
            }
            return pool;
        }
        /// <summary>
        /// 获取缓冲区池
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Pool getPool(int index)
        {
            return getPool(index + minBufferSizeBits, index);
        }
        /// <summary>
        /// 获取缓冲区池
        /// </summary>
        /// <param name="size">缓冲区字节大小</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Pool GetPool(int size)
        {
            if (size <= (int)AutoCSer.Memory.BufferSize.Kilobyte4)
            {
                if (size <= (int)AutoCSer.Memory.BufferSize.Kilobyte)
                {
                    if (size <= (int)AutoCSer.Memory.BufferSize.Byte256) return getPool(0);
                    return getPool(size <= (int)AutoCSer.Memory.BufferSize.Byte512 ? 1 : 2);
                }
                return getPool(size <= (int)AutoCSer.Memory.BufferSize.Kilobyte2 ? 3 : 4);
            }
            if (size <= (int)AutoCSer.Memory.BufferSize.Kilobyte128)
            {
                if (size <= (int)AutoCSer.Memory.BufferSize.Kilobyte32)
                {
                    if (size <= (int)AutoCSer.Memory.BufferSize.Kilobyte8) return getPool(5);
                    return getPool(size <= (int)AutoCSer.Memory.BufferSize.Kilobyte16 ? 6 : 7);
                }
                return getPool(size <= (int)AutoCSer.Memory.BufferSize.Kilobyte64 ? 8 : 9);
            }
            return null;
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void GetBuffer(ref PoolBufferFull buffer, int size)
        {
            Pool pool = GetPool(size);
            if (pool == null) buffer.Set(new byte[size], 0);
            else pool.Get(ref buffer);
        }
        /// <summary>
        /// 获取独立缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void GetSingleBuffer(ref PoolBufferFull buffer, int size)
        {
            GetBuffer(ref buffer, Math.Max(size, bufferSize));
        }
        ///// <summary>
        ///// 清除缓存数据
        ///// </summary>
        //private static void clearCache()
        //{
        //    foreach (Pool pool in pools)
        //    {
        //        if (pool != null) pool.clear();
        //    }
        //}

        static Pool()
        {
            //AutoCSer.Memory.Common.AddClearCache(clearCache);
        }
    }
}
