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
    public class ServerCallQueue : AutoCSer.Threading.QueueTaskThread<ServerCallBase>
    {
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="isStart">是否启动线程</param>
        internal ServerCallQueue(bool isBackground, bool isStart) : base(isBackground, isStart) { }
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        public ServerCallQueue(bool isBackground = true) : this(isBackground, true) { }
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
                WaitHandle.Set();
            }
            else
            {
                end.LinkNext = value;
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
            if (value.LinkNext == null && value != end)
            {
                if (head == null)
                {
                    end = value;
                    head = value;
                    System.Threading.Interlocked.Exchange(ref queueLock, 0);
                    WaitHandle.Set();
                }
                else
                {
                    end.LinkNext = value;
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
                WaitHandle.Wait();
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                ServerCallBase value = head;
                end = null;
                head = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                do
                {
                    try
                    {
                        do
                        {
                            value.RunTask(ref value);
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
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        internal static readonly ServerCallQueue Default = new ServerCallQueue();
    }
}
