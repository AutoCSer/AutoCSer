using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 超时切换任务线程
    /// </summary>
    public abstract class TaskSwitchThreadBase
    {
        /// <summary>
        /// 超时切换任务线程集合
        /// </summary>
        protected readonly TaskSwitchThreadArrayBase threadArray;
        /// <summary>
        /// 等待事件
        /// </summary>
        internal OnceAutoWaitHandle WaitHandle;
        /// <summary>
        /// 当前处理任务时钟周期
        /// </summary>
        internal long CurrentTaskTimestamp;
        /// <summary>
        /// 超时切换任务线程
        /// </summary>
        /// <param name="threadArray">超时切换任务线程集合</param>
        protected TaskSwitchThreadBase(TaskSwitchThreadArrayBase threadArray)
        {
            this.threadArray = threadArray;
            CurrentTaskTimestamp = threadArray.NoSwitchTimestamp;
            WaitHandle.Set(0);
            ThreadPool.TinyBackground.FastStart(this, AutoCSer.Threading.ThreadTaskType.TaskSwitchThreadRun);
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected internal abstract void run();
    }
    /// <summary>
    /// 超时切换任务线程
    /// </summary>
    /// <typeparam name="T">任务对象类型</typeparam>
    public abstract class TaskSwitchThread<T> : TaskSwitchThreadBase
        where T : class
    {
        /// <summary>
        /// 链表头部
        /// </summary>
        internal T Head;
        /// <summary>
        /// 超时切换任务线程
        /// </summary>
        /// <param name="threadArray">超时切换任务线程集合</param>
        protected TaskSwitchThread(TaskSwitchThreadArrayBase threadArray) : base(threadArray) { }
    }
    /// <summary>
    /// 超时切换任务线程
    /// </summary>
    public sealed class TaskSwitchThread : TaskSwitchThread<ISwitchTaskNode>
    {
        /// <summary>
        /// 超时切换任务线程
        /// </summary>
        /// <param name="threadArray">超时切换任务线程集合</param>
        internal TaskSwitchThread(TaskSwitchThreadArray threadArray) : base(threadArray) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ISwitchTaskNode value)
        {
            value.SwitchTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
            ISwitchTaskNode head;
            do
            {
                if ((head = Head) == null)
                {
                    value.NextSwitchTask = null;
                    if (System.Threading.Interlocked.CompareExchange(ref Head, value, null) == null)
                    {
                        WaitHandle.Set();
                        return;
                    }
                }
                else
                {
                    value.NextSwitchTask = head;
                    if (System.Threading.Interlocked.CompareExchange(ref Head, value, head) == head) return;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckAdd(ISwitchTaskNode value)
        {
            if (value.NextSwitchTask == null)
            {
                Add(value);
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 添加任务
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private int addNullHead(ISwitchTaskNode value)
        //{
        //    if (Head != null) System.Threading.Thread.Sleep(0);
        //    value.SwitchTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        //    ISwitchTaskNode headValue;
        //    do
        //    {
        //        if ((headValue = Head) == null)
        //        {
        //            value.NextSwitchTask = null;
        //            if (System.Threading.Interlocked.CompareExchange(ref Head, value, null) == null)
        //            {
        //                if (System.Threading.Interlocked.CompareExchange(ref isThread, 1, 0) == 0) runThread();
        //                return 1;
        //            }
        //            AutoCSer.Threading.ThreadYield.YieldOnly();
        //        }
        //        else return 0;
        //    }
        //    while (true);
        //}
        /// <summary>
        /// 链表任务处理
        /// </summary>
        protected internal override void run()
        {
            do
            {
                CurrentTaskTimestamp = threadArray.NoSwitchTimestamp;
                WaitHandle.Wait();
                if (threadArray.IsDisposed) break;
                ISwitchTaskNode value = System.Threading.Interlocked.Exchange(ref Head, null);
                do
                {
                    try
                    {
                        do
                        {
                            value.RunTask(ref value, ref CurrentTaskTimestamp);
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.LogHelper.Exception(error);
                    }
                }
                while (value != null);
            }
            while (!threadArray.IsDisposed);

            System.Threading.Interlocked.Exchange(ref Head, null);
        }
    }
}
