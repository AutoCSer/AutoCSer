using System;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 首页重定向
    /// </summary>
    class Index : AutoCSer.WebView.Call<Index>
    {
        /// <summary>
        /// 首页重定向
        /// </summary>
        [AutoCSer.WebView.CallMethod(FullName = "")]
        public void Home()
        {
            location("/Index.html");
        }
    }
}
