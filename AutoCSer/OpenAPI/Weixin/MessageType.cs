using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType : byte
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        text,
        /// <summary>
        /// 图片消息
        /// </summary>
        image,
        /// <summary>
        /// 语音消息
        /// </summary>
        voice,
        /// <summary>
        /// 视频消息
        /// </summary>
        video,
        /// <summary>
        /// 音乐消息
        /// </summary>
        music,
        /// <summary>
        /// 图文消息 图文消息条数限制在10条以内，注意，如果图文数超过10，则将会无响应
        /// </summary>
        news,
        /// <summary>
        /// 卡券
        /// </summary>
        wxcard,
    }
}
