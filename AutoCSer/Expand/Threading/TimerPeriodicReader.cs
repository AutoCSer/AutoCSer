using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时器周期时间读取器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct TimerPeriodicReader
    {
        /// <summary>
        /// 定时器周期数据
        /// </summary>
        private readonly TimerPeriodic timerPeriodic;
        /// <summary>
        /// 间隔时钟周期
        /// </summary>
        private readonly long intervalTicks;
        /// <summary>
        /// 当前读取时间 
        /// </summary>
        private DateTime time;
        /// <summary>
        /// 获取下一个时间
        /// </summary>
        public DateTime NextTime
        {
            get
            {
                if (time != default(DateTime)) return GetNextTime();
                return time = timerPeriodic.GetStartRunTime(intervalTicks);
            }
        }
        /// <summary>
        /// 定时器周期时间读取器
        /// </summary>
        /// <param name="timerPeriodic">定时器周期数据</param>
        public TimerPeriodicReader(ref TimerPeriodic timerPeriodic)
        {
            this.timerPeriodic = timerPeriodic;
            intervalTicks = timerPeriodic.GetIntervalTicks();
            time = default(DateTime);
        }
        /// <summary>
        /// 获取下一个时间
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal DateTime GetNextTime()
        {
            time = timerPeriodic.GetStartRunTime(intervalTicks);
            return time;
        }
    }
}
