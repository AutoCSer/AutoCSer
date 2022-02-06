using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// 客户端心跳检测定时
    /// </summary>
    internal sealed class ClientCheckTimer : AutoCSer.Threading.SecondTimerTaskNode
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        private readonly Client client;
        /// <summary>
        /// 客户端心跳检测定时
        /// </summary>
        /// <param name="client"></param>
        /// <param name="seconds">超时秒数</param>
        internal ClientCheckTimer(Client client, int seconds)
            : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, seconds, Threading.SecondTimerThreadMode.Synchronous, Threading.SecondTimerKeepMode.After, seconds)
        {
            this.client = client;
        }
        /// <summary>
        /// 定时器触发
        /// </summary>
        protected internal override void OnTimer()
        {
            if (!client.Check()) KeepMode = AutoCSer.Threading.SecondTimerKeepMode.Canceled;
        }
    }
}
