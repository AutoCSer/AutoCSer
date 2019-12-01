using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpInternalStreamServer
{
    /// <summary>
    /// TCP 内部服务端套接字任务处理
    /// </summary>
    internal sealed class ServerSocketTask : AutoCSer.Threading.LinkTaskThread<ServerSocket>
    {
        /// <summary>
        /// TCP 内部服务端套接字任务处理
        /// </summary>
        /// <param name="taskTimestamp">线程切换超时时钟周期</param>
        private ServerSocketTask(long taskTimestamp) : base(taskTimestamp) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ServerSocket value)
        {
            value.TaskTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
            ServerSocket headValue;
            do
            {
                if ((headValue = head) == null)
                {
                    value.NextTask = null;
                    if (Interlocked.CompareExchange(ref head, value, null) == null)
                    {
                        waitHandle.Set();
                        return;
                    }
                }
                else
                {
                    value.NextTask = headValue;
                    if (Interlocked.CompareExchange(ref head, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.YieldOnly();
            }
            while (true);
        }
        /// <summary>
        /// TCP服务端套接字任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                currentTaskTimestamp = taskTimestamp;
                waitHandle.Wait();
                ServerSocket value = Interlocked.Exchange(ref head, null);
                do
                {
                    try
                    {
                        do
                        {
                            value.RunTask(ref value, ref currentTaskTimestamp);
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                    }
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// TCP 内部服务端套接字任务处理
        /// </summary>
        internal static ServerSocketTask Task;
        /// <summary>
        /// TCP 内部服务端套接字任务处理集合
        /// </summary>
        private static readonly ServerSocketTask[] tasks;
        /// <summary>
        /// 线程切换检测
        /// </summary>
        private static void check()
        {
            if (TcpInternalServer.ServerSocketTaskConfig.Default.IsCheck(Task.currentTaskTimestamp))
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
                        Task = new ServerSocketTask(TcpInternalServer.ServerSocketTaskConfig.Default.TaskTimestamp);
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

        static ServerSocketTask()
        {
            TcpInternalServer.ServerSocketTaskConfig config = TcpInternalServer.ServerSocketTaskConfig.Default;
            if (config.ThreadCount == 1) Task = new ServerSocketTask(0);
            else
            {
                tasks = new ServerSocketTask[config.ThreadCount];
                tasks[0] = Task = new ServerSocketTask(config.TaskTimestamp);
                config.OnCheck(check);
            }
        }
    }
}
