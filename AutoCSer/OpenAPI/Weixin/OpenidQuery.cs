using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 用户的OpenID
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct OpenidQuery
    {
        /// <summary>
        /// 用户的OpenID
        /// </summary>
        public string openid;
    }
}
