using System;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 带参数初始化 示例
    /// </summary>
    [AutoCSer.WebView.View(IsPage = false)]
    partial class LoadViewQuery : AutoCSer.WebView.View<LoadViewQuery>
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
            return true;
        }
    }
}
