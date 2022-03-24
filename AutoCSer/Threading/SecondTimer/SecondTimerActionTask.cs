using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维秒级定时委托任务节点
    /// </summary>
    internal sealed class SecondTimerActionTask : SecondTimerTaskNode
    {
        /// <summary>
        /// 委托任务
        /// </summary>
        private Action task;
        /// <summary>
        /// 二维秒级定时委托任务节点
        /// </summary>
        private SecondTimerActionTask() : base() { task = AutoCSer.Common.EmptyAction; }
        /// <summary>
        /// 二维秒级定时委托任务节点
        /// </summary>
        /// <param name="taskArray">二维定时任务数组</param>
        /// <param name="task">委托任务</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        /// <param name="threadMode">执行任务的线程模式</param>
        /// <param name="KeepMode">定时任务继续模式</param>
        /// <param name="keepSeconds">继续执行间隔秒数，0 表示不继续执行</param>
#if DOTNET2 || DOTNET4 || UNITY3D
        internal SecondTimerActionTask(SecondTimerTaskArray taskArray, Action task, int timeoutSeconds, SecondTimerThreadMode threadMode = SecondTimerThreadMode.TinyBackgroundThreadPool, SecondTimerKeepMode KeepMode = SecondTimerKeepMode.Once, int keepSeconds = 0)
#else
        internal SecondTimerActionTask(SecondTimerTaskArray taskArray, Action task, int timeoutSeconds, SecondTimerThreadMode threadMode = SecondTimerThreadMode.TaskRun, SecondTimerKeepMode KeepMode = SecondTimerKeepMode.Once, int keepSeconds = 0)
#endif
            : base(taskArray, timeoutSeconds, threadMode, KeepMode, keepSeconds)
        {
            this.task = task;
        }
        /// <summary>
        /// 取消执行
        /// </summary>
        protected override void cancel() 
        {
            task = Common.EmptyAction;
        }
        /// <summary>
        /// 触发定时操作
        /// </summary>
        protected internal override void OnTimer()
        {
            task();
        }

        /// <summary>
        /// 空节点
        /// </summary>
        internal static new readonly SecondTimerActionTask Null = new SecondTimerActionTask();
    }
}
