using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 素材的类型
    /// </summary>
    public enum MediaQueryType : byte
    {
        /// <summary>
        /// 图文
        /// </summary>
        news,
        /// <summary>
        /// 图片
        /// </summary>
        image,
        /// <summary>
        /// 视频
        /// </summary>
        video,
        /// <summary>
        /// 语音
        /// </summary>
        voice
    }
}
