using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 客服账号
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct AccountListItem
    {
        /// <summary>
        /// 完整客服账号，格式为：账号前缀@公众号微信号
        /// </summary>
        public string kf_account;
        /// <summary>
        /// 客服昵称
        /// </summary>
        public string kf_nick;
        /// <summary>
        /// 客服工号
        /// </summary>
        public string kf_id;
        /// <summary>
        /// 客服头像
        /// </summary>
        public string kf_headimgurl;
    }
}
