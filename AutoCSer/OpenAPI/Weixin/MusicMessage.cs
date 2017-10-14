using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 音乐消息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct MusicMessage
    {
        /// <summary>
        /// 消息的标题
        /// </summary>
        public string title;
        /// <summary>
        /// 消息的描述
        /// </summary>
        public string description;
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string musicurl;
        /// <summary>
        /// 高品质音乐链接，wifi环境优先使用该链接播放音乐
        /// </summary>
        public string hqmusicurl;
        /// <summary>
        /// 缩略图的媒体ID
        /// </summary>
        public string thumb_media_id;
    }
}
