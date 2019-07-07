using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时器周期数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct TimerPeriodic
    {
        /// <summary>
        /// 第一次执行时间
        /// </summary>
        public DateTime StartTime;
        /// <summary>
        /// 周期单位
        /// </summary>
        public TimerPeriodicUnit PeriodicUnit;
        /// <summary>
        /// 间隔周期
        /// </summary>
        public int Interval;

        /// <summary>
        /// 获取间隔时钟周期
        /// </summary>
        /// <returns></returns>
        public long GetIntervalTicks()
        {
            switch (PeriodicUnit)
            {
                case TimerPeriodicUnit.Second: return TimeSpan.TicksPerSecond * Interval;
                case TimerPeriodicUnit.Minute: return TimeSpan.TicksPerMinute * Interval;
                case TimerPeriodicUnit.Hour: return TimeSpan.TicksPerHour * Interval;
                case TimerPeriodicUnit.Day: return TimeSpan.TicksPerDay * Interval;
                case TimerPeriodicUnit.Week: return TimeSpan.TicksPerDay * 7 * Interval;
                case TimerPeriodicUnit.Month: return StartTime.Ticks - new DateTime(StartTime.Year, StartTime.Month, 1).Ticks;
                case TimerPeriodicUnit.Year: return StartTime.Ticks - new DateTime(StartTime.Year, 1, 1).Ticks;
                default: return 0;
            }
        }
        /// <summary>
        /// 获取第一次运行时间
        /// </summary>
        /// <param name="intervalTicks">间隔时钟周期</param>
        /// <returns></returns>
        public DateTime GetStartRunTime(long intervalTicks)
        {
            DateTime now = DateTime.Now;
            long ticks = now.Ticks - StartTime.Ticks;
            if (ticks <= 0) return StartTime;
            switch (PeriodicUnit)
            {
                case TimerPeriodicUnit.Once: return StartTime;
                case TimerPeriodicUnit.Second:
                case TimerPeriodicUnit.Minute:
                case TimerPeriodicUnit.Hour:
                case TimerPeriodicUnit.Day:
                case TimerPeriodicUnit.Week:
                    long mod = ticks % intervalTicks;
                    if (mod == 0) return StartTime.AddTicks(ticks);
                    return StartTime.AddTicks(ticks - mod + intervalTicks);
                case TimerPeriodicUnit.Month:
                    int monthCount = (now.Year - StartTime.Year) * 12 + (now.Month - StartTime.Month), monthMod = (int)(monthCount % Interval);
                    if (monthMod != 0) monthCount += (int)(Interval - monthMod);
                    DateTime monthTime = new DateTime(StartTime.Year, StartTime.Month, 1).AddMonths(monthCount).AddTicks(intervalTicks);
                    if (monthTime.Day != StartTime.Day) monthTime = new DateTime(monthTime.Year, monthTime.Month, 1);
                    return monthTime >= now ? monthTime : getNextMonth(monthTime, intervalTicks);
                case TimerPeriodicUnit.Year:
                    int yearCount = now.Year - StartTime.Year, yearMod = (int)(yearCount % Interval);
                    if (yearMod != 0) yearCount += (int)(Interval - yearMod);
                    DateTime yearTime = getYear(new DateTime(StartTime.Year, 1, 1).AddYears(yearCount).AddTicks(intervalTicks));
                    return yearTime >= now ? yearTime : getNextYear(yearTime, intervalTicks);
            }
            return DateTime.MaxValue;
        }
        /// <summary>
        /// 获取下一个月
        /// </summary>
        /// <param name="monthTime"></param>
        /// <param name="intervalTicks">间隔时钟周期</param>
        /// <returns></returns>
        private DateTime getNextMonth(DateTime monthTime, long intervalTicks)
        {
            monthTime = new DateTime(monthTime.Year, monthTime.Month, 1)
                .AddMonths((int)(StartTime.Day == monthTime.Day ? Interval : (Interval - 1)))
                .AddTicks(intervalTicks);
            return StartTime.Day == monthTime.Day ? monthTime : new DateTime(monthTime.Year, monthTime.Month, 1);
        }
        /// <summary>
        /// 获取下一个年
        /// </summary>
        /// <param name="yearTime"></param>
        /// <param name="intervalTicks">间隔时钟周期</param>
        /// <returns></returns>
        private DateTime getNextYear(DateTime yearTime, long intervalTicks)
        {
            return getYear(new DateTime(yearTime.Year, 1, 1).AddYears((int)Interval).AddTicks(intervalTicks));
        }
        /// <summary>
        /// 年份计算误差调整
        /// </summary>
        /// <param name="yearTime"></param>
        /// <returns></returns>
        private DateTime getYear(DateTime yearTime)
        {
            int days = yearTime.Day - StartTime.Day;
            switch (days)
            {
                case -1: return yearTime.AddDays(1);
                case 0: return yearTime;
                case 1: return yearTime.AddDays(-1);
                case 1 - 29: return new DateTime(yearTime.Year, 3, 1);
                default: return yearTime.AddDays(days < 0 ? -1 : 1);
            }
        }
        /// <summary>
        /// 获取下一个次运行时间
        /// </summary>
        /// <param name="runTime"></param>
        /// <param name="intervalTicks"></param>
        /// <returns></returns>
        public DateTime GetNextRunTime(DateTime runTime, long intervalTicks)
        {
            switch (PeriodicUnit)
            {
                case TimerPeriodicUnit.Second:
                case TimerPeriodicUnit.Minute:
                case TimerPeriodicUnit.Hour:
                case TimerPeriodicUnit.Day:
                case TimerPeriodicUnit.Week:
                    return runTime.AddTicks(intervalTicks);
                case TimerPeriodicUnit.Month:
                    return getNextMonth(runTime, intervalTicks);
                case TimerPeriodicUnit.Year:
                    return getNextYear(runTime, intervalTicks);
            }
            return DateTime.MaxValue;
        }
    }
}
