using System;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// 服务端输出信息
    /// </summary>
    public sealed class OutputInfo
    {
        /// <summary>
        /// 输出参数编号
        /// </summary>
        public int OutputParameterIndex;
        /// <summary>
        /// 是否简单序列化输出参数
        /// </summary>
        public bool IsSimpleSerializeOutputParamter;
        /// <summary>
        /// 远程表达式服务端节点标识解析输出参数信息
        /// </summary>
        internal static readonly OutputInfo RemoteExpressionNodeId = new OutputInfo { OutputParameterIndex = -TcpServer.Server.RemoteExpressionNodeIdCommandIndex };
        /// <summary>
        /// 远程表达式输出参数信息
        /// </summary>
        internal static readonly OutputInfo RemoteExpression = new OutputInfo { OutputParameterIndex = -TcpServer.Server.RemoteExpressionCommandIndex };
    }
}
