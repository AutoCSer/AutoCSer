using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// WEB 异步调用测试
    /// </summary>
    internal sealed class WebCallAsynchronous : AutoCSer.WebView.CallAsynchronous<WebCallAsynchronous>
    {
        /// <summary>
        /// 调用测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void Xor(int left, int right)
        {
            RepsonseEnd((left ^ right).toString(), true);
        }
    }
}
