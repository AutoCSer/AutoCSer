using System;
using AutoCSer.Log;

namespace AutoCSer.Net.TcpStaticServer
{
    /// <summary>
    /// TCP 静态服务客户端
    /// </summary>
    public sealed class Client : TcpInternalServer.Client
    {
        /// <summary>
        /// 验证委托
        /// </summary>
        private readonly Func<TcpInternalServer.ClientSocketSender, bool> verifyMethod;
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        /// <param name="clientRoute">TCP 客户端路由</param>
        /// <param name="verifyMethod">验证委托</param>
        public Client(TcpInternalServer.ServerAttribute attribute, Action<SubArray<byte>> onCustomData, ILog log, AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> clientRoute = null, Func<TcpInternalServer.ClientSocketSender, bool> verifyMethod = null)
            : base(attribute, onCustomData, log, clientRoute)
        {
            this.verifyMethod = verifyMethod;
        }
        /// <summary>
        /// 套接字验证
        /// </summary>
        /// <param name="socket">TCP 调用客户端套接字</param>
        /// <returns></returns>
        internal override bool SocketVerifyMethod(TcpServer.ClientSocketSenderBase socket)
        {
            return verifyMethod == null || verifyMethod(new TcpInternalServer.UnionType { Value = socket }.ClientSocketSender);
        }
    }
}
