using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 关键词自动回复
    /// </summary>
    public sealed class KeywordAutoReply
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time;
        /// <summary>
        /// 规则名称
        /// </summary>
        public string rule_name;
        /// <summary>
        /// 回复模式
        /// </summary>
        public AutoReplyMode reply_mode;
        /// <summary>
        /// 匹配的关键词列表
        /// </summary>
        public AutoReplyKeyword[] keyword_list_info;
        /// <summary>
        /// 自动回复列表
        /// </summary>
        public AutoReplyReply[] reply_list_info;
    }
}
