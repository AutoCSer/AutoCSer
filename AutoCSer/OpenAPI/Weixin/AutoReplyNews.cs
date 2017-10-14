using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public sealed class AutoReplyNews
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 作者
        /// </summary>
        public string author;
        /// <summary>
        /// 摘要
        /// </summary>
        public string digest;
        /// <summary>
        /// 封面图片的URL
        /// </summary>
        public string cover_url;
        /// <summary>
        /// 正文的URL
        /// </summary>
        public string content_url;
        /// <summary>
        /// 原文的URL，若置空则无查看原文入口
        /// </summary>
        public string source_url;
        /// <summary>
        /// 是否显示封面，0为不显示，1为显示
        /// </summary>
        public byte show_cover;
    }
}
