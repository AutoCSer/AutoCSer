using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存
    /// </summary>
    public unsafe static class Unmanaged
    {
        /// <summary>
        /// 8个0字节（公用）
        /// </summary>
        internal static Pointer NullByte8 = GetStaticPointer(8, true);

        /// <summary>
        /// 不释放的固定内存申请大小
        /// </summary>
        private static long staticSize;
        /// <summary>
        /// 静态类型初始化申请非托管内存(不释放的固定内存)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns></returns>
        internal static void* GetStatic(int size, bool isClear)
        {
            void* data = (void*)Marshal.AllocHGlobal(size);
            Interlocked.Add(ref staticSize, size);
            if (isClear) new Span<byte>(data, size).Clear();
            return data;
        }
        /// <summary>
        /// 静态类型初始化申请非托管内存(不释放的固定内存)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isClear"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Pointer GetStaticPointer(int size, bool isClear)
        {
            return new Pointer(GetStatic(size, isClear), size);
        }
        /// <summary>
        /// 静态类型初始化申请非托管内存(不释放的固定内存)（8字节补齐）
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isClear"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Pointer GetStaticPointer8(int size, bool isClear)
        {
            return GetStaticPointer((size + 7) & (int.MaxValue - 7), isClear);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        public static void FreeStatic(ref Pointer data)
        {
            int size = Interlocked.Exchange(ref data.ByteSize, 0);
            if (size != 0) FreeStatic(ref data, size);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void FreeStatic(ref Pointer data, int size)
        {
            Marshal.FreeHGlobal((IntPtr)data.GetDataClearOnly());
            Interlocked.Add(ref staticSize, -size);
        }

        /// <summary>
        /// 非托管内存申请字节数
        /// </summary>
        private static long totalSize;
        /// <summary>
        /// 申请非托管内存
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        internal static void* Get(int size, bool isClear)
        {
            void* data = (void*)Marshal.AllocHGlobal(size);
            Interlocked.Add(ref totalSize, size);
            if (isClear) new Span<byte>(data, size).Clear();
            return data;
        }
        /// <summary>
        /// 申请非托管内存
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Pointer GetPointer(int size, bool isClear)
        {
            return new Pointer(Get(size, isClear), size);
        }
        /// <summary>
        /// 申请非托管内存（8字节补齐）
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Pointer GetPointer8(int size, bool isClear)
        {
            return GetPointer((size + 7) & (int.MaxValue - 7), isClear);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        public static void Free(ref Pointer data)
        {
            int size = Interlocked.Exchange(ref data.ByteSize, 0);
            if (size != 0) Free(ref data, size);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Free(ref Pointer data, int size)
        {
            Marshal.FreeHGlobal((IntPtr)data.GetDataClearOnly());
            Interlocked.Add(ref totalSize, -size);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        internal static void Free(ref byte* data, int size)
        {
            if (data != null)
            {
                Marshal.FreeHGlobal((IntPtr)data);
                data = null;
                Interlocked.Add(ref totalSize, -size);
            }
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        internal static void FreePool(byte* data, int size)
        {
            Marshal.FreeHGlobal((IntPtr)data);
            Interlocked.Add(ref totalSize, -size);
        }
    }
}
