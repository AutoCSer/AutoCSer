using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存池
    /// </summary>
    public unsafe sealed class UnmanagedPool
    {
        /// <summary>
        /// 空闲内存地址
        /// </summary>
        private byte* free;
        /// <summary>
        /// 空闲内存地址访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock freeLock;
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
        private byte* tryGet()
        {
            freeLock.EnterYield();
            if (free != null)
            {
                byte* value = free;
                free = *(byte**)free;
                freeLock.Exit();
                return value;
            }
            freeLock.Exit();
            return null;
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <returns>缓冲区</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Pointer GetPointer()
        {
            byte* data = tryGet();
            return data != null ? new Pointer(data, Size) : Unmanaged.GetPointer(Size, false);
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="minSize">数据字节长度</param>
        /// <returns>缓冲区</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Pointer GetMinSize(int minSize)
        {
            return minSize <= Size ? GetPointer() : Unmanaged.GetPointer(minSize, false);
        }
        /// <summary>
        /// 保存缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        internal void Push(byte* buffer)
        {
#if DEBUG
            if (buffer == null) throw new Exception("buffer == null");
#endif
            freeLock.EnterYield();
            *(byte**)buffer = free;
            free = buffer;
            freeLock.Exit();
        }
        /// <summary>
        /// 保存缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PushOnly(ref Pointer buffer)
        {
            Push(buffer.Byte);
        }
        /// <summary>
        /// 保存缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        public void Push(ref Pointer buffer)
        {
            int size = System.Threading.Interlocked.Exchange(ref buffer.ByteSize, 0);
            if (size != 0)
            {
                if (size == Size) Push((byte*)buffer.GetDataClearOnly());
                else Unmanaged.Free(ref buffer, size);
            }
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private void clear(int count)
        {
            freeLock.EnterYield();
            byte* head = free;
            free = null;
            freeLock.Exit();
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
                            freeLock.EnterYield();
                            *(byte**)end = free;
                            free = head;
                            freeLock.Exit();
                            return;
                        }
                        end = *(byte**)end;
                    }
                    byte* next = *(byte**)end;
                    freeLock.EnterYield();
                    *(byte**)end = free;
                    free = head;
                    freeLock.Exit();
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
                Unmanaged.FreePool(head, Size);
                if (*(byte**)next == null)
                {
                    head = next;
                    break;
                }
                head = *(byte**)next;
                Unmanaged.FreePool(next, Size);
            }
            Unmanaged.FreePool(head, Size);
        }

        /// <summary>
        /// 微型缓冲区池字节大小 256B
        /// </summary>
        public const int TinySize = 256;
        /// <summary>
        /// 微型缓冲区池(256B)
        /// </summary>
        public static readonly UnmanagedPool Tiny;
        /// <summary>
        /// 默认缓冲区池字节大小 4KB
        /// </summary>
        public const int DefaultSize = 4 << 10;
        /// <summary>
        /// 默认缓冲区池(4KB)
        /// </summary>
        public static readonly UnmanagedPool Default;
        /// <summary>
        /// 基数排序计数缓冲区字节大小 8KB
        /// </summary>
        internal const int RadixSortCountBufferSize = 256 * 8 * sizeof(int);
        /// <summary>
        /// 基数排序计数缓冲区池(4KB)
        /// </summary>
        internal static readonly UnmanagedPool RadixSortCountBuffer;
        /// <summary>
        /// 获取临时缓冲区
        /// </summary>
        /// <param name="length">缓冲区字节长度</param>
        /// <returns>临时缓冲区</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static UnmanagedPool GetPool(int length)
        {
            return length <= DefaultSize ? (length <= TinySize ? Tiny : Default) : RadixSortCountBuffer;
        }
        /// <summary>
        /// LZW压缩编码查询表缓冲区(2MB)
        /// </summary>
        internal static readonly UnmanagedPool LzwEncodeTableBuffer;

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        internal static void ClearCache(int count)
        {
            Tiny.clear(count);
            Default.clear(count);
            RadixSortCountBuffer.clear(count);
            LzwEncodeTableBuffer.clear(count);
        }
        static UnmanagedPool()
        {
            Tiny = new UnmanagedPool(TinySize);
            Default = new UnmanagedPool(DefaultSize);
            RadixSortCountBuffer = new UnmanagedPool(RadixSortCountBufferSize);
            LzwEncodeTableBuffer = new UnmanagedPool(4096 * 256 * 2);
        }
    }
}
