using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 当前时间
    /// </summary>
    public sealed class NowTime
    {
        /// <summary>
        /// 下一次最小时间
        /// </summary>
        private DateTime minTime;
        /// <summary>
        /// 初始化最大时间
        /// </summary>
        private DateTime maxTime = AutoCSer.Date.NowTime.Now;
        /// <summary>
        /// 间隔时钟周期
        /// </summary>
        private long ticks;
        /// <summary>
        /// 时间访问锁
        /// </summary>
        private int timeLock;
        /// <summary>
        /// 获取下一个时间
        /// </summary>
        public DateTime Next
        {
            get
            {
                DateTime now = Date.NowTime.Now;
                while (System.Threading.Interlocked.CompareExchange(ref timeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.SqlNowTimeSet);
                if (now < minTime) now = minTime;
                minTime = now.AddTicks(ticks);
                System.Threading.Interlocked.Exchange(ref timeLock, 0);
                return now;
            }
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <param name="milliseconds">间隔毫秒数</param>
        public NowTime(int milliseconds = MsSql.Sql2000.DefaultNowTimeMilliseconds)
        {
            ticks = TimeSpan.TicksPerMillisecond * Math.Max(milliseconds, MsSql.Sql2000.DefaultNowTimeMilliseconds);
        }
        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="time"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(DateTime time)
        {
            while (System.Threading.Interlocked.CompareExchange(ref timeLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.SqlNowTimeSet);
            minTime = time.AddTicks(ticks);
            System.Threading.Interlocked.Exchange(ref timeLock, 0);
        }
        /// <summary>
        /// 在初始化循环中设置最大时间
        /// </summary>
        /// <param name="time"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetMaxTime(DateTime time)
        {
            if (time > maxTime) maxTime = time;
        }
        /// <summary>
        /// 在初始化循环中设置最大时间
        /// </summary>
        /// <param name="time"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetMaxTime(DateTime? time)
        {
            if (time != null) SetMaxTime((DateTime)time);
        }
        /// <summary>
        /// 在初始化循环结束后确认最大时间
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetMaxTime()
        {
            Set(maxTime);
        }
    }
}
