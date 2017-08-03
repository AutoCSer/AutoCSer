using System;

namespace AutoCSer.TestCase.WebPerformance.Ajax
{
    /// <summary>
    /// AJAX 调用测试
    /// </summary>
    internal sealed class Call : AutoCSer.WebView.Ajax<Call>
    {
        /// <summary>
        /// 调用测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false, IsOnlyPost = false)]
        public int Add(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 调用测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false, IsOnlyPost = false)]
        public void Xor(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> onReturn)
        {
            onReturn(left ^ right);
        }
    }
}
