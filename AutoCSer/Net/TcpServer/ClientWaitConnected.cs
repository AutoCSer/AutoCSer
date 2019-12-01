using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 客户端等待连接，避免连接与验证时间长造成阻塞
    /// </summary>
    public sealed class ClientWaitConnected
    {
        /// <summary>
        /// TCP 客户端
        /// </summary>
        private readonly Client client;
        /// <summary>
        /// 等待时间戳
        /// </summary>
        private readonly long waitTimestamp;
        /// <summary>
        /// 等待事件
        /// </summary>
        private readonly ManualResetEvent waitEvent;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        private readonly CheckSocketVersion checkSocketVersion;
        /// <summary>
        /// 客户端等待连接
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        /// <param name="waitMilliseconds">等待毫秒数</param>
        /// <param name="onCheckSocketVersion">TCP 客户端套接字初始化处理</param>
        internal ClientWaitConnected(Client client, uint waitMilliseconds, Action<ClientSocketEventParameter> onCheckSocketVersion)
        {
            this.client = client;
            waitEvent = new ManualResetEvent(false);
            waitTimestamp = Stopwatch.GetTimestamp() + AutoCSer.Date.GetTimestampByMilliseconds((long)waitMilliseconds);
            checkSocketVersion = client.CreateCheckSocketVersion(onCheckSocketVersion ?? this.onCheckSocketVersion);
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        private void onCheckSocketVersion(ClientSocketEventParameter parameter)
        {
            if (parameter.Type == ClientSocketEventParameter.EventType.SetSocket) waitEvent.Set();
        }
        /// <summary>
        /// 等待连接初始化
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool WaitConnected()
        {
            return checkSocketVersion.IsConnected || waitConnected();
        }
        /// <summary>
        /// 等待连接初始化
        /// </summary>
        /// <returns></returns>
        private bool waitConnected()
        {
            long timestamp = Stopwatch.GetTimestamp() - waitTimestamp;
            if (timestamp > 0)
            {
                waitEvent.WaitOne(new TimeSpan(AutoCSer.Date.GetTicksByTimestamp(timestamp)));
                return checkSocketVersion.IsConnected;
            }
            return false;
        }
    }
}
