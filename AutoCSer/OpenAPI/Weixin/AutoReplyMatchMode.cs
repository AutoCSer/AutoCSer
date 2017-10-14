using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 匹配模式
    /// </summary>
    public enum AutoReplyMatchMode : byte
    {
        /// <summary>
        /// 消息中含有该关键词即可
        /// </summary>
        contain,
        /// <summary>
        /// 消息内容必须和关键词严格相同
        /// </summary>
        equal
    }
}
