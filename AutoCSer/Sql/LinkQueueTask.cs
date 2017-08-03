using System;
using AutoCSer.Extension;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 链表任务队列
    /// </summary>
    internal sealed class LinkQueueTask
    {
        /// <summary>
        /// SQL 客户端操作
        /// </summary>
        private readonly Client client;
        /// <summary>
        /// 线程池
        /// </summary>
        private readonly AutoCSer.Threading.ThreadPool threadPool;
        /// <summary>
        /// 任务队列首节点
        /// </summary>
        private LinkQueueTaskNode head;
        /// <summary>
        /// 任务队列尾节点
        /// </summary>
        private LinkQueueTaskNode end;
        /// <summary>
        /// 任务队列访问锁
        /// </summary>
        private int queueLock;
        /// <summary>
        /// 是否启动线程
        /// </summary>
        private int isThread;
        /// <summary>
        /// 链表任务队列
        /// </summary>
        /// <param name="client">SQL 客户端操作</param>
        /// <param name="threadPool">线程池</param>
        internal LinkQueueTask(Client client, AutoCSer.Threading.ThreadPool threadPool)
        {
            this.client = client;
            this.threadPool = threadPool ?? AutoCSer.Threading.ThreadPool.TinyBackground;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(LinkQueueTaskNode value)
        {
            AutoCSer.Threading.Interlocked.CompareExchangeYield(ref queueLock, AutoCSer.Threading.ThreadYield.Type.SqlLinkQueueTaskPush);
            if (head == null) head = end = value;
            else
            {
                end.LinkNext = value;
                end = value;
            }
            queueLock = 0;
            if (System.Threading.Interlocked.CompareExchange(ref isThread, 1, 0) == 0)
            {
                try
                {
                    threadPool.FastStart((Action)run, Threading.Thread.CallType.Action);
                }
                catch (Exception error)
                {
                    AutoCSer.Log.Pub.Log.add(Log.LogType.Error, error);
                    run();
                }
            }
        }
        /// <summary>
        /// 链表任务处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void run()
        {
        START:
            DbConnection connection = null;
            do
            {
                AutoCSer.Threading.Interlocked.CompareExchangeYield(ref queueLock, AutoCSer.Threading.ThreadYield.Type.SqlLinkQueueTaskPop);
                LinkQueueTaskNode value = head;
                head = end = null;
                queueLock = 0;
                if (value == null)
                {
                    System.Threading.Thread.Sleep(0);
                    AutoCSer.Threading.Interlocked.CompareExchangeYield(ref queueLock, AutoCSer.Threading.ThreadYield.Type.SqlLinkQueueTaskPop);
                    value = head;
                    head = end = null;
                    queueLock = 0;
                    if (value == null)
                    {
                        client.FreeConnection(ref connection);
                        isThread = 0;
                        if (head != null && System.Threading.Interlocked.CompareExchange(ref isThread, 1, 0) == 0) goto START;
                        return;
                    }
                }
                do
                {
                    try
                    {
                        do
                        {
                            value = value.RunLinkQueueTask(ref connection);
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        client.CloseErrorConnection(ref connection);
                        AutoCSer.Log.Pub.Log.add(Log.LogType.Error, error);
                    }
                    LinkQueueTaskNode next = value.LinkNext;
                    value.LinkNext = null;
                    value = next;
                }
                while (value != null);
            }
            while (true);
        }
    }
}
