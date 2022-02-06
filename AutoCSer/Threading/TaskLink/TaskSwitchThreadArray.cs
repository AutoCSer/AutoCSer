using System;
using System.Diagnostics;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 超时切换任务线程集合
    /// </summary>
    public abstract class TaskSwitchThreadArrayBase
    {
        /// <summary>
        /// 线程切换超时时钟周期
        /// </summary>
        protected readonly long switchTimestamp;
        /// <summary>
        /// 等待任务时钟周期(不切换线程的最大值)
        /// </summary>
        internal readonly long NoSwitchTimestamp;
        /// <summary>
        /// 当前任务处理索引
        /// </summary>
        protected int threadIndex;
        /// <summary>
        /// 是否已经创建所有任务
        /// </summary>
        protected bool isAllThread;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        internal bool IsDisposed;
        /// <summary>
        /// 超时切换任务线程集合
        /// </summary>
        /// <param name="config"></param>
        protected TaskSwitchThreadArrayBase(TaskSwitchThreadConfig config)
        {
            switchTimestamp = Date.GetTimestampByMilliseconds(config.GetSwitchThreadMilliseconds);
            NoSwitchTimestamp = long.MaxValue - switchTimestamp;
        }

        /// <summary>
        /// 默认超时切换任务线程配置
        /// </summary>
        internal static readonly TaskSwitchThreadConfig DefaultConfig = (TaskSwitchThreadConfig)AutoCSer.Configuration.Common.Get(typeof(TaskSwitchThreadConfig)) ?? new TaskSwitchThreadConfig { SwitchThreadMilliseconds = 1, ThreadCount = AutoCSer.Common.ProcessorCount };
    }
    /// <summary>
    /// 超时切换任务线程集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="NT"></typeparam>
    public abstract class TaskSwitchThreadArray<T, NT> : TaskSwitchThreadArrayBase, IDisposable
        where T : TaskSwitchThread<NT>
        where NT : class
    {
        /// <summary>
        /// 创建线程访问锁
        /// </summary>
        private readonly object createLock;
        /// <summary>
        /// 线程切换检测定时器
        /// </summary>
        private readonly Timer timer;
        /// <summary>
        /// 当前任务线程集合
        /// </summary>
        private readonly T[] threads;
        /// <summary>
        /// 当前任务线程
        /// </summary>
        internal T CurrentThread;
        /// <summary>
        /// 超时切换任务线程集合
        /// </summary>
        /// <param name="config"></param>
        internal TaskSwitchThreadArray(TaskSwitchThreadConfig config) : base(config)
        {
            int threadCount = config.GetThreadCount;
            CurrentThread = CreateThread();
            if (threadCount != 1)
            {
                threads = new T[threadCount];
                threads[0] = CurrentThread;
                createLock = new object();

                int milliseconds = config.SwitchThreadMilliseconds;
                timer = new System.Threading.Timer(check, null, milliseconds, milliseconds);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                if (threads == null) CurrentThread.WaitHandle.Set();
                else
                {
                    if (timer != null) timer.Dispose();
                    Monitor.Enter(createLock);
                    try
                    {
                        foreach (T thread in threads)
                        {
                            if (thread != null) thread.WaitHandle.Set();
                        }
                    }
                    finally { Monitor.Exit(createLock); }
                }
            }
        }
        /// <summary>
        /// 创建线程对象
        /// </summary>
        /// <returns></returns>
        public abstract T CreateThread();
        /// <summary>
        /// 线程切换检测
        /// </summary>
        /// <param name="state"></param>
        private void check(object state)
        {
            if (CurrentThread.CurrentTaskTimestamp + switchTimestamp <= Stopwatch.GetTimestamp())
            {
                if (isAllThread)
                {
                    int taskIndex = ++this.threadIndex;
                    if (taskIndex >= threads.Length)
                    {
                        do
                        {
                            taskIndex -= threads.Length;
                        }
                        while (taskIndex >= threads.Length);
                        this.threadIndex = taskIndex;
                    }
                    CurrentThread = threads[taskIndex];
                }
                else
                {
                    Monitor.Enter(createLock);
                    try
                    {
                        if (!isAllThread && !IsDisposed)
                        {
                            CurrentThread = CreateThread();
                            threads[++threadIndex] = CurrentThread;
                            if (threadIndex + 1 == threads.Length) isAllThread = true;
                        }
                    }
                    catch (Exception error)
                    {
                        AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    finally { Monitor.Exit(createLock); }
                }
            }
        }
    }
    /// <summary>
    /// 超时切换任务线程集合
    /// </summary>
    public class TaskSwitchThreadArray : TaskSwitchThreadArray<TaskSwitchThread, ISwitchTaskNode>
    {
        /// <summary>
        /// 超时切换任务线程集合
        /// </summary>
        /// <param name="config"></param>
        public TaskSwitchThreadArray(TaskSwitchThreadConfig config = null) : base(config ?? DefaultConfig) { }
        /// <summary>
        /// 创建线程对象
        /// </summary>
        /// <returns></returns>
        public override TaskSwitchThread CreateThread() { return new TaskSwitchThread(this); }

        /// <summary>
        /// 默认超时切换任务线程集合
        /// </summary>
        internal static readonly TaskSwitchThreadArray Default = (TaskSwitchThreadArray)AutoCSer.Configuration.Common.Get(typeof(TaskSwitchThreadArray)) ?? new TaskSwitchThreadArray();
    }
}
