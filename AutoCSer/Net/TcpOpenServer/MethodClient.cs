using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 客户端
    /// </summary>
    /// <typeparam name="clientType">客户端类型</typeparam>
    public abstract class MethodClient<clientType> : AutoCSer.Net.TcpServer.MethodClient, IDisposable
        where clientType : MethodClient<clientType>
    {
        /// <summary>
        /// TCP 开放服务客户端
        /// </summary>
        public Client<clientType> _TcpClient_ { get; protected set; }
        /// <summary>
        /// 客户端等待连接
        /// </summary>
        public TcpServer.ClientWaitConnected _WaitConnected_ { get; protected set; }
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private int isDisposed;
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (System.Threading.Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0) _TcpClient_.Dispose();
        }
    }
}
