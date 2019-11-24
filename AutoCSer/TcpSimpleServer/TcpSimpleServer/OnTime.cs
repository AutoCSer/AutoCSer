using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// 定时触发简单 TCP 服务扩展
    /// </summary>
    internal sealed class OnTime : Date.NowTime.OnTime
    {
        /// <summary>
        /// 触发定时任务
        /// </summary>
        internal override void OnTimer()
        {
            for (AutoCSer.Net.TcpSimpleServer.ClientCheckTimer timeout = AutoCSer.Net.TcpSimpleServer.ClientCheckTimer.TimeoutEnd; timeout != null; timeout = timeout.DoubleLinkPrevious) timeout.OnTimer();
        }
        /// <summary>
        /// 设置定时触发器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set()
        {
            if (Date.NowTime.TcpSimpleServerOnTime == null)
            {
                Date.NowTime.TcpSimpleServerOnTime = this;
                Date.NowTime.IsOnTime = true;
            }
        }
        /// <summary>
        /// 定时触发简单 TCP 服务扩展
        /// </summary>
        internal static readonly OnTime Default = new OnTime();
    }
}
