using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文消息素材
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct NewsContent
    {
        /// <summary>
        /// 图文消息素材
        /// </summary>
        public ArticleUrl[] news_item;
    }
}
