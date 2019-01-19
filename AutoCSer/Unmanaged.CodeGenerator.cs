using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace fastCSharp
{
    /// <summary>
    /// 非托管内存
    /// </summary>
    internal unsafe static partial class Unmanaged
    {
        /// <summary>
        /// 静态类型初始化申请非托管内存(不释放的固定内存)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static Pointer.Size GetStaticSize64(int size, bool isClear)
        {
            return new Pointer.Size { Data = GetStatic64(size, isClear), ByteSize = size };
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static void FreeStatic(ref Pointer.Size data)
        {
            if (data.Data != null)
            {
                Marshal.FreeHGlobal((IntPtr)data.Data);
                Interlocked.Add(ref staticSize, -data.ByteSize);
                data.Null();
            }
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static void FreeOnly(ref Pointer.Size data)
        {
            if (data.Data != null)
            {
                Marshal.FreeHGlobal((IntPtr)data.Data);
                Interlocked.Add(ref totalSize, -data.ByteSize);
            }
        }
    }
}
