using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weibo
{
    /// <summary>
    /// 访问令牌+用户身份的标识，用于保存
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct TokenUid
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string Token;
        /// <summary>
        /// 用户身份的标识
        /// </summary>
        public string Uid;
    }
}
