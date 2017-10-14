using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文消息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ArticleMessage
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
        /// 图文消息被点击后跳转的链接
        /// </summary>
        public string url;
        /// <summary>
        /// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
        /// </summary>
        public string picurl;
    }
}
