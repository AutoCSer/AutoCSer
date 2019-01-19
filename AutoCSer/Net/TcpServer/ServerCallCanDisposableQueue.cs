using System;
using System.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用队列处理
    /// </summary>
    internal sealed class ServerCallCanDisposableQueue : ServerCallQueue, IDisposable
    {
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile bool isDisposed;
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            waitHandle.Set();
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
                while (value != null)
                {
                    value = value.SingleRunTask();
                }
            }
            while (!isDisposed);
        }
    }
}
