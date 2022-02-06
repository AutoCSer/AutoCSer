using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// Interlocked.CompareExchange 自旋锁
    /// </summary>
    public struct SpinLock
    {
        /// <summary>
        /// 锁数据
        /// </summary>
        internal int Lock;
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryEnter()
        {
            return System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0;
        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频一般冲突场景
        /// </summary>
        public void EnterYield()
        {
            do
            {
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0) return;
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0) return;
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0) return;
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0) return;
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) == 0) return;
                System.Threading.Thread.Sleep(0);
            }
            while (true);
        }
        /// <summary>
        /// 申请锁，一直调用 Thread.Sleep(0)，用于低频场景
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void EnterSleep()
        {
            while (System.Threading.Interlocked.CompareExchange(ref Lock, 1, 0) != 0) System.Threading.Thread.Sleep(0);
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Exit()
        {
            System.Threading.Interlocked.Exchange(ref Lock, 0);
        }
    }
}
