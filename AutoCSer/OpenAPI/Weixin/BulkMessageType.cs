using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum BulkMessageType : byte
    {
        /// <summary>
        /// 图文消息
        /// </summary>
        mpnews,
        /// <summary>
        /// 文本
        /// </summary>
        text,
        /// <summary>
        /// 语音（注意此处media_id需通过基础支持中的上传下载多媒体文件来得到）
        /// </summary>
        voice,
        /// <summary>
        /// 图片（注意此处media_id需通过基础支持中的上传下载多媒体文件来得到）
        /// </summary>
        image,
        /// <summary>
        /// 视频
        /// </summary>
        mpvideo,
        /// <summary>
        /// 卡券消息
        /// </summary>
        wxcard
    }
}
