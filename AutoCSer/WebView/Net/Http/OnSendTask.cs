using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字数据发送完成后的任务处理
    /// </summary>
    internal sealed class OnSendTask : AutoCSer.Threading.LinkTaskThread<Socket>
    {
        /// <summary>
        /// HTTP 套接字数据发送完成后的任务处理
        /// </summary>
        /// <param name="taskTicks">线程切换超时时钟周期</param>
        private OnSendTask(long taskTicks) : base(taskTicks) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(Socket value)
        {
            value.TaskTicks = AutoCSer.Pub.Stopwatch.ElapsedTicks;
            Socket headValue;
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
        /// HTTP 套接字数据发送完成后的任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                currentTaskTicks = taskTicks;
                waitHandle.Wait();
                Socket value = Interlocked.Exchange(ref head, null);
                do
                {
                    value = value.SingleRunTask(ref currentTaskTicks);
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// HTTP 套接字数据发送完成后的任务处理
        /// </summary>
        internal static OnSendTask Task;
        /// <summary>
        /// HTTP 套接字数据发送完成后的任务处理集合
        /// </summary>
        private static readonly OnSendTask[] tasks;
        /// <summary>
        /// 线程切换检测
        /// </summary>
        private static void check()
        {
            if (TcpServer.TaskConfig.Default.IsCheck(Task.currentTaskTicks))
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
                        Task = new OnSendTask(TcpServer.TaskConfig.Default.TaskTicks);
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

        static OnSendTask()
        {
            TcpServer.TaskConfig config = TcpServer.TaskConfig.Default;
            if (config.ThreadCount == 1) Task = new OnSendTask(0);
            else
            {
                tasks = new OnSendTask[config.ThreadCount];
                tasks[0] = Task = new OnSendTask(config.TaskTicks);
                config.OnCheck(check);
            }
        }
    }
}
