using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 用户语言
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct UserLanguage
    {
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid;
        /// <summary>
        /// 国家地区语言版本
        /// </summary>
        public Language lang;
    }
}
