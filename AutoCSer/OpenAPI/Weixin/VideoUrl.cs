using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 视频消息素材
    /// </summary>
    public sealed class VideoUrl : Return
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 下载地址
        /// </summary>
        public string down_url;
    }
}
