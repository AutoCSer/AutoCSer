using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Example.HtmlTitle
{
    /// <summary>
    /// URL 抓取标题回调
    /// </summary>
    sealed class UrlTitle
    {
        /// <summary>
        /// 当前未完成的抓取数量
        /// </summary>
        private static int currentCount;
        /// <summary>
        /// URL
        /// </summary>
        private string url;
        /// <summary>
        /// 抓取标题回调
        /// </summary>
        /// <param name="title">标题</param>
        private void onTitle(string title)
        {
            Console.WriteLine("[" + Interlocked.Decrement(ref currentCount).toString() + "] " + url + " => " + (title ?? "null"));
        }
        /// <summary>
        /// 根据URL抓取标题
        /// </summary>
        /// <param name="task">HTML标题获取客户端任务池</param>
        /// <param name="url">URL</param>
        internal static void Crawl(AutoCSer.Net.HtmlTitle.HttpTask task, string url)
        {
            UrlTitle urlTitle = new UrlTitle { url = url };
            Interlocked.Increment(ref currentCount);
            task.Get(url, urlTitle.onTitle);
        }
    }
}
