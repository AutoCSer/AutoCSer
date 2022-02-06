using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 常用公共定义
    /// </summary>
    public static class Pub
    {
#if !Serialize
        /// <summary>
        /// 未结束定时器线程数量
        /// </summary>
        internal static long TimerThread;
        /// <summary>
        /// 未结束定时器线程数量
        /// </summary>
        public static long TimerThreadCount { get { return TimerThread + AutoCSer.Threading.SecondTimer.RefreshTimeThreadCount; } }
#endif
    }
}
