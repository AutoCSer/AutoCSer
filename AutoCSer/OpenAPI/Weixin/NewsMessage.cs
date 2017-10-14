using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文消息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct NewsMessage
    {
        /// <summary>
        /// 图文消息
        /// </summary>
        public ArticleMessage[] articles;
    }
}
