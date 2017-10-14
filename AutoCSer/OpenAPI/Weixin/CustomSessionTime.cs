using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 客户当前的会话状态
    /// </summary>
    public sealed class CustomSessionTime : Return
    {
        /// <summary>
        /// 正在接待的客服，为空表示没有人在接待
        /// </summary>
        public string kf_account;
        /// <summary>
        /// 会话接入的时间
        /// </summary>
        public long createtime;
    }
}
