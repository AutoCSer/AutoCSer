using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 微博ID
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct MicroblogId
    {
        /// <summary>
        /// 微博消息的ID，用来唯一标识一条微博消息
        /// </summary>
        public string id;
        /// <summary>
        /// 微博消息的发表时间
        /// </summary>
        public long time;
    }
}
