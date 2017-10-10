using System;

namespace AutoCSer.Web
{
    /// <summary>
    /// 关键字搜索
    /// </summary>
    internal partial class Search : AutoCSer.WebView.View<Search>
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        [AutoCSer.WebView.ViewQuery]
        string Key;
        /// <summary>
        /// 搜索结果
        /// </summary>
        AutoCSer.Web.SearchServer.SearchItem[] Items
        {
            get
            {
                return AutoCSer.Web.SearchServer.TcpCall.Server.Search(Key);
            }
        }
    }
}
