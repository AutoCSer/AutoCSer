using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 获取所有客服账号
    /// </summary>
    public sealed class AccountList : Return
    {
        /// <summary>
        /// 获取所有客服账号
        /// </summary>
        public AccountListItem[] kf_list;
    }
}
