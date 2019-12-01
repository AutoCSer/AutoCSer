using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用任务处理
    /// </summary>
    internal sealed class ServerCallTask : AutoCSer.Threading.LinkTaskThread<ServerCallBase>
    {
        /// <summary>
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        /// <param name="taskTimestamp">线程切换超时时钟周期</param>
        private ServerCallTask(long taskTimestamp) : base(taskTimestamp) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ServerCallBase value)
        {
            value.TaskTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
            ServerCallBase headValue;
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
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                currentTaskTimestamp = taskTimestamp;
                waitHandle.Wait();
                ServerCallBase value = Interlocked.Exchange(ref head, null);
                do
                {
                    try
                    {
                        do
                        {
                            value.SingleRunTask(ref value, ref currentTaskTimestamp);
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
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        internal static ServerCallTask Task;
        /// <summary>
        /// TCP 服务器端同步调用任务处理集合
        /// </summary>
        private static readonly ServerCallTask[] tasks;
        /// <summary>
        /// 线程切换检测
        /// </summary>
        private static void check()
        {
            if (TaskConfig.Default.IsCheck(Task.currentTaskTimestamp))
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
                        Task = new ServerCallTask(TaskConfig.Default.TaskTimestamp);
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

        static ServerCallTask()
        {
            TaskConfig config = TaskConfig.Default;
            if (config.ThreadCount == 1) Task = new ServerCallTask(0);
            else
            {
                tasks = new ServerCallTask[config.ThreadCount];
                tasks[0] = Task = new ServerCallTask(config.TaskTimestamp);
                config.OnCheck(check);
            }
        }
    }
}
