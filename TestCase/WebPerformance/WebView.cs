using System;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// WEB 视图调用测试
    /// </summary>
    internal sealed partial class WebView : AutoCSer.WebView.View<WebView>
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public int Return;
        /// <summary>
        /// 负载均衡测试web视图调用
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private bool loadView(int left, int right)
        {
            Return = left + right;
            return true;
        }
    }
}
