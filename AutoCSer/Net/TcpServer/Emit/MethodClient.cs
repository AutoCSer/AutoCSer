using System;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 客户端
    /// </summary>
    public abstract class MethodClient : AutoCSer.Net.TcpServer.MethodClient
    {
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        protected volatile int _isDisposed_;
    }
    /// <summary>
    /// TCP 客户端
    /// </summary>
    /// <typeparam name="clientType">TCP 服务客户端类型</typeparam>
    public abstract class MethodClient<clientType> : MethodClient, IDisposable
        where clientType : CommandBase
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public clientType _TcpClient_ { get; internal set; }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (System.Threading.Interlocked.CompareExchange(ref _isDisposed_, 1, 0) == 0) _TcpClient_.Dispose();
        }
    }
}
