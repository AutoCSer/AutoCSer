using System;
using AutoCSer.Extension;
using System.Threading;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 链表任务节点接口
    /// </summary>
    internal interface ILinkTask
    {
        /// <summary>
        /// 线程切换检测时间
        /// </summary>
        long LinkTaskTicks { get; set; }
        /// <summary>
        /// 下一个任务节点
        /// </summary>
        ILinkTask NextLinkTask { get; set; }
        /// <summary>
        /// 执行任务
        /// </summary>
        ILinkTask SingleRunLinkTask();
    }
    /// <summary>
    /// 链表任务
    /// </summary>
    internal sealed class LinkTask
    {
        /// <summary>
        /// 链表任务节点
        /// </summary>
        private ILinkTask head;
        ///// <summary>
        ///// 链表任务访问锁
        ///// </summary>
        //private int linkLock;
        /// <summary>
        /// 是否启动线程
        /// </summary>
        private int isThread;
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ILinkTask value)
        {
            value.LinkTaskTicks = AutoCSer.Pub.Stopwatch.ElapsedTicks;
            ILinkTask headValue;
            do
            {
                if ((headValue = head) == null)
                {
                    value.NextLinkTask = null;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null)
                    {
                        if (System.Threading.Interlocked.CompareExchange(ref isThread, 1, 0) == 0) runThread();
                        return;
                    }
                }
                else
                {
                    value.NextLinkTask = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, headValue) == headValue)
                    {
                        if (System.Threading.Interlocked.CompareExchange(ref isThread, 1, 0) == 0) runThread();
                        return;
                    }
                }
                AutoCSer.Threading.ThreadYield.YieldOnly();
            }
            while (true);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int addNullHead(ILinkTask value)
        {
            if (head != null) System.Threading.Thread.Sleep(0);
            value.LinkTaskTicks = AutoCSer.Pub.Stopwatch.ElapsedTicks;
            ILinkTask headValue;
            do
            {
                if ((headValue = head) == null)
                {
                    value.NextLinkTask = null;
                    if (System.Threading.Interlocked.CompareExchange(ref head, value, null) == null)
                    {
                        if (System.Threading.Interlocked.CompareExchange(ref isThread, 1, 0) == 0) runThread();
                        return 1;
                    }
                    AutoCSer.Threading.ThreadYield.YieldOnly();
                }
                else return 0;
            }
            while (true);
        }
        /// <summary>
        /// 启动线程
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void runThread()
        {
            try
            {
                ThreadPool.TinyBackground.FastStart(this, Threading.Thread.CallType.LinkTaskRun);
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                Run();
            }
        }
        /// <summary>
        /// 链表任务处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Run()
        {
            do
            {
            START:
                ILinkTask value = System.Threading.Interlocked.Exchange(ref head, null);
                if (value == null)
                {
                    System.Threading.Thread.Sleep(0);
                    if ((value = System.Threading.Interlocked.Exchange(ref head, null)) == null)
                    {
                        //isThread = 0;
                        System.Threading.Interlocked.Exchange(ref isThread, 0);
                        if (head != null && System.Threading.Interlocked.CompareExchange(ref isThread, 1, 0) == 0) goto START;
                        return;
                    }
                }
                do
                {
                    value = value.SingleRunLinkTask();
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// 链表任务
        /// </summary>
        internal static LinkTask Task;
        /// <summary>
        /// 链表任务集合
        /// </summary>
        private static readonly LinkTask[] tasks;
        /// <summary>
        /// 当前任务处理索引
        /// </summary>
        private static int taskIndex;
        /// <summary>
        /// 是否已经创建所有任务
        /// </summary>
        private static bool isAllTask;
        /// <summary>
        /// 线程切换检测
        /// </summary>
        private static void check()
        {
            ILinkTask value = Task.head;
            if (value != null && LinkTaskConfig.Default.IsCheck(value.LinkTaskTicks))
            {
                if (isAllTask)
                {
                    if (++taskIndex == tasks.Length) taskIndex = 0;
                    Task = tasks[taskIndex];
                }
                else
                {
                    try
                    {
                        Task = new LinkTask();
                        tasks[++taskIndex] = Task;
                        if (taskIndex + 1 == tasks.Length) isAllTask = true;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                    }
                }
            }
        }

        static LinkTask()
        {
            LinkTaskConfig config = LinkTaskConfig.Default;
            if (config.ThreadCount == 1) Task = new LinkTask();
            else
            {
                tasks = new LinkTask[config.ThreadCount];
                tasks[0] = Task = new LinkTask();
                config.OnCheck(check);
            }
        }
    }
}
