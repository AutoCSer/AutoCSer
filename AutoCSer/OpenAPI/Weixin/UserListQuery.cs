using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 用户列表
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct UserListQuery
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public UserLanguage[] user_list;
    }
}
