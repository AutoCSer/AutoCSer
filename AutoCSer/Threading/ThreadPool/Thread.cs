using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程操作
    /// </summary>
    public partial class Thread : Link<Thread>
    {
        /// <summary>
        /// 线程首节点
        /// </summary>
        private static Thread threadHead;
        /// <summary>
        /// 删除已经停止的线程 访问锁
        /// </summary>
        private static readonly object removeLock = new object();
        /// <summary>
        /// 获取线程集合
        /// </summary>
        /// <param name="isCheckRun">是否检测运行状态，false 表示输出所有线程信息</param>
        /// <returns></returns>
        public static IEnumerable<Thread> GetThreads(bool isCheckRun = true)
        {
            Thread thread;
            Monitor.Enter(removeLock);
            thread = threadHead;
            Monitor.Exit(removeLock);
            while (thread != null)
            {
                if ((!isCheckRun || thread.CurrentTask != null) && !thread.isStop) yield return thread;
                thread = thread.threadLink;
            }
        }
        /// <summary>
        /// 删除已经停止的线程
        /// </summary>
        internal static void RemoveStop()
        {
            Monitor.Enter(removeLock);
            for (Thread thread = System.Threading.Interlocked.Exchange(ref threadHead, null); thread != null;)
            {
                if (thread.isStop) thread = thread.threadLink;
                else
                {
                    Thread nextThread = thread.threadLink;
                    thread.appendLink();
                    thread = nextThread;
                }
            }
            Monitor.Exit(removeLock);
        }

        /// <summary>
        /// 线程池
        /// </summary>
        private readonly ThreadPool threadPool;
        /// <summary>
        /// 线程句柄
        /// </summary>
        public readonly System.Threading.Thread Handle;
        /// <summary>
        /// 下一个节点
        /// </summary>
        private Thread threadLink;
        /// <summary>
        /// 线程是否已经退出
        /// </summary>
        internal bool IsAborted
        {
            get
            {
                return isStop || Handle.ThreadState == System.Threading.ThreadState.Aborted;
            }
        }
        /// <summary>
        /// 任务
        /// </summary>
        private ThreadTask task;
        /// <summary>
        /// 等待事件
        /// </summary>
        private OnceAutoWaitHandle waitHandle;
        /// <summary>
        /// 当前运行任务，null 表示空闲状态
        /// </summary>
        public object CurrentTask 
        {
            get 
            {
                object task = this.task.Value;
                return object.ReferenceEquals(task, this) ? null : task;
            }
        }
        /// <summary>
        /// 是否已经停止
        /// </summary>
        private bool isStop;
        /// <summary>
        /// 线程池线程
        /// </summary>
        /// <param name="threadPool">线程池</param>
        internal Thread(ThreadPool threadPool)
        {
            this.task.Value = this;
            waitHandle.Set(0);
            this.threadPool = threadPool;
            Handle = new System.Threading.Thread(exitTest, threadPool.StackSize);
            start();
        }
        /// <summary>
        /// 线程池线程
        /// </summary>
        /// <param name="threadPool">线程池</param>
        /// <param name="task">任务委托</param>
        /// <param name="taskType">任务委托调用类型</param>
        internal Thread(ThreadPool threadPool, object task, Threading.ThreadTaskType taskType)
        {
            this.task.Set(task, taskType);
            waitHandle.Set(0);
            this.threadPool = threadPool;
            Handle = threadPool.IsBackground ? new System.Threading.Thread(runBackground, threadPool.StackSize) : new System.Threading.Thread(run, threadPool.StackSize);
            start();
        }
        /// <summary>
        /// 添加到线程链表
        /// </summary>
        private void appendLink()
        {
            Thread head;
            do
            {
                if ((head = threadHead) == null)
                {
                    threadLink = null;
                    if (System.Threading.Interlocked.CompareExchange(ref threadHead, this, null) == null) return;
                }
                else
                {
                    threadLink = head;
                    if (System.Threading.Interlocked.CompareExchange(ref threadHead, this, head) == head) return;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 启动线程
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void start()
        {
            appendLink();
            Handle.IsBackground = true;
            Handle.Start();
        }
        /// <summary>
        /// 退出测试线程
        /// </summary>
        private void exitTest()
        {
            threadPool.PushBackground(this);
            waitHandle.Wait();
            if (task.Type != ThreadTaskType.None) runBackground();
        }
        /// <summary>
        /// 运行线程
        /// </summary>
        private void runBackground()
        {
            try
            {
                do
                {
                    try
                    {
                        do
                        {
                            task.Call();
                            task.Value = this;
                            threadPool.PushBackground(this);
                            waitHandle.Wait();
                        }
                        while (task.Type != Threading.ThreadTaskType.None);
                        return;
                    }
                    catch (Exception exception)
                    {
                        try
                        {
                            AutoCSer.LogHelper.Exception(exception, null, LogLevel.Exception | LogLevel.AutoCSer);
                        }
                        catch { }
                    }
                    finally
                    {
                        task.Value = this;
                    }
                    threadPool.PushBackground(this);
                    waitHandle.Wait();
                }
                while (task.Type != Threading.ThreadTaskType.None);
            }
            finally { isStop = true; }
        }
        /// <summary>
        /// 运行线程
        /// </summary>
        private void run()
        {
            try
            {
                do
                {
                    try
                    {
                        do
                        {
                            task.Call();
                            task.Value = this;
                            if (threadPool.Push(this)) return;
                            waitHandle.Wait();
                        }
                        while (task.Type != Threading.ThreadTaskType.None);
                        return;
                    }
                    catch (Exception exception)
                    {
                        try
                        {
                            AutoCSer.LogHelper.Exception(exception, null, LogLevel.Exception | LogLevel.AutoCSer);
                        }
                        catch { }
                    }
                    finally
                    {
                        task.Value = this;
                    }
                    if (threadPool.Push(this)) return;
                    waitHandle.Wait();
                }
                while (task.Type != Threading.ThreadTaskType.None);
            }
            finally { isStop = true; }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        /// <param name="taskType">任务委托调用类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RunTask(object task, Threading.ThreadTaskType taskType)
        {
            this.task.Set(task, taskType);
            waitHandle.Set();
        }
        /// <summary>
        /// 结束线程
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Stop()
        {
            task.Type = Threading.ThreadTaskType.None;
            waitHandle.Set();
        }
        /// <summary>
        /// 结束线程
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Thread StopLink()
        {
            Thread next = LinkNext;
            task.Type = Threading.ThreadTaskType.None;
            LinkNext = null;
            waitHandle.Set();
            return next;
        }
    }
}
