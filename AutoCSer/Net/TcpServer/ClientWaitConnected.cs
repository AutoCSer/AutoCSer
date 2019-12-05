using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 客户端等待连接，避免连接与验证时间长造成阻塞
    /// </summary>
    public sealed class ClientWaitConnected : IDisposable
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
        public readonly ManualResetEvent WaitEvent;
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        public readonly CheckSocketVersion CheckSocketVersion;
        /// <summary>
        /// 客户端等待连接
        /// </summary>
        /// <param name="client">TCP 客户端</param>
        /// <param name="onCheckSocketVersion">TCP 客户端套接字初始化处理</param>
        internal ClientWaitConnected(Client client, Action<ClientSocketEventParameter> onCheckSocketVersion)
        {
            this.client = client;
            WaitEvent = new ManualResetEvent(false);
            waitTimestamp = Stopwatch.GetTimestamp() + AutoCSer.Date.GetTimestampByMilliseconds((long)client.Attribute.GetClientWaitConnectedMilliseconds);
            CheckSocketVersion = client.CreateCheckSocketVersion(onCheckSocketVersion ?? this.OnCheckSocketVersion);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (WaitEvent != null) WaitEvent.Dispose();
        }
        /// <summary>
        /// TCP 客户端套接字初始化处理
        /// </summary>
        /// <param name="parameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OnCheckSocketVersion(ClientSocketEventParameter parameter)
        {
            if (parameter.Type == ClientSocketEventParameter.EventType.SetSocket) WaitEvent.Set();
        }
        /// <summary>
        /// 是否连接状态
        /// </summary>
        public bool IsConnected
        {
            get { return CheckSocketVersion.IsConnected; }
        }
        /// <summary>
        /// 等待连接初始化
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool WaitConnected()
        {
            return CheckSocketVersion.IsConnected || waitConnected();
        }
        /// <summary>
        /// 等待连接初始化
        /// </summary>
        /// <returns></returns>
        private bool waitConnected()
        {
            long timestamp = waitTimestamp - Stopwatch.GetTimestamp();
            if (timestamp > 0)
            {
                long ticks = AutoCSer.Date.GetTicksByTimestamp(timestamp);
                if (ticks > 0)
                {
                    WaitEvent.WaitOne(new TimeSpan(ticks));
                    return CheckSocketVersion.IsConnected;
                }
            }
            return false;
        }
    }
}
