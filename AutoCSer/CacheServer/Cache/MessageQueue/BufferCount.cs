using System;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息队列数据缓冲区计数
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct BufferCount
    {
        /// <summary>
        /// 消息标识
        /// </summary>
        internal ulong Identity;
        /// <summary>
        /// 参数数据
        /// </summary>
        private ValueData.Data data;
        /// <summary>
        /// 消息队列数据缓冲区计数
        /// </summary>
        private AutoCSer.CacheServer.BufferCount count;
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        private int isBufferCount;
        /// <summary>
        /// 消息队列数据缓冲区计数
        /// </summary>
        /// <param name="buffer">消息队列数据缓冲区</param>
        internal BufferCount(Buffer buffer)
        {
            data = buffer.Data;
            count = buffer.BufferCount;
            Identity = buffer.Identity;
            data.ReturnType = ReturnType.Success;
            isBufferCount = 0;
            buffer.BufferCount = null;
        }
        /// <summary>
        /// 获取消息队列数据缓冲区计数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.CacheServer.BufferCount Get(out ValueData.Data data)
        {
            data = this.data;
            if (count != null)
            {
                if (isBufferCount == 0) isBufferCount = 1;
                else Interlocked.Increment(ref count.Count);
                return count;
            }
            return null;
        }
        /// <summary>
        /// 释放数据缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeBuffer()
        {
            if (count != null && isBufferCount == 0) count.Free();
        }

        /// <summary>
        /// 当前分配缓冲区
        /// </summary>
        private static AutoCSer.CacheServer.BufferCount bufferCount = new AutoCSer.CacheServer.BufferCount();
        /// <summary>
        /// 当前分配缓冲区访问锁
        /// </summary>
        private static int bufferCountLock;
        /// <summary>
        /// 当前分配缓冲区创建访问锁
        /// </summary>
        private static readonly object newBufferCountLock = new object();
        /// <summary>
        /// 获取数据分配缓冲区
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static AutoCSer.CacheServer.BufferCount GetBufferCount(ref SubArray<byte> data)
        {
            if (data.Length > (int)AutoCSer.CacheServer.BufferCount.BufferSize)
            {
                data.Set(new byte[data.Length], 0, data.Length);
                return null;
            }
            AutoCSer.CacheServer.BufferCount buffer;
            while (System.Threading.Interlocked.CompareExchange(ref bufferCountLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.MessageQueueGetBuffer);
            buffer = bufferCount.Get(ref data);
            System.Threading.Interlocked.Exchange(ref bufferCountLock, 0);

            if (buffer == null)
            {
                Monitor.Enter(newBufferCountLock);
                AutoCSer.CacheServer.BufferCount oldBufferCount = bufferCount;
                try
                {
                    while (System.Threading.Interlocked.CompareExchange(ref bufferCountLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.MessageQueueGetBuffer);
                    buffer = oldBufferCount.Get(ref data);
                    System.Threading.Interlocked.Exchange(ref bufferCountLock, 0);
                    if (buffer == null)
                    {
                        AutoCSer.CacheServer.BufferCount newBufferCount = new AutoCSer.CacheServer.BufferCount();
                        buffer = newBufferCount.Get(ref data);
                        if (newBufferCount.Size > oldBufferCount.Size)
                        {
                            bufferCount = newBufferCount;
                            oldBufferCount.Free();
                        }
                        else newBufferCount.Free();
                    }
                }
                finally { Monitor.Exit(newBufferCountLock); }
            }
            return buffer;
        }
    }
}
