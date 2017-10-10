using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 计数等待
    /// </summary>
    internal sealed class AutoWaitCount
    {
        /// <summary>
        /// 当前计数
        /// </summary>
        private int count;
        /// <summary>
        /// 等待事件
        /// </summary>
        private AutoWaitHandle waitHandle;
        /// <summary>
        /// 计数等待
        /// </summary>
        /// <param name="count">当前计数</param>
        public AutoWaitCount(int count)
        {
            waitHandle.Set(0);
            this.count = count + 1;
        }
        /// <summary>
        /// 减少计数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Free()
        {
            if (System.Threading.Interlocked.Decrement(ref count) == 0) waitHandle.Set();
        }
        /// <summary>
        /// 等待计数完成并重置计数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WaitSet(int count)
        {
            if (System.Threading.Interlocked.Decrement(ref this.count) != 0) waitHandle.Wait();
            this.count = count + 1;
        }
    }
}
