using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 消息
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public TextMessage text;
        /// <summary>
        /// 图片消息
        /// </summary>
        public MediaMessage image;
        /// <summary>
        /// 语音消息
        /// </summary>
        public MediaMessage voice;
    }
}
