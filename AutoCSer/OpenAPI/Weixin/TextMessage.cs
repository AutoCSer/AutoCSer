using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 文本消息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct TextMessage
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string content;
    }
}
