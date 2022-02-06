using System;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 客户端回调任务处理
    /// </summary>
    internal sealed class ClientCallThread : AutoCSer.Threading.TaskSwitchThread<ClientCommand.CommandBase>
    {
        /// <summary>
        /// TCP 客户端回调任务处理
        /// </summary>
        /// <param name="threadArray">TCP 客户端任务处理线程集合</param>
        internal ClientCallThread(ClientCallThreadArray threadArray) : base(threadArray) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ClientCommand.CommandBase value)
        {
            value.TaskTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
            ClientCommand.CommandBase head;
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
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        protected internal override void run()
        {
            do
            {
                CurrentTaskTimestamp = threadArray.NoSwitchTimestamp;
                WaitHandle.Wait();
                ClientCommand.CommandBase value = Interlocked.Exchange(ref Head, null);
                do
                {
                    try
                    {
                        do
                        {
                            value.OnReceiveTask(ref value, ref CurrentTaskTimestamp);
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
