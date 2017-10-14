using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 回复模式
    /// </summary>
    public enum AutoReplyMode : byte
    {
        /// <summary>
        /// 全部回复
        /// </summary>
        reply_all,
        /// <summary>
        /// 随机回复其中一条
        /// </summary>
        random_one
    }
}
