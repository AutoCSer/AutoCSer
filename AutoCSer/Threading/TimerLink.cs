using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时链表
    /// </summary>
    /// <typeparam name="timerType"></typeparam>
    internal abstract class TimerLink<timerType> : AutoCSer.Threading.DoubleLink<timerType>
        where timerType : TimerLink<timerType>
    {
        /// <summary>
        /// 超时秒数
        /// </summary>
        protected int seconds;
        /// <summary>
        /// 引用次数
        /// </summary>
        protected int count;
        /// <summary>
        /// 是否已经触发定时任务
        /// </summary>
        protected int isTimer;
        /// <summary>
        /// 当前秒计数
        /// </summary>
        protected long currentSeconds;
        /// <summary>
        /// 定时器双向链表节点
        /// </summary>
        /// <param name="seconds">超时秒数</param>
        protected TimerLink(int seconds)
        {
            this.seconds = seconds;
            count = 1;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value"></param>
        protected void set(timerType value)
        {
            if (TimeoutEnd != null)
            {
                DoubleLinkPrevious = TimeoutEnd;
                TimeoutEnd.DoubleLinkNext = value;
            }
            TimeoutEnd = value;
        }
        /// <summary>
        /// 释放定时器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void free()
        {
            if (--count == 0)
            {
                if (DoubleLinkNext == null) TimeoutEnd = DoubleLinkPrevious;
                else DoubleLinkNext.DoubleLinkPrevious = DoubleLinkPrevious;
                if (DoubleLinkPrevious != null) DoubleLinkPrevious.DoubleLinkNext = DoubleLinkNext;
            }
        }

        /// <summary>
        /// 定时链表
        /// </summary>
        internal static timerType TimeoutEnd;
        /// <summary>
        /// 定时链表集合访问锁
        /// </summary>
        protected static readonly object timeoutLock = new object();
        /// <summary>
        /// 释放套接字超时
        /// </summary>
        /// <param name="timeout"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Free(ref timerType timeout)
        {
            if (timeout != null)
            {
                FreeNotNull(timeout);
                timeout = null;
            }
        }
        /// <summary>
        /// 释放套接字超时
        /// </summary>
        /// <param name="timeout"></param>
        internal static void FreeNotNull(timerType timeout)
        {
            Monitor.Enter(timeoutLock);
            timeout.free();
            Monitor.Exit(timeoutLock);
        }
    }
}
