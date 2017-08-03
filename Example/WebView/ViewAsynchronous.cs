using System;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 异步输出 示例
    /// </summary>
    [AutoCSer.WebView.View(IsPage = false, IsAsynchronous = true)]
    partial class ViewAsynchronous : AutoCSer.WebView.View<ViewAsynchronous>
    {
        /// <summary>
        /// 输出数据
        /// </summary>
        public int Sum;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>是否成功</returns>
        private bool loadView(int left, int right)
        {
            Sum = left + right;
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(AsynchronousResponse);
            return true;
        }
    }
}
