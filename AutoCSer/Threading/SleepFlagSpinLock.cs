using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 休眠标志自旋锁
    /// </summary>
    public struct SleepFlagSpinLock
    {
        /// <summary>
        /// 锁数据
        /// </summary>
        private int lockValue;
        /// <summary>
        /// 休眠标志
        /// </summary>
        public volatile int SleepFlag;
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryEnter()
        {
            return System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0;
        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频一般冲突场景
        /// </summary>
        public void Enter()
        {
            do
            {
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                if (SleepFlag == 0)
                {
                    ThreadYield.YieldOnly();
                    if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                    if (SleepFlag == 0)
                    {
                        ThreadYield.YieldOnly();
                        if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                        if (SleepFlag == 0)
                        {
                            ThreadYield.YieldOnly();
                            if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                            if (SleepFlag == 0)
                            {
                                ThreadYield.YieldOnly();
                                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(0);
            }
            while (true);
        }
        /// <summary>
        /// 申请锁并设置休眠标志，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频一般冲突场景
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void EnterSleepFlag()
        {
            Enter();
            SleepFlag = 1;
        }
        /// <summary>
        /// 申请锁，每间隔 4 次调用 1 次 Thread.Sleep(0)，用于高频高冲突场景（不检测休眠标识，和 SpinLock.Enter4 效果一样）
        /// </summary>
        public void EnterNotCheckSleepFlag()
        {
            do
            {
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                ThreadYield.YieldOnly();
                if (System.Threading.Interlocked.CompareExchange(ref lockValue, 1, 0) == 0) return;
                System.Threading.Thread.Sleep(0);
            }
            while (true);
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Exit()
        {
            System.Threading.Interlocked.Exchange(ref lockValue, 0);
        }
        /// <summary>
        /// 重置休眠标志并释放锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ExitSleepFlag()
        {
            SleepFlag = 0;
            System.Threading.Interlocked.Exchange(ref lockValue, 0);
        }
    }
}
