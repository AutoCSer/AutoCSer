using System;
using AutoCSer.Extensions;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Threading
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
        private AutoCSer.Threading.SpinLock queueLock;
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
            queueLock.EnterYield();
            if (head == null) head = end = value;
            else
            {
                end.LinkNext = value;
                end = value;
            }
            queueLock.Exit();
            if (System.Threading.Interlocked.CompareExchange(ref isThread, 1, 0) == 0)
            {
                try
                {
                    threadPool.FastStart(run);
                }
                catch (Exception error)
                {
                    AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
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
                queueLock.EnterYield();
                LinkQueueTaskNode value = head;
                head = end = null;
                queueLock.Exit();
                if (value == null)
                {
                    System.Threading.Thread.Sleep(0);
                    queueLock.EnterYield();
                    value = head;
                    head = end = null;
                    queueLock.Exit();
                    if (value == null)
                    {
                        client.FreeConnection(ref connection);
                        //isThread = 0;
                        System.Threading.Interlocked.Exchange(ref isThread, 0);
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
                            value.RunLinkQueueTask(ref connection, ref value);
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        client.ResetErrorConnection(ref connection);
                        AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                }
                while (value != null);
            }
            while (true);
        }
    }
}
