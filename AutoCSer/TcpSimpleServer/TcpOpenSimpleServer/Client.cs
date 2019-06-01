using System;
using AutoCSer.Log;

namespace AutoCSer.Net.TcpOpenSimpleServer
{
    /// <summary>
    /// TCP 开放服务客户端
    /// </summary>
    public abstract class Client : TcpSimpleServer.Client<ServerAttribute>
    {
#if !NOJIT
        /// <summary>
        /// TCP 开放服务客户端
        /// </summary>
        internal Client() : base() { }
#endif
        /// <summary>
        /// TCP 开放服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        public Client(ServerAttribute attribute, ILog log)
            : base(attribute, log, attribute.MaxInputSize)
        {
        }
    }
    /// <summary>
    /// TCP 开放服务客户端
    /// </summary>
    /// <typeparam name="clientType">客户端代理类型</typeparam>
    public sealed class Client<clientType> : Client
    {
        /// <summary>
        /// TCP 内部服务客户端代理对象
        /// </summary>
        private readonly clientType client;
        /// <summary>
        /// 验证委托
        /// </summary>
        private readonly Func<clientType, bool> verifyMethod;
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        /// <param name="client">TCP 服务客户端对象</param>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="log">日志接口</param>
        /// <param name="verifyMethod">验证委托</param>
        public Client(clientType client, ServerAttribute attribute, ILog log, Func<clientType, bool> verifyMethod = null)
            : base(attribute, log)
        {
            this.client = client;
            this.verifyMethod = verifyMethod;
        }
        /// <summary>
        /// 套接字验证
        /// </summary>
        /// <returns></returns>
        internal override bool CallVerifyMethod()
        {
            return verifyMethod == null || verifyMethod(client);
        }
    }
}
