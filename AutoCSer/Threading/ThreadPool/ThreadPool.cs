using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程池
    /// </summary>
    public sealed class ThreadPool
    {
        /// <summary>
        /// 最低线程堆栈大小 128KB
        /// </summary>
        internal const int TinyStackSize = 128 << 10;
        /// <summary>
        /// 默认线程堆栈大小 1MB
        /// </summary>
        private const int defaultStackSize = 1 << 20;

        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 线程堆栈大小
        /// </summary>
        internal readonly int StackSize;
        /// <summary>
        /// 是否后台线程
        /// </summary>
        internal readonly bool IsBackground;
        /// <summary>
        /// 线程链表
        /// </summary>
        private Thread.YieldLink threads;
        /// <summary>
        /// 空闲线程数量
        /// </summary>
        private volatile int freeThreadCount;
        /// <summary>
        /// 线程池
        /// </summary>
        /// <param name="stackSize">线程堆栈大小</param>
        /// <param name="isBackground">是否后台线程</param>
        private ThreadPool(int stackSize = defaultStackSize, bool isBackground = false)
        {
            StackSize = Math.Max(stackSize, TinyStackSize);
            IsBackground = isBackground;
        }
        /// <summary>
        /// 后台线程入池
        /// </summary>
        /// <param name="thread">线程池线程</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PushBackground(Thread thread)
        {
            threads.Push(thread);
            System.Threading.Interlocked.Increment(ref freeThreadCount);
        }
        /// <summary>
        /// 前台线程入池
        /// </summary>
        /// <param name="thread">线程池线程</param>
        /// <returns>线程池是否已经释放</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Push(Thread thread)
        {
            if (isDisposed == 0)
            {
                threads.Push(thread);
                System.Threading.Interlocked.Increment(ref freeThreadCount);
                if (isDisposed == 0) return false;
            }
            return true;
        }
        /// <summary>
        /// 前台线程退出
        /// </summary>
        private void exit()
        {
            isDisposed = 1;
            System.Threading.Interlocked.Exchange(ref freeThreadCount, 0);
            Thread thread = threads.GetClear();
            while (thread != null) thread = thread.StopLink();
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FastStart(Action task)
        {
            Thread thread = threads.Pop();
            if (thread == null) new Thread(this, task, ThreadTaskType.Action);
            else
            {
                thread.RunTask(task, ThreadTaskType.Action);
                System.Threading.Interlocked.Decrement(ref freeThreadCount);
            }
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        /// <param name="taskType">任务委托调用类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FastStart(object task, ThreadTaskType taskType)
        {
            Thread thread = threads.Pop();
            if (thread == null) new Thread(this, task, taskType);
            else
            {
                thread.RunTask(task, taskType);
                System.Threading.Interlocked.Decrement(ref freeThreadCount);
            }
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Start(Action task)
        {
            if (task != null && isDisposed == 0)
            {
                FastStart(task);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放多余线程
        /// </summary>
        /// <returns></returns>
        private int releaseFree()
        {
            int count = AutoCSer.Common.ProcessorCount, freeCount = 0;
            while (freeThreadCount > count)
            {
                Thread thread = threads.Pop();
                if (thread == null) return freeCount;
                System.Threading.Interlocked.Decrement(ref freeThreadCount);
                thread.Stop();
                ++freeCount;
            }
            return freeCount;
        }
        /// <summary>
        /// 释放多余线程
        /// </summary>
        /// <returns></returns>
        private int releaseFreeBackground()
        {
            Thread exitThread = null;
            int count = AutoCSer.Common.ProcessorCount, freeCount = 0;
            while (freeThreadCount > count)
            {
                Thread thread = threads.Pop();
                if (thread == null) break;
                if (object.ReferenceEquals(thread, backgroundExitThread)) exitThread = thread;
                else
                {
                    System.Threading.Interlocked.Decrement(ref freeThreadCount);
                    thread.Stop();
                    ++freeCount;
                }
            }
            if (exitThread != null) threads.Push(exitThread);
            return freeCount;
        }

        /// <summary>
        /// 微型线程池,堆栈 128K
        /// </summary>
        public static readonly ThreadPool Tiny = new ThreadPool(TinyStackSize);
        /// <summary>
        /// 微型后台线程池,堆栈 128K
        /// </summary>
        public static readonly ThreadPool TinyBackground = new ThreadPool(TinyStackSize, true);
        /// <summary>
        /// 后台退出测试线程
        /// </summary>
        private static readonly Thread backgroundExitThread = new Thread(TinyBackground);
        /// <summary>
        /// 前台退出测试
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CheckExit()
        {
            if (backgroundExitThread.IsAborted)
            {
                TinyBackground.isDisposed = 1;
                Tiny.exit();
            }
        }
        /// <summary>
        /// 释放多余线程累计数量
        /// </summary>
        private static int releaseFreeThreadCount;
        /// <summary>
        /// 释放多余线程
        /// </summary>
        private static void releaseFreeThread()
        {
            if (releaseFreeThreadCount >= AutoCSer.Common.ProcessorCount)
            {
                Thread.RemoveStop();
                releaseFreeThreadCount = 0;
            }
            releaseFreeThreadCount += Tiny.releaseFree() + TinyBackground.releaseFreeBackground();
        }
        static ThreadPool()
        {
            SecondTimer.InternalTaskArray.AppendMinute(releaseFreeThread);
        }
    }
}
