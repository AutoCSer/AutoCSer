using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文群发总数据
    /// </summary>
    internal sealed class ArticleTotalResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 图文群发总数据
        /// </summary>
        public ArticleTotal[] list;
#pragma warning restore
    }
}
