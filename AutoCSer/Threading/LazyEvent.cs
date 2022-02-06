using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 延迟事件
    /// </summary>
    internal abstract class LazyEvent : SecondTimerTaskNode
    {
        /// <summary>
        /// 当前检测超时秒数
        /// </summary>
        protected long timeoutSeconds;
        /// <summary>
        /// 延迟秒数
        /// </summary>
        protected readonly int seconds;
        /// <summary>
        /// 保留字段
        /// </summary>
        protected int reserve;
        /// <summary>
        /// 延迟事件
        /// </summary>
        /// <param name="seconds">延迟秒数</param>
        internal LazyEvent(int seconds) : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, seconds, SecondTimerThreadMode.Synchronous, SecondTimerKeepMode.After, seconds)
        {
            this.seconds = seconds;
            setTimeout();
        }
        ///// <summary>
        ///// 释放资源
        ///// </summary>

        //public void Dispose()
        //{
        //    AutoCSer.Date.OnTime -= chekHandle;
        //}
        /// <summary>
        /// 重置超时时间
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setTimeout()
        {
            timeoutSeconds = AutoCSer.Threading.SecondTimer.CurrentSeconds + this.seconds;
        }
   }
}
