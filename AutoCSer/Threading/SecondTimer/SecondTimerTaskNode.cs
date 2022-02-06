using System;
using System.Runtime.CompilerServices;
#if !DOTNET2
using System.Threading.Tasks;
#endif

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维秒级定时任务节点
    /// </summary>
    public class SecondTimerTaskNode : DoubleLink<SecondTimerTaskNode>
    {
        /// <summary>
        /// 二维定时任务数组
        /// </summary>
        private readonly SecondTimerTaskArray taskArray;
        /// <summary>
        /// 继续执行间隔秒数，0 表示不继续执行
        /// </summary>
        private readonly int keepSeconds;
        /// <summary>
        /// 执行任务的线程模式
        /// </summary>
        private readonly SecondTimerThreadMode threadMode;
        /// <summary>
        /// 定时任务继续模式
        /// </summary>
        internal SecondTimerKeepMode KeepMode;
        /// <summary>
        /// 超时
        /// </summary>
        internal long TimeoutSeconds;
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        protected SecondTimerTaskNode() { KeepMode = SecondTimerKeepMode.Canceled; }
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        /// <param name="taskArray">二维定时任务数组</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        /// <param name="threadMode">执行任务的线程模式</param>
        /// <param name="KeepMode">定时任务继续模式</param>
        /// <param name="keepSeconds">继续执行间隔秒数，0 表示不继续执行</param>
        internal SecondTimerTaskNode(SecondTimerTaskArray taskArray, int timeoutSeconds, SecondTimerThreadMode threadMode, SecondTimerKeepMode KeepMode, int keepSeconds)
        {
            switch (KeepMode)
            {
                case SecondTimerKeepMode.After:
                case SecondTimerKeepMode.Before:
                    if (keepSeconds <= 0) KeepMode = SecondTimerKeepMode.Once;
                    else
                    {
                        this.keepSeconds = keepSeconds;
                        if (timeoutSeconds < 0) timeoutSeconds = 0;
                    }
                    break;
            }

            this.taskArray = taskArray;
            TimeoutSeconds = SecondTimer.CurrentSeconds + timeoutSeconds;
            this.threadMode = threadMode;
            this.KeepMode = KeepMode;
        }
        /// <summary>
        /// 任务添加到二维定时任务数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AppendTaskArray()
        {
            taskArray.Append(this);
        }
        /// <summary>
        /// 取消执行
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Cancel()
        {
            KeepMode = SecondTimerKeepMode.Canceled;
            cancel();
        }
        /// <summary>
        /// 取消执行
        /// </summary>
        protected virtual void cancel() { }
        /// <summary>
        /// 添加任务直接触发定时操作
        /// </summary>
        internal void AppendCall()
        {
            switch(KeepMode)
            {
                case SecondTimerKeepMode.Once: once(); return;
                case SecondTimerKeepMode.After:
                    switch(threadMode)
                    {
                        case SecondTimerThreadMode.Synchronous: After(); return;
#if !DOTNET2
                        case SecondTimerThreadMode.TaskRun: Task.Run((Action)After); return;
#endif
                        case SecondTimerThreadMode.TinyBackgroundThreadPool: ThreadPool.TinyBackground.FastStart(this, ThreadTaskType.SecondTimerTaskNodeAfter); return;
                    }
                    return;
                case SecondTimerKeepMode.Before:
                    TimeoutSeconds += keepSeconds;
                    taskArray.Append(this);
                    once();
                    return;
            }
        }
        /// <summary>
        /// 触发定时操作
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void once()
        {
            switch (threadMode)
            {
                case SecondTimerThreadMode.Synchronous: OnTimer(); return;
#if !DOTNET2
                case SecondTimerThreadMode.TaskRun: Task.Run((Action)OnTimer); return;
#endif
                case SecondTimerThreadMode.TinyBackgroundThreadPool: ThreadPool.TinyBackground.FastStart(this, ThreadTaskType.SecondTimerTaskNodeOnTimer); return;
            }
        }
        /// <summary>
        /// 执行之后添加新的定时任务
        /// </summary>
        internal void After()
        {
            try
            {
                OnTimer();
            }
            finally
            {
                if (KeepMode == SecondTimerKeepMode.After)
                {
                    TimeoutSeconds = SecondTimer.CurrentSeconds + keepSeconds;
                    taskArray.Append(this);
                }
            }
        }
        /// <summary>
        /// 触发定时任务并返回下一个节点
        /// </summary>
        /// <returns></returns>
        internal void Call(ref SecondTimerTaskNode next)
        {
            next = DoubleLinkNext;
            switch (KeepMode)
            {
                case SecondTimerKeepMode.Once: once(); return;
                case SecondTimerKeepMode.After:
                    ResetDoubleLink();
                    switch (threadMode)
                    {
                        case SecondTimerThreadMode.Synchronous: After(); return;
#if !DOTNET2
                        case SecondTimerThreadMode.TaskRun: Task.Run((Action)After); return;
#endif
                        case SecondTimerThreadMode.TinyBackgroundThreadPool: ThreadPool.TinyBackground.FastStart(this, ThreadTaskType.SecondTimerTaskNodeAfter); return;
                    }
                    return;
                case SecondTimerKeepMode.Before:
                    ResetDoubleLink();
                    TimeoutSeconds += keepSeconds;
                    taskArray.Append(this);
                    once();
                    return;
            }
        }
        /// <summary>
        /// 触发定时操作
        /// </summary>
        protected internal virtual void OnTimer() { }

        /// <summary>
        /// 默认空节点
        /// </summary>
        internal static readonly SecondTimerTaskNode Null = new SecondTimerTaskNode();
    }
}
