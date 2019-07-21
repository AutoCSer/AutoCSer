using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 公用 AJAX 调用
    /// </summary>
    internal sealed class PubAjax : Ajax<PubAjax>
    {
        /// <summary>
        /// 公用 AJAX 调用
        /// </summary>
        /// <returns></returns>
        internal bool Call()
        {
            PubErrorParameter parameter = new PubErrorParameter();
            if (ParseParameter(ref parameter))
            {
                Error(parameter.error);
                Response();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 公用错误处理函数
        /// </summary>
        /// <param name="error">错误信息</param>
        public void Error(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                bool isLog = false;
                HashString errorHash = error;
                Monitor.Enter(errorQueueLock);
                try
                {
                    if (errorQueue.Set(ref errorHash, error) == null)
                    {
                        isLog = true;
                        if (errorQueue.Count > 512) errorQueue.UnsafePopNode();
                    }
                }
                finally { Monitor.Exit(errorQueueLock); }
                if (isLog) DomainServer.WebClientLog.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, error);
            }
        }

        /// <summary>
        /// 错误信息队列
        /// </summary>
        private static FifoPriorityQueue<HashString, string> errorQueue = new FifoPriorityQueue<HashString, string>();
        /// <summary>
        /// 错误信息队列访问锁
        /// </summary>
        private static readonly object errorQueueLock = new object();
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            if (errorQueue.Count != 0) errorQueue = new FifoPriorityQueue<HashString, string>();
        }

        static PubAjax()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
