using System;
using AutoCSer.Metadata;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 调用函数配置
    /// </summary>
    public partial class MethodAttribute : MethodBaseAttribute
    {
        /// <summary>
        /// 服务端任务类型，默认为 Timeout
        /// </summary>
        public ServerTaskType ServerTask = ServerTaskType.Timeout;
        /// <summary>
        /// 服务端任务类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override ServerTaskType ServerTaskType
        {
            get { return ServerTask; }
            set { ServerTask = value; }
        }
        /// <summary>
        /// 客户端异步任务类型，默认为 Timeout
        /// </summary>
        public ClientTaskType ClientTask = ClientTaskType.Timeout;
        /// <summary>
        /// 客户端异步任务类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override ClientTaskType ClientTaskType { get { return ClientTask; } }
        /// <summary>
        /// 默认为 true 表示生成同步调用代理函数，同步模式使用的是 Monitor.Wait，会占用一个工作线程，并存在线程调度开销，优点是使用方便、安全。
        /// </summary>
        public bool IsClientSynchronous = true;
        /// <summary>
        /// 是否生成同步调用代理函数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsClientSynchronous
        {
            get { return IsClientSynchronous; }
        }
        /// <summary>
        /// 默认为 false 表示不生成异步调用代理函数。
        /// </summary>
        public bool IsClientAsynchronous;
        /// <summary>
        /// 是否生成异步调用代理函数。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsClientAsynchronous
        {
            get { return IsClientAsynchronous; }
        }
        /// <summary>
        /// 保持异步回调，1 问多答的交互模式（客户端一个请求，服务器端可以任意多次回调回应）。
        /// </summary>
        public bool IsKeepCallback;
        /// <summary>
        /// 保持异步回调
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsKeepCallback
        {
            get { return IsKeepCallback; }
        }
        /// <summary>
        /// 默认为 false 表示服务端需要应答客户端请求，否则仅仅是客户端发送数据到服务端（服务端不应答）。
        /// </summary>
        public bool IsClientSendOnly;
        /// <summary>
        /// 客户端是否仅发送数据，无需服务端应答
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsClientSendOnly { get { return IsClientSendOnly; } }
        /// <summary>
        /// 默认为 true 表示生成 await 代理，不支持 ref / out
        /// </summary>
        public bool IsClientAwaiter = true;
        /// <summary>
        /// 是否支持 async Task
        /// </summary>
        internal override bool GetIsClientAwaiter { get { return IsClientAwaiter; } }
        /// <summary>
        /// 默认为 true 表示服务端异步回调创建输出需要开启线程任务
        /// </summary>
        public bool IsServerAsynchronousCallbackBuildOutputThread = true;
    }
}
