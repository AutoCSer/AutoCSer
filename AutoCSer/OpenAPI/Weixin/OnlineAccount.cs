using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 在线客服接待信息
    /// </summary>
    public sealed class OnlineAccount
    {
        /// <summary>
        /// 完整客服账号，格式为：账号前缀@公众号微信号
        /// </summary>
        public string kf_account;
        /// <summary>
        /// 客服工号
        /// </summary>
        public string kf_id;
        /// <summary>
        /// 客服设置的最大自动接入数
        /// </summary>
        public int auto_accept;
        /// <summary>
        /// 客服当前正在接待的会话数
        /// </summary>
        public int accepted_case;
        /// <summary>
        /// 客服在线状态 1：pc在线，2：手机在线。若pc和手机同时在线则为 1+2=3
        /// </summary>
        public byte status;
    }
}
