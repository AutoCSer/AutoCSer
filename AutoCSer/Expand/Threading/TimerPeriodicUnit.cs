using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时器周期单位
    /// </summary>
    public enum TimerPeriodicUnit : byte
    {
        /// <summary>
        /// 仅一次
        /// </summary>
        Once,
        /// <summary>
        /// 每秒
        /// </summary>
        Second,
        /// <summary>
        /// 每分钟
        /// </summary>
        Minute,
        /// <summary>
        /// 每小时
        /// </summary>
        Hour,
        /// <summary>
        /// 每天
        /// </summary>
        Day,
        /// <summary>
        /// 每星期
        /// </summary>
        Week,
        /// <summary>
        /// 每月
        /// </summary>
        Month,
        /// <summary>
        /// 每年
        /// </summary>
        Year
    }
}
