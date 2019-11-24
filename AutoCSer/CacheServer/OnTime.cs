using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 定时触发 缓存 扩展
    /// </summary>
    internal sealed class OnTime : Date.NowTime.OnTime
    {
        /// <summary>
        /// 触发定时任务
        /// </summary>
        internal override void OnTimer()
        {
            for (FileStreamWriter writer = FileStreamWriter.Writers.End; writer != null; writer = writer.DoubleLinkPrevious) writer.OnTimer();
            Cache.MessageQueue.DistributionFileReader.OnTimer();
            //if (CacheTimeout.Timeouts.End != null)
            //{
            //    AutoCSer.Net.TcpServer.ServerCallQueue.Default.Add(new CacheManagerServerCall { Type = CacheManagerServerCallType.Timeout });
            //}
        }
        /// <summary>
        /// 设置定时触发器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set()
        {
            if (Date.NowTime.CacheServerOnTime == null)
            {
                Date.NowTime.CacheServerOnTime = this;
                Date.NowTime.IsOnTime = true;
            }
        }
        /// <summary>
        /// 定时触发 WEB 扩展
        /// </summary>
        internal static readonly OnTime Default = new OnTime();
    }
}
