using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维定时任务数组
    /// </summary>
    internal sealed class SecondTimerTaskArray
    {
        /// <summary>
        /// 第一维定时任务数组
        /// </summary>
        private readonly SecondTimerTaskLink[] linkArray;
        /// <summary>
        /// 第二维定时任务数组
        /// </summary>
        private readonly SecondTimerTaskLink[] nextLinkArray;
        /// <summary>
        /// 任务数组容器大小
        /// </summary>
        private readonly int linkArrayCapacity;
        /// <summary>
        /// 容器二进制位长度，最小值为 8，最大值为 12
        /// </summary>
        private readonly int linkArrayBitSize;
        /// <summary>
        /// 超出二维任务链表
        /// </summary>
        private SecondTimerTaskLink timerLink;
        /// <summary>
        /// 每秒尝试一次的定时任务链表，不能保证每秒触发一次
        /// </summary>
        internal SecondTimerNode.YieldLink NodeLink;
        /// <summary>
        /// 第一维定时任务数组基础秒数计时
        /// </summary>
        private long linkArrayBaseSeconds;
        /// <summary>
        /// 第一维定时任务数组当前位置
        /// </summary>
        private int linkArrayIndex;
        /// <summary>
        /// 第二维定时任务数组当前位置
        /// </summary>
        private int nextLinkArrayIndex;
        /// <summary>
        /// 任务节点访问锁
        /// </summary>
        internal AutoCSer.Threading.SleepFlagSpinLock TimerLinkLock;
        /// <summary>
        /// 二维定时任务数组
        /// </summary>
        /// <param name="linkArrayBitSize">容器二进制位长度，最小值为 8，最大值为 12</param>
        internal SecondTimerTaskArray(byte linkArrayBitSize)
        {
            this.linkArrayBitSize = Math.Min(Math.Max((int)linkArrayBitSize, 8), 12);
            linkArrayCapacity = 1 << linkArrayBitSize;
            linkArray = new SecondTimerTaskLink[linkArrayCapacity];
            nextLinkArray = new SecondTimerTaskLink[linkArrayCapacity];
        }
        /// <summary>
        /// 添加定时任务节点
        /// </summary>
        /// <param name="node"></param>
        internal void Append(SecondTimerTaskNode node)
        {
            long timeoutSeconds = node.TimeoutSeconds;
            TimerLinkLock.Enter();
            long index = timeoutSeconds - linkArrayBaseSeconds;
            if (index < linkArrayCapacity)
            {
                if (index >= linkArrayIndex)
                {
                    linkArray[(int)index].Append(node);
                    TimerLinkLock.Exit();
                    return;
                }
            }
            else
            {
                index = ((index - linkArrayCapacity) >> linkArrayBitSize) + nextLinkArrayIndex;
                if (index < linkArrayCapacity) nextLinkArray[(int)index].Append(node);
                else timerLink.Append(node);
                TimerLinkLock.Exit();
                return;
            }
            TimerLinkLock.Exit();
            node.AppendCall();
        }
        /// <summary>
        /// 添加定时任务（下一次定时器触发时执行）
        /// </summary>
        /// <param name="task">委托任务</param>
        /// <param name="threadMode">执行任务的线程模式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AppendNext(Action task, SecondTimerThreadMode threadMode = SecondTimerThreadMode.TinyBackgroundThreadPool)
        {
            Append(new SecondTimerActionTask(this, task, 1, threadMode));
        }
        /// <summary>
        /// 添加定时委托任务
        /// </summary>
        /// <param name="task">委托任务</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        /// <param name="threadMode">执行任务的线程模式</param>
        /// <param name="KeepMode">定时任务继续模式</param>
        /// <param name="keepSeconds">继续执行间隔秒数，0 表示不继续执行</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#if DOTNET2
        public void Append(Action task, int timeoutSeconds, SecondTimerThreadMode threadMode = SecondTimerThreadMode.TinyBackgroundThreadPool, SecondTimerKeepMode KeepMode = SecondTimerKeepMode.Once, int keepSeconds = 0)
#else
        public void Append(Action task, int timeoutSeconds, SecondTimerThreadMode threadMode = SecondTimerThreadMode.TaskRun, SecondTimerKeepMode KeepMode = SecondTimerKeepMode.Once, int keepSeconds = 0)
#endif
        {
            Append(new SecondTimerActionTask(this, task, timeoutSeconds, threadMode, KeepMode, keepSeconds));
        }
        /// <summary>
        /// 添加定时委托任务
        /// </summary>
        /// <param name="task">委托任务</param>
        /// <param name="timeout">第一次执行任务时间</param>
        /// <param name="threadMode">执行任务的线程模式</param>
        /// <param name="KeepMode">定时任务继续模式</param>
        /// <param name="keepSeconds">继续执行间隔秒数，0 表示不继续执行</param>
#if DOTNET2
        public void Append(Action task, DateTime timeout, SecondTimerThreadMode threadMode = SecondTimerThreadMode.TinyBackgroundThreadPool, SecondTimerKeepMode KeepMode = SecondTimerKeepMode.Once, int keepSeconds = 0)
#else
        public void Append(Action task, DateTime timeout, SecondTimerThreadMode threadMode = SecondTimerThreadMode.TaskRun, SecondTimerKeepMode KeepMode = SecondTimerKeepMode.Once, int keepSeconds = 0)
#endif
        {
            long ticks = timeout.Ticks - (timeout.Kind == DateTimeKind.Utc ? SecondTimer.UtcNow : SecondTimer.Now).Ticks;
            long timeoutSeconds = ticks > 0 ? (ticks + (TimeSpan.TicksPerSecond - 1)) / TimeSpan.TicksPerSecond : 0;
            Append(new SecondTimerActionTask(this, task, (int)Math.Min(timeoutSeconds, int.MaxValue), threadMode, KeepMode, keepSeconds));
        }
        /// <summary>
        /// 添加每分钟执行一次的委托任务，比如清理缓存
        /// </summary>
        /// <param name="task"></param>
        /// <param name="threadMode"></param>
        /// <param name="KeepMode"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AppendMinute(Action task, SecondTimerThreadMode threadMode = SecondTimerThreadMode.Synchronous, SecondTimerKeepMode KeepMode = SecondTimerKeepMode.Before)
        {
            Append(new SecondTimerActionTask(this, task, 60, threadMode, KeepMode, 60));
        }
        /// <summary>
        /// 尝试执行定时任务
        /// </summary>
        internal void OnTimer()
        {
            SecondTimerNode node = NodeLink.End;
            if (node != null) SecondTimerNode.LinkOnTimer(node);

            SecondTimerTaskNode head;
            do
            {
                TimerLinkLock.Enter();
                long seconds = linkArrayBaseSeconds + linkArrayIndex;
                if (seconds < SecondTimer.CurrentSeconds)
                {
                    head = linkArray[linkArrayIndex++].GetClear();
                    if (linkArrayIndex == linkArrayCapacity)
                    {
                        linkArrayIndex = 0;
                        linkArrayBaseSeconds += linkArrayCapacity;

                        SecondTimerTaskNode nextHead = nextLinkArray[nextLinkArrayIndex++].GetClear();
                        while (nextHead != null) nextHead = linkArray[(int)(nextHead.TimeoutSeconds - linkArrayBaseSeconds)].AppendOtherHead(nextHead);

                        if (nextLinkArrayIndex == linkArrayCapacity)
                        {
                            nextLinkArrayIndex = 0;

                            long baseSeconds = linkArrayBaseSeconds + linkArrayCapacity, maxSeconds = baseSeconds + (linkArrayCapacity << linkArrayBitSize);
                            nextHead = timerLink.GetClear();
                            while (nextHead != null)
                            {
                                if (nextHead.TimeoutSeconds < maxSeconds)
                                {
                                    nextHead = nextLinkArray[(int)(nextHead.TimeoutSeconds - baseSeconds) >> linkArrayBitSize].AppendOtherHead(nextHead);
                                }
                                else nextHead = timerLink.AppendOtherHead(nextHead);
                            }
                        }
                    }
                    TimerLinkLock.Exit();

                    while (head != null)
                    {
                        try
                        {
                            do
                            {
                                head.Call(ref head);
                            }
                            while (head != null);
                        }
                        catch (Exception exception)
                        {
                            AutoCSer.LogHelper.Exception(exception, null, LogLevel.AutoCSer | LogLevel.Exception);
                        }
                    }
                }
                else
                {
                    TimerLinkLock.Exit();
                    return;
                }
            }
            while (true);
        }
    }
}
