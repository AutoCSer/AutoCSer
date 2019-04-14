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
        /// <param name="taskTicks">线程切换超时时钟周期</param>
        private ServerCallTask(long taskTicks) : base(taskTicks) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ServerCallBase value)
        {
            value.TaskTicks = AutoCSer.Pub.Stopwatch.ElapsedTicks;
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
                currentTaskTicks = taskTicks;
                waitHandle.Wait();
                ServerCallBase value = Interlocked.Exchange(ref head, null);
                do
                {
                    value = value.SingleRunTask(ref currentTaskTicks);
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
            if (TaskConfig.Default.IsCheck(Task.currentTaskTicks))
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
                        Task = new ServerCallTask(TaskConfig.Default.TaskTicks);
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
                tasks[0] = Task = new ServerCallTask(config.TaskTicks);
                config.OnCheck(check);
            }
        }
    }
}
