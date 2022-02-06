using System;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpOpenSimpleServer
{
    /// <summary>
    /// TCP 服务端套接字任务处理
    /// </summary>
    internal sealed class ServerSocketThread : AutoCSer.Threading.TaskSwitchThread<ServerSocket>
    {
        /// <summary>
        /// TCP 服务端套接字任务线程
        /// </summary>
        /// <param name="threadArray">TCP 服务端套接字任务线程集合</param>
        internal ServerSocketThread(ServerSocketThreadArray threadArray) : base(threadArray) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ServerSocket value)
        {
            value.TaskTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
            ServerSocket head;
            do
            {
                if ((head = Head) == null)
                {
                    value.NextTask = null;
                    if (Interlocked.CompareExchange(ref Head, value, null) == null)
                    {
                        WaitHandle.Set();
                        return;
                    }
                }
                else
                {
                    value.NextTask = head;
                    if (Interlocked.CompareExchange(ref Head, value, head) == head) return;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// TCP服务端套接字任务处理
        /// </summary>
        protected internal override void run()
        {
            do
            {
                CurrentTaskTimestamp = threadArray.NoSwitchTimestamp;
                WaitHandle.Wait();
                ServerSocket value = Interlocked.Exchange(ref Head, null);
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
            while (true);
        }
    }
}
