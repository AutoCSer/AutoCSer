using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Renren
{
    /// <summary>
    /// 访问令牌+用户身份的标识，用于保存
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct RefreshToken
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string access_token;
        /// <summary>
        /// 刷新访问令牌
        /// </summary>
        public string refresh_token;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int id;
    }
}
