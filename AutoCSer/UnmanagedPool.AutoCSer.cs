using System;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存池
    /// </summary>
    public unsafe sealed partial class UnmanagedPool
    {
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="minSize">数据字节长度</param>
        /// <returns>缓冲区</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Pointer.Size GetSize(int minSize)
        {
            return new Pointer.Size { Data = minSize <= Size ? Get() : (byte*)Unmanaged.Get(minSize, false), ByteSize = minSize };
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <returns>缓冲区</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Pointer.Size GetSize()
        {
            return new Pointer.Size { Data = Get(), ByteSize = Size };
        }
        /// <summary>
        /// 保存缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Push(ref Pointer.Size buffer)
        {
            byte* data = buffer.Byte;
            buffer.Data = null;
            if (data != null && buffer.ByteSize == Size) Push(data);
        }
        /// <summary>
        /// 获取非托管内存池
        /// </summary>
        /// <param name="size">缓冲区字节大小</param>
        /// <returns>非托管内存池</returns>
        public static UnmanagedPool GetOrCreate(int size)
        {
            if (size <= 0) throw new IndexOutOfRangeException("size" + size.toString() + " <= 0");
            UnmanagedPool pool;
            Monitor.Enter(poolLock);
            if (pools.TryGetValue(size, out pool)) Monitor.Exit(poolLock);
            else
            {
                try
                {
                    pools.Add(size, pool = new UnmanagedPool(size));
                }
                finally { Monitor.Exit(poolLock); }
            }
            return pool;
        }
    }
}
