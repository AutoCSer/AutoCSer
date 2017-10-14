using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 访问令牌
    /// </summary>
    internal sealed class Token : Return
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token;
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in;
    }
}
