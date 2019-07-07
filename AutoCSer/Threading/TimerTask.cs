using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时任务
    /// </summary>
    public sealed class TimerTask : IDisposable
    {
        /// <summary>
        /// 已排序任务
        /// </summary>
        private ArrayHeap<long, TimerTaskInfo> taskHeap = new ArrayHeap<long, TimerTaskInfo>(true);
        /// <summary>
        /// 任务访问锁
        /// </summary>
        private object taskLock = new object();
        /// <summary>
        /// 最近时间
        /// </summary>
        private long nearTime = long.MaxValue;
        /// <summary>
        /// 是否已经触发定时任务
        /// </summary>
        private int isTimer;
        /// <summary>
        /// 定时任务
        /// </summary>
        internal TimerTask() { }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (taskHeap != null)
            {
                taskHeap.Dispose();
                taskHeap = null;
            }
        }
        /// <summary>
        /// 添加新任务
        /// </summary>
        /// <param name="value">任务委托</param>
        /// <param name="type">调用类型</param>
        /// <param name="threadType">定时任务线程模式</param>
        /// <param name="runTime">执行时间</param>
        private void add(object value, Thread.CallType type, TimerTaskThreadType threadType, DateTime runTime)
        {
            long runTimeTicks = runTime.Ticks;
            TimerTaskInfo taskInfo = new TimerTaskInfo { Value = value, CallType = type, ThreadType = threadType };
            Monitor.Enter(taskLock);
            try
            {
                taskHeap.Push(runTimeTicks, ref taskInfo);
                if (runTimeTicks < nearTime) nearTime = runTimeTicks;
            }
            finally { Monitor.Exit(taskLock); }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="run">任务执行委托</param>
        /// <param name="runTime">执行时间</param>
        /// <param name="threadType">定时任务线程模式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(Action run, DateTime runTime, TimerTaskThreadType threadType = TimerTaskThreadType.ThreadPool)
        {
            if (run != null) add(run, Thread.CallType.Action, threadType, runTime);
        }
        /// <summary>
        /// 激活计时器
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly DateTime timer = Date.NowTime.Now;
        /// <summary>
        /// 触发定时任务
        /// </summary>
        /// <param name="now"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnTimer(DateTime now)
        {
            if (now.Ticks >= nearTime && System.Threading.Interlocked.CompareExchange(ref isTimer, 1, 0) == 0) onTimer();
        }
        /// <summary>
        /// 线程池任务
        /// </summary>
        private unsafe void onTimer()
        {
            TimerTaskInfo taskInfo = new TimerTaskInfo();
            Monitor.Enter(taskLock);
            try
            {
                do
                {
                    try
                    {
                        while (taskHeap.Count != 0)
                        {
                            int index = taskHeap.Heap.Int[1];
                            long ticks = taskHeap.Array[index].Key;
                            if (ticks <= Date.NowTime.Set().Ticks)
                            {
                                taskInfo = taskHeap.Array[index].Value;
                                taskHeap.RemoveTop();
                                switch (taskInfo.ThreadType)
                                {
                                    case TimerTaskThreadType.ThreadPool: taskInfo.Start(ThreadPool.TinyBackground); break;
                                    case TimerTaskThreadType.Queue: taskInfo.Call(); break;
                                    case TimerTaskThreadType.OnTimer: goto ONTIMER;
                                }
                            }
                            else
                            {
                                nearTime = ticks;
                                return;
                            }
                        }
                        nearTime = long.MaxValue;
                        return;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                    }
                }
                while (true);
            ONTIMER: ;
            }
            finally
            {
                Monitor.Exit(taskLock);
                System.Threading.Interlocked.Exchange(ref isTimer, 0);
            }
            try
            {
                taskInfo.Call();
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 默认定时任务（不保证任务触发的及时性）
        /// </summary>
        public static readonly TimerTask Default = new TimerTask();
    }
}
