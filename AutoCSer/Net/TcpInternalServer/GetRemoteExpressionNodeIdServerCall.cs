using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// 获取远程表达式服务端节点标识同步调用
    /// </summary>
    internal sealed class GetRemoteExpressionNodeIdServerCall : AutoCSer.Net.TcpStaticServer.ServerCall<GetRemoteExpressionNodeIdServerCall, RemoteType[]>
    {
        /// <summary>
        /// 调用处理
        /// </summary>
        public override void Call()
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
                Sender.Push(CommandIndex, Sender.IsServerBuildOutputThread ? RemoteExpression.ServerNodeIdChecker.Output.OutputThreadInfo : RemoteExpression.ServerNodeIdChecker.Output.OutputInfo, ref value);
            }
            push(this);
        }
    }
}
