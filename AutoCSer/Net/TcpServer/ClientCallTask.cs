using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 客户端回调任务处理
    /// </summary>
    internal sealed class ClientCallTask : AutoCSer.Threading.LinkTaskThread<ClientCommand.CommandBase>
    {
        /// <summary>
        /// TCP 客户端回调任务处理
        /// </summary>
        /// <param name="taskTicks">线程切换超时时钟周期</param>
        private ClientCallTask(long taskTicks) : base(taskTicks) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ClientCommand.CommandBase value)
        {
            value.TaskTicks = AutoCSer.Pub.Stopwatch.ElapsedTicks;
            ClientCommand.CommandBase headValue;
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
                ClientCommand.CommandBase value = Interlocked.Exchange(ref head, null);
                do
                {
                    currentTaskTicks = value.TaskTicks;
                    value = value.OnReceiveTask();
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// TCP 客户端回调任务处理
        /// </summary>
        internal static ClientCallTask Task;
        /// <summary>
        /// TCP 客户端回调任务处理集合
        /// </summary>
        private static readonly ClientCallTask[] tasks;
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
                        Task = new ClientCallTask(TaskConfig.Default.TaskTicks);
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

        static ClientCallTask()
        {
            TaskConfig config = TaskConfig.Default;
            if (config.ThreadCount == 1) Task = new ClientCallTask(0);
            else
            {
                tasks = new ClientCallTask[config.ThreadCount];
                tasks[0] = Task = new ClientCallTask(config.TaskTicks);
                config.OnCheck(check);
            }
        }
    }
}
