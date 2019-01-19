using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时任务线程模式
    /// </summary>
    public enum TimerTaskThreadType : byte
    {
        /// <summary>
        /// 线程池处理
        /// </summary>
        ThreadPool,
        /// <summary>
        /// 队列处理，阻塞定时任务，适应于快速执行任务
        /// </summary>
        Queue,
        /// <summary>
        /// 独占定时线程处理，不阻塞定时任务
        /// </summary>
        OnTimer,
    }
}
