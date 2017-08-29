using System;
using AutoCSer.Log;

namespace AutoCSer.Net.TcpStaticSimpleServer
{
    /// <summary>
    /// TCP 静态服务客户端
    /// </summary>
    public sealed class Client : TcpInternalSimpleServer.Client
    {
        /// <summary>
        /// 验证委托
        /// </summary>
        private readonly Func<bool> verifyMethod;
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        /// <param name="verifyMethod">验证委托</param>
        public Client(TcpInternalSimpleServer.ServerAttribute attribute, ILog log, Func<bool> verifyMethod)
            : base(attribute, log)
        {
            this.verifyMethod = verifyMethod;
        }
        /// <summary>
        /// 套接字验证
        /// </summary>
        /// <returns></returns>
        internal override bool CallVerifyMethod()
        {
            return verifyMethod == null || verifyMethod();
        }
    }
}
