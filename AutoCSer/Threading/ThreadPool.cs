using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程池
    /// </summary>
    public sealed class ThreadPool : Link<ThreadPool>
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
        /// 下一个节点
        /// </summary>
        private ThreadPool exitNext;
        /// <summary>
        /// 上一个节点
        /// </summary>
        private ThreadPool exitPrevious;
        /// <summary>
        /// 线程链表
        /// </summary>
        private Thread.YieldLink threads;
        /// <summary>
        /// 线程堆栈大小
        /// </summary>
        internal int StackSize;
        /// <summary>
        /// 是否后台线程
        /// </summary>
        internal bool IsBackground;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 线程池
        /// </summary>
        /// <param name="stackSize">线程堆栈大小</param>
        /// <param name="isBackground">是否后台线程</param>
        private ThreadPool(int stackSize = defaultStackSize, bool isBackground = false)
        {
            this.StackSize = stackSize < TinyStackSize ? TinyStackSize : stackSize;
            IsBackground = isBackground;
            if (!isBackground)
            {
                if (isThreadPoolExit == 0) exitThreadPools.Push(this);
                else
                {
                    isDisposed = true;
                    disposePool();
                }
            }
            if (!isDisposed) AutoCSer.DomainUnload.Unloader.Add(this, AutoCSer.DomainUnload.Type.ThreadPoolDispose);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void Dispose()
        {
            isDisposed = true;
            if (!IsBackground && isThreadPoolExit == 0) exitThreadPools.Pop(this);
            disposePool();
        }
        /// <summary>
        /// 释放线程池
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void disposePool()
        {
            Thread thread = threads.GetClear();
            while (thread != null) thread = thread.StopLink();
        }
        /// <summary>
        /// 线程入池
        /// </summary>
        /// <param name="thread">线程池线程</param>
        /// <returns>是否已经释放资源</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Push(Thread thread)
        {
            if (isDisposed) return true;
            threads.PushNotNull(thread);
            if (isDisposed) disposePool();
            return false;
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FastStart(Action task)
        {
            Thread thread = threads.Pop();
            if (thread == null) new Thread(this, task, AutoCSer.Threading.Thread.CallType.Action);
            else thread.RunTask(task, AutoCSer.Threading.Thread.CallType.Action);
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        /// <param name="taskType">任务委托调用类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FastStart(object task, AutoCSer.Threading.Thread.CallType taskType)
        {
            Thread thread = threads.Pop();
            if (thread == null) new Thread(this, task, taskType);
            else thread.RunTask(task, taskType);
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        /// <param name="taskType">任务委托调用类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CheckStart(object task, AutoCSer.Threading.Thread.CallType taskType)
        {
            Thread thread = threads.Pop();
            if (thread == null)
            {
                System.Threading.Thread.Sleep(0);
                if ((thread = threads.Pop()) == null)
                {
                    new Thread(this, task, taskType);
                    return;
                }
            }
            thread.RunTask(task, taskType);
        }
        /// <summary>
        /// 获取一个线程并执行任务
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Start(Action task)
        {
            if (isDisposed) throw new ObjectDisposedException("ThreadPool is disposed");
            if (task != null) FastStart(task);
            if (isDisposed) disposePool();
        }

        /// <summary>
        /// 双向链表
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct ExitLink
        {
            /// <summary>
            /// 链表尾部
            /// </summary>
            internal ThreadPool End;
            /// <summary>
            /// 链表访问锁
            /// </summary>
            private int linkLock;
            /// <summary>
            /// 获取链表尾部并清除数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ThreadPool GetClear()
            {
                while (System.Threading.Interlocked.CompareExchange(ref linkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePop);
                ThreadPool end = End;
                End = null;
                System.Threading.Interlocked.Exchange(ref linkLock, 0);
                return end;
            }
            /// <summary>
            /// 添加节点
            /// </summary>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push(ThreadPool value)
            {
                while (System.Threading.Interlocked.CompareExchange(ref linkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePush);
                if (End != null)
                {
                    End.exitNext = value;
                    value.exitPrevious = End;
                }
                End = value;
                System.Threading.Interlocked.Exchange(ref linkLock, 0);
            }
            /// <summary>
            /// 弹出节点
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否成功</returns>
            internal int Pop(ThreadPool value)
            {
                while (System.Threading.Interlocked.CompareExchange(ref linkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePop);
                ThreadPool previous = value.exitPrevious, next = value.exitNext;
                if (previous == null)
                {
                    if (next == null)
                    {
                        System.Threading.Interlocked.Exchange(ref linkLock, 0);
                        return 0;
                    }
                    value.exitNext = next.exitPrevious = null;
                    System.Threading.Interlocked.Exchange(ref linkLock, 0);
                }
                else if (next == null)
                {
                    End = previous;
                    previous.exitNext = next;
                    value.exitPrevious = null;
                    System.Threading.Interlocked.Exchange(ref linkLock, 0);
                }
                else
                {
                    next.exitPrevious = previous;
                    previous.exitNext = next;
                    value.clearExit();
                    System.Threading.Interlocked.Exchange(ref linkLock, 0);
                }
                return 1;
            }
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
        /// 退出测试线程
        /// </summary>
        private static readonly Thread exitTestThread = new Thread(TinyBackground);
        /// <summary>
        /// 前台线程池集合
        /// </summary>
        private static ExitLink exitThreadPools;
        /// <summary>
        /// 前台线程池集合访问锁
        /// </summary>
        private static volatile int isThreadPoolExit;
        /// <summary>
        /// 激活计时器
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly DateTime timer = Date.NowTime.Now;
        /// <summary>
        /// 前台退出测试
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CheckExit()
        {
            if (exitTestThread.IsAborted && System.Threading.Interlocked.CompareExchange(ref isThreadPoolExit, 1, 0) == 0) exit();
        }
        /// <summary>
        /// 前台线程退出
        /// </summary>
        private static void exit()
        {
            ThreadPool threadPool = exitThreadPools.GetClear();
            if (threadPool != null)
            {
                do
                {
                    threadPool.Dispose();
                    ThreadPool previous = threadPool.exitPrevious;
                    if (previous == null) return;
                    threadPool.exitPrevious = null;
                    previous.exitNext = null;
                    threadPool = previous;
                }
                while (true);
            }
        }
        /// <summary>
        /// 清除线程退出数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void clearExit()
        {
            exitNext = exitPrevious = null;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            Tiny.threads.ClearCache(count);
            TinyBackground.threads.ClearCache(count);
        }

        static ThreadPool()
        {
            AutoCSer.Pub.ClearCaches += clearCache;
        }
    }
}
