using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 执行任务的线程模式
    /// </summary>
    public enum SecondTimerThreadMode : byte
    {
        /// <summary>
        /// 阻塞定时线程同步执行，适用于无阻塞快速结束任务避免线程调度
        /// </summary>
        Synchronous,
        /// <summary>
        /// 线程池 AutoCSer.Threading.ThreadPool.TinyBackground（不是 System.Threading.ThreadPool）
        /// </summary>
        TinyBackgroundThreadPool,
#if !DOTNET2
        /// <summary>
        /// 调用 Task.Run 执行
        /// </summary>
        TaskRun,
#endif
    }
}
