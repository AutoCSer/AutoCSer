using System;
using AutoCSer.Metadata;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务配置
    /// </summary>
    public abstract class ServerAttribute : ServerBaseAttribute
    {
        /// <summary>
        /// 远程表达式服务端任务类型，默认为 Timeout
        /// </summary>
        public ServerTaskType RemoteExpressionServerTask = ServerTaskType.Timeout;
        /// <summary>
        /// 远程表达式服务端任务类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override ServerTaskType GetRemoteExpressionServerTask { get { return RemoteExpressionServerTask; } }
    }
}
