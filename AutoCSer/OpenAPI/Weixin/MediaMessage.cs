using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图片消息/语音消息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct MediaMessage
    {
        /// <summary>
        /// 发送的图片/语音/视频的媒体ID
        /// </summary>
        public string media_id;
    }
}
