using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 自动回复规则
    /// </summary>
    public sealed class AutoReply : Return
    {
        /// <summary>
        /// 关注后自动回复是否开启，0代表未开启，1代表开启
        /// </summary>
        public byte is_add_friend_reply_open;
        /// <summary>
        /// 消息自动回复是否开启，0代表未开启，1代表开启
        /// </summary>
        public byte is_autoreply_open;
        /// <summary>
        /// 关注后自动回复的信息
        /// </summary>
        public AutoReplyContent add_friend_autoreply_info;
        /// <summary>
        /// 消息自动回复的信息
        /// </summary>
        public AutoReplyContent message_default_autoreply_info;
        /// <summary>
        /// 关键词自动回复的信息
        /// </summary>
        public KeywordAutoReplyList keyword_autoreply_info;
    }
}
