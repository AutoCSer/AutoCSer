using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// WEB 同步调用测试
    /// </summary>
    internal sealed class WebCall : AutoCSer.WebView.Call<WebCall>
    {
        /// <summary>
        /// 调用测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void Add(int left, int right)
        {
            Response(left + right);
        }
    }
}
