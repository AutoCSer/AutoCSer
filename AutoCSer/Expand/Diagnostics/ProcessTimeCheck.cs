using System;
using System.Diagnostics;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程 CPU 事件检测
    /// </summary>
    public class ProcessTimeCheck : AutoCSer.Threading.SecondTimerNode, IDisposable
    {
        /// <summary>
        /// 进程信息
        /// </summary>
        private readonly Process process;
        /// <summary>
        /// 检测间隔时间戳
        /// </summary>
        private readonly long checkTimestamp;
        /// <summary>
        /// 进程占用 CPU 时间
        /// </summary>
        private long ticks;
        /// <summary>
        /// 计时器时间戳
        /// </summary>
        private long timestamp;
        /// <summary>
        /// 连续检测成功次数
        /// </summary>
        public int CheckCount { get; private set; }
        /// <summary>
        /// 进程计时
        /// </summary>
        /// <param name="process">进程信息，默认为当前进程</param>
        /// <param name="checkMilliseconds">检测间隔毫秒数，默认为 900</param>
        protected ProcessTimeCheck(Process process = null, int checkMilliseconds = 900)
        {
            this.process = process ?? Process.GetCurrentProcess();
            checkTimestamp = Date.GetTimestampByMilliseconds(checkMilliseconds);
            ticks = this.process.TotalProcessorTime.Ticks;
            timestamp = Stopwatch.GetTimestamp();
            AutoCSer.Threading.SecondTimer.TaskArray.NodeLink.PushNotNull(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (AutoCSer.Threading.SecondTimer.TaskArray.NodeLink.PopNotNull(this)) process.Dispose();
        }
        /// <summary>
        /// 检测定时处理
        /// </summary>
        protected internal override void OnTimer()
        {
            long nowTimestamp = Stopwatch.GetTimestamp();
            if (nowTimestamp - timestamp >= checkTimestamp)
            {
                long lastTicks = ticks, lastTimestamp = timestamp;
                ticks = process.TotalProcessorTime.Ticks;
                timestamp = Stopwatch.GetTimestamp();
                if (check(Date.GetTicksByTimestamp(timestamp - lastTimestamp), ticks - lastTicks)) ++CheckCount;
                else CheckCount = 0;
            }
        }
        /// <summary>
        /// 默认检测委托，默认检测状态为超过 7/8
        /// </summary>
        /// <param name="ticks">检测间隔时钟周期</param>
        /// <param name="processTicks">进程占用 CPU 时钟周期</param>
        /// <returns>是否超时</returns>
        protected virtual bool check(long ticks, long processTicks)
        {
            return (ticks - processTicks) <= (ticks >> 3);
        }
    }
}
