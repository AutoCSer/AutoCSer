using System;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索
    /// </summary>
    internal static class Searcher
    {
        /// <summary>
        /// 搜索器
        /// </summary>
        internal static readonly AutoCSer.Search.StaticSearcher<DataKey> Default = new AutoCSer.Search.StaticSearcher<DataKey>();
    }
}
