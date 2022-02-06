using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 定时触发 WEB 扩展
    /// </summary>
    //internal sealed class OnTime : Date.NowTime.OnTime
    {
        /// <summary>
        /// 触发定时任务
        /// </summary>
        internal override void OnTimer()
        {
            for (AutoCSer.IO.CreateFlieTimeoutWatcher watcher = AutoCSer.IO.CreateFlieTimeoutWatcher.Watchers.End; watcher != null; watcher = watcher.DoubleLinkPrevious) watcher.OnTimer();
        }
        /// <summary>
        /// 设置定时触发器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set()
        {
            if (Date.NowTime.WebViewOnTime == null)
            {
                Date.NowTime.WebViewOnTime = this;
                Date.NowTime.IsOnTime = true;
            }
        }
        /// <summary>
        /// 定时触发 WEB 扩展
        /// </summary>
        internal static readonly OnTime Default = new OnTime();
    }
}
