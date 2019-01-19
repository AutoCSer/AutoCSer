using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// 获取远程表达式服务端节点标识同步调用
    /// </summary>
    internal sealed class GetRemoteExpressionServerCall : AutoCSer.Net.TcpStaticServer.ServerCall<GetRemoteExpressionServerCall, RemoteExpression.ClientNode>
    {
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
            push(this);
        }
    }
}
