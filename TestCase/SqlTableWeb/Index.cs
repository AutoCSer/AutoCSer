using System;

namespace AutoCSer.TestCase.SqlTableWeb
{
    /// <summary>
    /// 公共基本调用
    /// </summary>
    [AutoCSer.WebView.Call]
    class Index : AutoCSer.WebView.Call<Index>
    {
        /// <summary>
        /// 首页重定向
        /// </summary>
        [AutoCSer.WebView.CallMethod(FullName = "")]
        public void Load()
        {
            location(new SqlModel.WebPath.Pub().ClassList);
        }
    }
}
