using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文消息素材
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 缩略图的media_id，可以在基础支持-上传多媒体文件接口中获得
        /// </summary>
        public string thumb_media_id;
        /// <summary>
        /// 作者
        /// </summary>
        public string author;
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 在图文消息页面点击“阅读原文”后的页面
        /// </summary>
        public string content_source_url;
        /// <summary>
        /// 图文消息页面的内容，支持HTML标签，将过滤外部的图片链接
        /// </summary>
        public string content;
        /// <summary>
        /// 图文消息的描述
        /// </summary>
        public string digest;
        /// <summary>
        /// 是否显示封面，1为显示，0为不显示
        /// </summary>
        public string show_cover_pic = "0";
    }
}
