using System;
using System.Threading;
using AutoCSer.Extensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 客户端回调队列处理
    /// </summary>
    internal sealed class ClientCallQueue : AutoCSer.Threading.TaskQueueThreadBase<ClientCommand.CommandBase>
    {
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="value"></param>
        internal void Add(ClientCommand.CommandBase value)
        {
            QueueLock.EnterYield();
            if (head == null)
            {
                end = value;
                head = value;
                QueueLock.Exit();
                WaitHandle.Set();
            }
            else
            {
                end.NextTask = value;
                end = value;
                QueueLock.Exit();
            }
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
                ClientCommand.CommandBase value = head;
                end = null;
                head = null;
                QueueLock.Exit();
                do
                {
                    try
                    {
                        do
                        {
                            value.OnReceiveTask(ref value);
                        }
                        while (value != null);
                        break;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                }
                while (value != null);
            }
            while (true);
        }

        /// <summary>
        /// TCP 客户端回调队列处理
        /// </summary>
        internal static readonly ClientCallQueue Default = new ClientCallQueue();
    }
}
