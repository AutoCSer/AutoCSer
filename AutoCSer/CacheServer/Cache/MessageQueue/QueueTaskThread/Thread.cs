using System;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 消息队列读取操作队列线程
    /// </summary>
    internal sealed class Thread : AutoCSer.Threading.QueueTaskThread<Node>
    {
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(Node value)
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
                end.LinkNext = value;
                end = value;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        internal void Add(Node head, Node end)
        {
            while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
            if (this.head == null)
            {
                this.end = end;
                this.head = head;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                waitHandle.Set();
            }
            else
            {
                this.end.LinkNext = head;
                this.end = end;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
            }
        }
        /// <summary>
        /// 消息队列读取操作任务处理
        /// </summary>
        protected override void run()
        {
            do
            {
                waitHandle.Wait();
                while (System.Threading.Interlocked.CompareExchange(ref queueLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.YieldOnly();
                Node value = head;
                end = null;
                head = null;
                System.Threading.Interlocked.Exchange(ref queueLock, 0);
                do
                {
                    try
                    {
                        do
                        {
                            value = value.RunTask();
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        value.MessageQueue.Cache.TcpServer.AddLog(error);
                    }
                    value = value.LinkNext;
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// 消息队列读取操作队列线程
        /// </summary>
        internal static readonly Thread Default = new Thread();
    }
}
