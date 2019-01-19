using System;

namespace AutoCSer.Net.TcpStaticServer
{
    /// <summary>
    /// 保持异步回调 TCP 调用函数配置
    /// </summary>
    public class KeepCallbackMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// 默认为 false 表示不生成同步调用代理函数，同步模式使用的是 Monitor.Wait，会占用一个工作线程，并存在线程调度开销，优点是使用方便、安全。
        /// </summary>
        public new bool IsClientSynchronous = false;
        /// <summary>
        /// 是否生成同步调用代理函数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsClientSynchronous
        {
            get { return IsClientSynchronous; }
        }
        /// <summary>
        /// 默认为 true 表示生成异步调用代理函数。
        /// </summary>
        public new bool IsClientAsynchronous = true;
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
        public new bool IsKeepCallback = true;
        /// <summary>
        /// 保持异步回调
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsKeepCallback
        {
            get { return IsKeepCallback; }
        }
    }
}
