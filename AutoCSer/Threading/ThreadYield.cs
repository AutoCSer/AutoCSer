using System;
using AutoCSer.Extensions;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程操作
    /// </summary>
    public static class ThreadYield
    {
        /// <summary>
        /// .NET 4.0 之前的版本不做任何事
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void YieldOnly()
        {
#if !DOTNET2 && !UNITY3D
            System.Threading.Thread.Yield();
#endif
        }
        /// <summary>
        /// .NET 4.0 之前的版本调用 System.Threading.Thread.Sleep(0)
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Yield()
        {
#if DOTNET2 || UNITY3D
            System.Threading.Thread.Sleep(0);
#else
            System.Threading.Thread.Yield();
#endif
        }
    }
}
