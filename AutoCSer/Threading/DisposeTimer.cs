using System;
using System.Net.Sockets;
using System.Threading;
using fastCSharp.Extension;

namespace fastCSharp.Threading
{
    /// <summary>
    /// 垃圾定时清理
    /// </summary>
    internal static class DisposeTimer
    {
        /// <summary>
        /// 避免 GC 压缩
        /// </summary>
        private const int arraySize = (128 << 10) >> 2;
        /// <summary>
        /// 清理类型
        /// </summary>
        internal enum Type : byte
        {
            /// <summary>
            /// 关闭套接字
            /// </summary>
            SocketDispose,
            ///// <summary>
            ///// 关闭套接字
            ///// </summary>
            //SocketShutdown,
        }
        /// <summary>
        /// 任务信息
        /// </summary>
        private sealed class Task
        {
            /// <summary>
            /// 清理目标对象
            /// </summary>
            public object Value;
            /// <summary>
            /// 清理类型
            /// </summary>
            internal Type Type;
            /// <summary>
            /// 设置任务调用
            /// </summary>
            /// <param name="value"></param>
            /// <param name="type"></param>
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
            public void Set(object value, Type type)
            {
                Value = value;
                Type = type;
            }
            /// <summary>
            /// 任务调用
            /// </summary>
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
            public void Call()
            {
                switch (Type)
                {
                    case Type.SocketDispose: new UnionType { Value = Value }.Socket.Dispose(); Value = null; return;
                    //case type.SocketShutdown: fastCSharp.net.socket.Shutdown(new unionType { Value = Value }.Socket); Value = null; return;
                }
            }
        }
        /// <summary>
        /// 空闲任务信息集合
        /// </summary>
        private static LeftArray<Task[]> freeTasks;
        /// <summary>
        /// 空闲任务信息集合访问锁
        /// </summary>
        private static readonly object freeTaskLock = new object();
        /// <summary>
        /// 当前任务信息集合
        /// </summary>
        private static LeftArray<Task[]> tasks;
        /// <summary>
        /// 当前任务信息集合
        /// </summary>
        private static LeftArray<Task> currentTasks;
        /// <summary>
        /// 任务信息集合访问锁
        /// </summary>
        private static readonly object taskLock = new object();
        /// <summary>
        /// 添加垃圾清理任务
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        private static void add(object value, Type type)
        {
            Monitor.Enter(taskLock);
            if (currentTasks.Array == null)
            {
                Monitor.Enter(freeTaskLock);
                if (freeTasks.Length == 0)
                {
                    Monitor.Exit(freeTaskLock);
                    try
                    {
                        Task[] array = new Task[arraySize];
                        array[0].Set(value, type);
                        currentTasks.Set(array, 1);
                    }
                    finally { Monitor.Exit(taskLock); }
                }
                else
                {
                    Task[] array = freeTasks.Array[--freeTasks.Length];
                    Monitor.Exit(freeTaskLock);
                    array[0].Set(value, type);
                    currentTasks.Set(array, 1);
                    Monitor.Exit(taskLock);
                }
            }
            else
            {
                currentTasks.Array[currentTasks.Length++].Set(value, type);
                if (currentTasks.Length == arraySize)
                {
                    Task[] array = currentTasks.GetNull();
                    try
                    {
                        tasks.Add(array);
                    }
                    finally { Monitor.Exit(taskLock); }
                }
                else Monitor.Exit(taskLock);
            }
        }
        /// <summary>
        /// 添加垃圾清理任务
        /// </summary>
        /// <param name="socket"></param>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static void Add(ref Socket socket)
        {
            add(socket, Type.SocketDispose);
            socket = null;
        }
        /// <summary>
        /// 是否已经触发定时任务
        /// </summary>
        private static int isTimer;
        /// <summary>
        /// 触发定时任务
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static void OnTimer()
        {
            if ((tasks.Length | currentTasks.Length) != 0 && System.Threading.Interlocked.CompareExchange(ref isTimer, 1, 0) == 0)
            {
                onTimer();
                isTimer = 0;
            }
        }
        /// <summary>
        /// 触发定时任务
        /// </summary>
        private static void onTimer()
        {
            Task[] taskAray = null;
            int index = 0;
            do
            {
            STRAT:
                Monitor.Enter(taskLock);
                if (tasks.Length == 0)
                {
                    if (currentTasks.Length == 0)
                    {
                        Monitor.Exit(taskLock);
                        return;
                    }
                    currentTasks.GetNull(ref taskAray, ref index);
                }
                else
                {
                    taskAray = tasks.Array[--tasks.Length];
                    index = arraySize;
                }
                Monitor.Exit(taskLock);
                do
                {
                    try
                    {
                        do
                        {
                            taskAray[--index].Call();
                        }
                        while (index != 0);
                        Monitor.Enter(freeTaskLock);
                        try
                        {
                            freeTasks.Add(taskAray);
                        }
                        finally { Monitor.Exit(freeTaskLock); }
                        goto STRAT;
                    }
                    catch (Exception error)
                    {
                        taskAray[index].Value = null;
                        fastCSharp.Log.Default.Log.add(fastCSharp.Log.Type.FastCSharp, error);
                    }
                }
                while (index != 0);
            }
            while (true);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        internal static void ClearCache()
        {
            Monitor.Enter(freeTaskLock);
            freeTasks.ClearLength();
            Monitor.Exit(freeTaskLock);
        }
    }
}
