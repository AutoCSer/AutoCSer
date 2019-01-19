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
        /// 申请非托管内存
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Pointer.Size GetSize64(int size, bool isClear = false)
        {
            return GetSizeUnsafe64((size + 7) & (int.MaxValue - 7), isClear);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void FreeStatic(ref Pointer.Size data)
        {
            if (data.Data != null)
            {
                Marshal.FreeHGlobal((IntPtr)data.Data);
                Interlocked.Add(ref staticSize, -data.ByteSize);
                data.SetNull();
            }
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isStaticUnmanaged"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Free(ref Pointer.Size data, bool isStaticUnmanaged)
        {
            if (isStaticUnmanaged) FreeStatic(ref data);
            else Free(ref data);
        }
    }
}
