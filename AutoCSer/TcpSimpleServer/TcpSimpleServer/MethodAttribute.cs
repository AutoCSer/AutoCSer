using System;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP 调用函数配置
    /// </summary>
    public class MethodAttribute : TcpServer.MethodBaseAttribute
    {
        /// <summary>
        /// 服务端任务类型，默认为 Synchronous
        /// </summary>
        public TcpServer.ServerTaskType ServerTask = TcpServer.ServerTaskType.Synchronous;
        /// <summary>
        /// 服务端任务类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override TcpServer.ServerTaskType ServerTaskType
        {
            get { return ServerTask; }
            set { ServerTask = value; }
        }
        /// <summary>
        /// 客户端异步任务类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override TcpServer.ClientTaskType ClientTaskType { get { return TcpServer.ClientTaskType.Synchronous; } }
        /// <summary>
        /// 是否生成同步调用代理函数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsClientSynchronous { get { return true; } }
        /// <summary>
        /// 是否生成异步调用代理函数。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsClientAsynchronous { get { return false; } }
        /// <summary>
        /// 保持异步回调
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsKeepCallback { get { return false; } }
        /// <summary>
        /// 客户端是否仅发送数据，无需服务端应答
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsClientSendOnly { get { return false; } }
    }
}
