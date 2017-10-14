using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 自动回复的类型
    /// </summary>
    public enum AutoReplyType : byte
    {
        /// <summary>
        /// 文本
        /// </summary>
        text,
        /// <summary>
        /// 图片
        /// </summary>
        img,
        /// <summary>
        /// 语音
        /// </summary>
        voice,
        /// <summary>
        /// 视频
        /// </summary>
        video,
        /// <summary>
        /// 关键词自动回复图文消息
        /// </summary>
        news,
    }
}
