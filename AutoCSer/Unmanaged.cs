using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 非托管内存
    /// </summary>
    public unsafe static partial class Unmanaged
    {
        /// <summary>
        /// 8个0字节（公用）
        /// </summary>
        internal static Pointer NullByte8 = new Pointer { Data = GetStatic64(8, true) };
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
        internal static void* GetStatic64(int size, bool isClear)
        {
            void* data = (void*)Marshal.AllocHGlobal(size);
            Interlocked.Add(ref staticSize, size);
            if (isClear) Memory.ClearUnsafe((ulong*)data, size >> 3);
            return data;
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void FreeStatic(ref byte* data, int size)
        {
            if (data != null)
            {
                Marshal.FreeHGlobal((IntPtr)data);
                data = null;
                Interlocked.Add(ref staticSize, -size);
            }
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
            if (isClear) Memory.Clear((byte*)data, size);
            return data;
        }
        /// <summary>
        /// 申请非托管内存
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        internal static void* Get64(int size, bool isClear)
        {
            void* data = (void*)Marshal.AllocHGlobal(size);
            Interlocked.Add(ref totalSize, size);
            if (isClear) Memory.ClearUnsafe((ulong*)data, size >> 3);
            return data;
        }
        /// <summary>
        /// 申请非托管内存
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Pointer.Size GetSizeUnsafe64(int size, bool isClear)
        {
            return new Pointer.Size { Data = Get64(size, isClear), ByteSize = size };
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Free(byte* data, int size)
        {
            Marshal.FreeHGlobal((IntPtr)data);
            Interlocked.Add(ref totalSize, -size);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Free(ref Pointer.Size data)
        {
            if (data.Data != null)
            {
                Marshal.FreeHGlobal((IntPtr)data.Data);
                Interlocked.Add(ref totalSize, -data.ByteSize);
                data.SetNull();
            }
        }
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
            if (isClear) Memory.Clear((byte*)data, size);
            return data;
        }
        /// <summary>
        /// 申请非托管内存
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isClear"></param>
        /// <param name="isStaticUnmanaged"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Pointer.Size Get(int size, bool isClear, bool isStaticUnmanaged)
        {
            return new Pointer.Size { Data = isStaticUnmanaged ? GetStatic(size, isClear) : Get(size, isClear), ByteSize = size };
        }
        ///// <summary>
        ///// 批量申请非托管内存
        ///// </summary>
        ///// <param name="isClear">是否需要清除</param>
        ///// <param name="sizes">内存字节数集合</param>
        ///// <returns>非托管内存起始指针</returns>
        //internal static Pointer[] GetStatic(bool isClear, params int[] sizes)
        //{
        //    int sum = 0, index = 0;
        //    foreach (int size in sizes) sum += size;
        //    byte* data = (byte*)GetStatic(sum, isClear);
        //    Pointer[] datas = new Pointer[sizes.Length];
        //    foreach (int size in sizes)
        //    {
        //        datas[index++].Data = data;
        //        data += size;
        //    }
        //    return datas;
        //}
    }
}
