using AutoCSer.Extensions;
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
        protected readonly AutoCSer.ILog log;
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
        internal ServerCallCanDisposableQueue(bool isBackground, bool isStart, AutoCSer.ILog log = null) : base(isBackground, isStart)
        {
            this.log = log ?? AutoCSer.LogHelper.Default;
        }
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        /// <param name="isBackground">是否后台线程</param>
        /// <param name="log">日志接口</param>
        public ServerCallCanDisposableQueue(bool isBackground = true, AutoCSer.ILog log = null) : this(isBackground, true, log) { }
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
                QueueLock.EnterYield();
                ServerCallBase value = head;
                end = null;
                head = null;
                QueueLock.Exit();
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            value.RunTask(ref value);
                        }
                        break;
                    }
                    catch (Exception error)
                    {
                        log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                }
                while (true);
            }
            while (!isDisposed);
        }
    }
}
