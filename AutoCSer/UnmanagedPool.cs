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
        /// 微型缓冲区池字节大小 256B
        /// </summary>
        public const int TinySize = 256;
        /// <summary>
        /// 默认缓冲区池字节大小 4KB
        /// </summary>
        public const int DefaultSize = 4 << 10;
        /// <summary>
        /// 空闲内存地址
        /// </summary>
        private byte* free;
        /// <summary>
        /// 空闲内存地址访问锁
        /// </summary>
        private int freeLock;
        /// <summary>
        /// 缓冲区尺寸
        /// </summary>
        public readonly int Size;
        /// <summary>
        /// 内存池
        /// </summary>
        /// <param name="size">缓冲区尺寸</param>
        public UnmanagedPool(int size)
        {
            Size = size;
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <returns>缓冲区,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public byte* TryGet()
        {
            while (System.Threading.Interlocked.CompareExchange(ref freeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.UnmanagedPoolFreePop);
            if (free != null)
            {
                byte* value = free;
                free = *(byte**)free;
                System.Threading.Interlocked.Exchange(ref freeLock, 0);
                return value;
            }
            System.Threading.Interlocked.Exchange(ref freeLock, 0);
            return null;
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <returns>缓冲区</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte* Get()
        {
            byte* data = TryGet();
            return data != null ? data : (byte*)Unmanaged.Get(Size, false);
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="minSize">数据字节长度</param>
        /// <returns>缓冲区</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Pointer.Size GetSize64(int minSize)
        {
            return new Pointer.Size { Data = minSize <= Size ? Get() : (byte*)Unmanaged.Get64(minSize, false), ByteSize = minSize };
        }
        /// <summary>
        /// 保存缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Push(byte* buffer)
        {
            while (System.Threading.Interlocked.CompareExchange(ref freeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.UnmanagedPoolFreePush);
            *(byte**)buffer = free;
            free = buffer;
            System.Threading.Interlocked.Exchange(ref freeLock, 0);
        }
        /// <summary>
        /// 保存缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PushOnly(ref Pointer.Size buffer)
        {
            if (buffer.ByteSize == Size) Push(buffer.Byte);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private void clear(int count)
        {
            while (System.Threading.Interlocked.CompareExchange(ref freeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.UnmanagedPoolFreePop);
            byte* head = free;
            free = null;
            System.Threading.Interlocked.Exchange(ref freeLock, 0);
            if (head != null)
            {
                if (count == 0) clear(head);
                else
                {
                    byte* end = head;
                    while (--count != 0)
                    {
                        if (*(byte**)end == null)
                        {
                            while (System.Threading.Interlocked.CompareExchange(ref freeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.UnmanagedPoolFreePush);
                            *(byte**)end = free;
                            free = head;
                            System.Threading.Interlocked.Exchange(ref freeLock, 0);
                            return;
                        }
                        end = *(byte**)end;
                    }
                    byte* next = *(byte**)end;
                    while (System.Threading.Interlocked.CompareExchange(ref freeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.UnmanagedPoolFreePush);
                    *(byte**)end = free;
                    free = head;
                    System.Threading.Interlocked.Exchange(ref freeLock, 0);
                    if (next != null) clear(next);
                }
            }
        }
        /// <summary>
        /// 释放缓冲区
        /// </summary>
        /// <param name="head"></param>
        private void clear(byte* head)
        {
            while (*(byte**)head != null)
            {
                byte* next = *(byte**)head;
                Unmanaged.Free(head, Size);
                if (*(byte**)next == null)
                {
                    head = next;
                    break;
                }
                head = *(byte**)next;
                Unmanaged.Free(next, Size);
            }
            Unmanaged.Free(head, Size);
        }

        /// <summary>
        /// 内存池
        /// </summary>
        private static readonly Dictionary<int, UnmanagedPool> pools;
        /// <summary>
        /// 内存池访问锁
        /// </summary>
        private static readonly object poolLock = new object();
        /// <summary>
        /// 微型缓冲区池(256B)
        /// </summary>
        public static readonly UnmanagedPool Tiny;
        /// <summary>
        /// 默认缓冲区池(4KB)
        /// </summary>
        public static readonly UnmanagedPool Default;
        /// <summary>
        /// 获取临时缓冲区
        /// </summary>
        /// <param name="length">缓冲区字节长度</param>
        /// <returns>临时缓冲区</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static UnmanagedPool GetDefaultPool(int length)
        {
            return length <= TinySize ? Tiny : Default;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        internal static void ClearCache(int count)
        {
            UnmanagedPool[] poolArray;
            Monitor.Enter(poolLock);
            try
            {
                poolArray = pools.Values.getArray();
            }
            finally { Monitor.Exit(poolLock); }
            foreach (UnmanagedPool pool in poolArray) pool.clear(count);
        }

        static UnmanagedPool()
        {
            pools = AutoCSer.DictionaryCreator.CreateInt<UnmanagedPool>();
            pools.Add(TinySize, Tiny = new UnmanagedPool(TinySize));
            pools.Add(DefaultSize, Default = new UnmanagedPool(DefaultSize));
        }
    }
}
