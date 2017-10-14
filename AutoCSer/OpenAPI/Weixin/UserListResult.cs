using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 用户列表
    /// </summary>
    internal sealed class UserListResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 用户列表
        /// </summary>
        public User[] user_info_list;
#pragma warning restore
    }
}
