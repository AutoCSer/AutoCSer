using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 自动回复
    /// </summary>
    public sealed class AutoReplyReply : AutoReplyContent
    {
        /// <summary>
        /// 图文消息的信息
        /// </summary>
        public AutoReplyNewsList news_info;
    }
}
