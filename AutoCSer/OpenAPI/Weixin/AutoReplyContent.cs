using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 自动回复的信息
    /// </summary>
    public class AutoReplyContent
    {
        /// <summary>
        /// 自动回复的类型
        /// </summary>
        public AutoReplyType type;
        /// <summary>
        /// 对于文本类型，content是文本内容，对于图文、图片、语音、视频类型，content是mediaID
        /// </summary>
        public string content;
    }
}
