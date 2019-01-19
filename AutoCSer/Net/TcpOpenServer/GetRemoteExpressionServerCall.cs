using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// 获取远程表达式服务端节点标识同步调用
    /// </summary>
    internal sealed class GetRemoteExpressionServerCall : TcpServer.ServerCall
    {
        /// <summary>
        /// 套接字
        /// </summary>
        internal ServerSocketSender Sender;
        /// <summary>
        /// 输入参数
        /// </summary>
        private RemoteExpression.ClientNode inputParameter;
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sender">套接字</param>
        /// <param name="taskType"></param>
        /// <param name="inputParameter">输入参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ServerSocketSender sender, TcpServer.ServerTaskType taskType, ref RemoteExpression.ClientNode inputParameter)
        {
            this.Sender = sender;
            CommandIndex = sender.ServerSocket.CommandIndex;
            this.inputParameter = inputParameter;
            switch (taskType)
            {
                case TcpServer.ServerTaskType.ThreadPool: if (!System.Threading.ThreadPool.QueueUserWorkItem(threadPoolCall)) AutoCSer.Threading.LinkTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.Timeout: AutoCSer.Threading.LinkTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.TcpTask: TcpServer.ServerCallTask.Task.Add(this); return;
                case TcpServer.ServerTaskType.TcpQueue: TcpServer.ServerCallQueue.Default.Add(this); return;
                case TcpServer.ServerTaskType.Queue: sender.Server.CallQueue.Add(this); return;
            }
        }
        /// <summary>
        /// 调用处理
        /// </summary>
        public override void Call()
        {
            if (Sender.IsSocket)
            {
                AutoCSer.Net.TcpServer.ReturnValue<RemoteExpression.ReturnValue.Output> value = new AutoCSer.Net.TcpServer.ReturnValue<RemoteExpression.ReturnValue.Output>();
                try
                {
                    value.Value.Return = inputParameter.GetReturnValue();
                    value.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
                }
                catch (Exception error)
                {
                    value.Type = AutoCSer.Net.TcpServer.ReturnType.ServerException;
                    Sender.AddLog(error);
                }
                Sender.Push(CommandIndex, Sender.IsServerBuildOutputThread ? RemoteExpression.ReturnValue.Output.OutputThreadInfo : RemoteExpression.ReturnValue.Output.OutputInfo, ref value);
            }
            Sender = null;
            inputParameter.SetNull();
            AutoCSer.Threading.RingPool<GetRemoteExpressionServerCall>.Default.PushNotNull(this);
        }
    }
}
