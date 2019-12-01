using AutoCSer.Extension;
using System;
using System.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用队列处理
    /// </summary>
    public class ServerCallCanDisposableQueue : ServerCallQueue, IDisposable
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly AutoCSer.Log.ILog log;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        protected volatile bool isDisposed;
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="isStart">是否启动线程</param>
        /// <param name="log">日志接口</param>
        internal ServerCallCanDisposableQueue(bool isBackground, bool isStart, AutoCSer.Log.ILog log = null) : base(isBackground, isStart)
        {
            this.log = log ?? AutoCSer.Log.Pub.Log;
        }
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="log">日志接口</param>
        public ServerCallCanDisposableQueue(bool isBackground = true, AutoCSer.Log.ILog log = null) : this(isBackground, true, log) { }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            WaitHandle.Set();
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
                        while (value != null)
                        {
                            value.SingleRunTask(ref value);
                        }
                        break;
                    }
                    catch (Exception error)
                    {
                        log.Add(Log.LogType.Error, error);
                    }
                }
                while (true);
            }
            while (!isDisposed);
        }
    }
}
