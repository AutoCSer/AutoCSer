using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文消息素材列表
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct NewsItem
    {
        /// <summary>
        /// 这篇图文消息素材的最后更新时间
        /// </summary>
        public long update_time;
        /// <summary>
        /// 
        /// </summary>
        public string media_id;
        /// <summary>
        /// 图文消息素材
        /// </summary>
        public NewsContent content;
    }
}
