using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 定时触发 缓存 扩展
    /// </summary>
    internal static class OnTime
    {
        /// <summary>
        /// 设置定时触发类型
        /// </summary>
        /// <param name="flag"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Set(Date.NowTime.OnTimeFlag flag)
        {
            Date.NowTime.Flag |= flag;
        }
        /// <summary>
        /// 触发定时任务
        /// </summary>
        /// <param name="flag"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void onTime(Date.NowTime.OnTimeFlag flag)
        {
            if ((flag & Date.NowTime.OnTimeFlag.CacheFile) != 0)
            {
                for (FileStreamWriter writer = FileStreamWriter.Writers.End; writer != null; writer = writer.DoubleLinkPrevious) writer.OnTimer();
            }
            if ((flag & Date.NowTime.OnTimeFlag.CacheDistributionTimeout) != 0) Cache.MessageQueue.DistributionFileReader.OnTimer();
            //if (CacheTimeout.Timeouts.End != null)
            //{
            //    AutoCSer.Net.TcpServer.ServerCallQueue.Default.Add(new CacheManagerServerCall { Type = CacheManagerServerCallType.Timeout });
            //}
        }

        [AutoCSer.IOS.Preserve(Conditional = true)]
        static OnTime()
        {
            Date.NowTime.CacheOnTime = onTime;
        }
    }
}
