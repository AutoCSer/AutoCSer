using System;
using System.Threading;
using AutoCSer.Extension;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用队列处理
    /// </summary>
    internal class ServerCallQueue : AutoCSer.Threading.QueueTaskThread<ServerCallBase>
    {
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ServerCallBase value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (head == null)
            {
                end = value;
                head = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                waitHandle.Set();
            }
            else
            {
                end.NextTask = value;
                end = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool CheckAdd(ServerCallBase value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (value.NextTask == null && value != end)
            {
                if (head == null)
                {
                    end = value;
                    head = value;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                    waitHandle.Set();
                }
                else
                {
                    end.NextTask = value;
                    end = value;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                }
                return true;
            }
            System.Threading.Interlocked.Exchange(ref queueLock, 0);
            return false;
        }
        /// <summary>
        /// TCP 服务器端同步调用任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                waitHandle.Wait();
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                ServerCallBase value = head;
                end = null;
                head = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                do
                {
                    value = value.SingleRunTask();
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        internal static readonly ServerCallQueue Default = new ServerCallQueue();
    }
}
