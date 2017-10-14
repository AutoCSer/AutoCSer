using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 视频消息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct VideoMessage
    {
        /// <summary>
        /// 发送的图片/语音/视频的媒体ID
        /// </summary>
        public string media_id;
        /// <summary>
        /// 缩略图的媒体ID
        /// </summary>
        public string thumb_media_id;
        /// <summary>
        /// 消息的标题
        /// </summary>
        public string title;
        /// <summary>
        /// 消息的描述
        /// </summary>
        public string description;
    }
}
