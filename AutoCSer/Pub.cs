using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 常用公共定义
    /// </summary>
    public static partial class Pub
    {
        /// <summary>
        /// LGD
        /// </summary>
        internal const int PuzzleValue = 0x10035113;
        /// <summary>
        /// 程序启用时间
        /// </summary>
        public static readonly DateTime StartTime = DateTime.Now;
        /// <summary>
        /// 默认自增标识
        /// </summary>
        private static int identity32;
        /// <summary>
        /// 默认自增标识
        /// </summary>
        internal static int Identity32
        {
            get { return Interlocked.Increment(ref identity32); }
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        internal static Action<int> ClearCaches;
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            AutoCSer.Metadata.MemberIndexGroup.ClearCache();
            AutoCSer.Metadata.AttributeMethod.ClearCache();
            AutoCSer.Metadata.TypeAttribute.ClearCache();
            if (ClearCaches != null) ClearCaches(count);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearUnmanagedCache(int count)
        {
            UnmanagedPool.ClearCache(count);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void ClearCache(int count = 0)
        {
            clearCache(count);
            GC.Collect();
            clearUnmanagedCache(count);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void ClearCacheNoGC(int count = 0)
        {
            clearCache(count);
            clearUnmanagedCache(count);
        }

        /// <summary>
        /// 空委托
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void emptyAction() { }
        /// <summary>
        /// 空委托
        /// </summary>
        internal static readonly Action EmptyAction = emptyAction;
    }
}
