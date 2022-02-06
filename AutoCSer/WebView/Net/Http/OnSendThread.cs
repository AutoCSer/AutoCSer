using System;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字数据发送完成后的任务处理
    /// </summary>
    internal sealed class OnSendThread : AutoCSer.Threading.TaskSwitchThread<Socket>
    {
        /// <summary>
        /// HTTP 套接字数据发送完成后的任务处理
        /// </summary>
        /// <param name="threadArray"></param>
        internal OnSendThread(OnSendThreadArray threadArray) : base(threadArray) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(Socket value)
        {
            value.TaskTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
            Socket head;
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
        /// HTTP 套接字数据发送完成后的任务处理
        /// </summary>
        protected internal override void run()
        {
            do
            {
                CurrentTaskTimestamp = threadArray.NoSwitchTimestamp;
                WaitHandle.Wait();
                Socket value = Interlocked.Exchange(ref Head, null);
                do
                {
                    try
                    {
                        do
                        {
                            value.SingleRunTask(ref value, ref CurrentTaskTimestamp);
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
