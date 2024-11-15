﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// 获取远程表达式服务端节点标识同步调用
    /// </summary>
    internal sealed class GetRemoteExpressionNodeIdServerCall : TcpServer.ServerCall
    {
        /// <summary>
        /// 套接字
        /// </summary>
        internal ServerSocketSender Sender;
        /// <summary>
        /// 输入参数
        /// </summary>
        private AutoCSer.Reflection.RemoteType[] inputParameter;
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sender">套接字</param>
        /// <param name="attribute"></param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ServerSocketSender sender, ref TcpServer.ServerAttributeCache attribute, AutoCSer.Reflection.RemoteType[] inputParameter)
        {
            this.Sender = sender;
            CommandIndex = sender.ServerSocket.CommandIndex;
            this.inputParameter = inputParameter;
            switch (attribute.RemoteExpressionTask)
            {
                case TcpServer.ServerTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(ThreadPoolCall)) AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.Timeout: AutoCSer.Threading.TaskSwitchThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.TcpTask: TcpServer.ServerCallThreadArray.Default.CurrentThread.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueue: TcpServer.ServerCallQueue.Default.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueueLink: TcpServer.ServerCallQueue.DefaultLink.Add(this); return;
                case TcpServer.ServerTaskType.Queue:
                    if (attribute.RemoteExpressionCallQueueIndex == 0) sender.Server.CallQueue.Add(this);
                    else sender.Server.CallQueueArray[attribute.RemoteExpressionCallQueueIndex].Key.Add(this);
                    return;
                case TcpServer.ServerTaskType.QueueLink:
                    if (attribute.RemoteExpressionCallQueueIndex == 0) sender.Server.CallQueueLink.Add(this);
                    else sender.Server.CallQueueArray[attribute.RemoteExpressionCallQueueIndex].Value.Add(this);
                    return;
            }
        }
        /// <summary>
        /// 调用处理
        /// </summary>
        public override void RunTask()
        {
            if (Sender.IsSocket)
            {
                AutoCSer.Net.TcpServer.ReturnValue<RemoteExpression.ServerNodeIdChecker.Output> value = new AutoCSer.Net.TcpServer.ReturnValue<RemoteExpression.ServerNodeIdChecker.Output>();
                try
                {
                    value.Value.Return = RemoteExpression.Node.Get(inputParameter);
                    value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                }
                catch (Exception error)
                {
                    value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                    Sender.AddLog(error);
                }
                Sender.Push(CommandIndex, Sender.IsBuildOutputThread ? RemoteExpression.ServerNodeIdChecker.Output.OutputThreadInfo : RemoteExpression.ServerNodeIdChecker.Output.OutputInfo, ref value);
            }
            Sender = null;
            inputParameter = null;
            AutoCSer.Threading.RingPool<GetRemoteExpressionNodeIdServerCall>.Default.PushNotNull(this);
        }
    }
}
