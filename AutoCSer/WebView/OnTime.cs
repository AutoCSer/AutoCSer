using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 定时触发 WEB 扩展
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
            if ((flag & Date.NowTime.OnTimeFlag.HttpSession) != 0)
            {
                for (AutoCSer.Net.HttpDomainServer.Session session = AutoCSer.Net.HttpDomainServer.Session.SessionEnd; session != null; session = session.DoubleLinkPrevious) session.OnTimer();
            }
            if ((flag & Date.NowTime.OnTimeFlag.CreateFlieTimeoutWatcher) != 0)
            {
                for (AutoCSer.IO.CreateFlieTimeoutWatcher watcher = AutoCSer.IO.CreateFlieTimeoutWatcher.Watchers.End; watcher != null; watcher = watcher.DoubleLinkPrevious) watcher.OnTimer();
            }
        }

        [AutoCSer.IOS.Preserve(Conditional = true)]
        static OnTime()
        {
            Date.NowTime.WebViewOnTime = onTime;
        }
    }
}
