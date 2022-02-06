using System;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务端同步调用任务处理线程
    /// </summary>
    internal sealed class ServerCallThread : AutoCSer.Threading.TaskSwitchThread<ServerCallBase>
    {
        /// <summary>
        /// TCP 服务端同步调用任务处理线程
        /// </summary>
        /// <param name="threadArray">TCP 服务端套接字任务线程集合</param>
        internal ServerCallThread(ServerCallThreadArray threadArray) : base(threadArray) { }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ServerCallBase value)
        {
            value.SwitchTaskTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
            ServerCallBase head;
            do
            {
                if ((head = Head) == null)
                {
                    value.LinkNext = null;
                    if (Interlocked.CompareExchange(ref Head, value, null) == null)
                    {
                        WaitHandle.Set();
                        return;
                    }
                }
                else
                {
                    value.LinkNext = head;
                    if (Interlocked.CompareExchange(ref Head, value, head) == head) return;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckAdd(ServerCallBase value)
        {
            if (value.LinkNext == null)
            {
                Add(value);
                return true;
            }
            return false;
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
                ServerCallBase value = Interlocked.Exchange(ref Head, null);
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
