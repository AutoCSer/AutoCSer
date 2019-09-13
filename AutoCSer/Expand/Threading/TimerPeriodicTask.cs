using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 周期定时任务
    /// </summary>
    public sealed class TimerPeriodicTask
    {
        /// <summary>
        /// 定时时钟周期
        /// </summary>
        private readonly long intervalTicks;
        /// <summary>
        /// 执行定时任务
        /// </summary>
        private readonly Action runHandle;
        /// <summary>
        /// 定时任务
        /// </summary>
        private readonly Action run;
        /// <summary>
        /// 定时器周期数据
        /// </summary>
        private readonly TimerPeriodic timerPeriodic;
        /// <summary>
        /// 定时任务线程模式
        /// </summary>
        private readonly TimerTaskThreadType threadType;
        /// <summary>
        /// 是否已经停止
        /// </summary>
        private bool isStop;
        /// <summary>
        /// 下一个执行时间
        /// </summary>
        private DateTime runTime;
        /// <summary>
        /// 周期定时任务
        /// </summary>
        /// <param name="run">定时任务</param>
        /// <param name="timerPeriodic">定时器周期数据</param>
        /// <param name="threadType">定时任务线程模式</param>
        public TimerPeriodicTask(Action run, TimerPeriodic timerPeriodic, TimerTaskThreadType threadType = TimerTaskThreadType.ThreadPool)
        {
            if (run == null) throw new ArgumentNullException();
            this.run = run;
            this.threadType = threadType;
            TimerTask.Default.Add(runHandle = Run, runTime = timerPeriodic.GetStartRunTime(intervalTicks = timerPeriodic.GetIntervalTicks()), threadType);
        }
        /// <summary>
        /// 停止执行任务
        /// </summary>
        private void Stop()
        {
            isStop = true;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        private void Run()
        {
            try
            {
                if (!isStop) run();
            }
            finally
            {
                if (timerPeriodic.PeriodicUnit != TimerPeriodicUnit.Once && !isStop)
                {
                    TimerTask.Default.Add(runHandle, runTime = timerPeriodic.GetNextRunTime(runTime, intervalTicks), threadType);
                }
            }
        }
    }
}
