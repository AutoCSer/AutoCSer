using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 微信服务器IP地址
    /// </summary>
    internal sealed class CallbakIP : Return
    {
#pragma warning disable
        /// <summary>
        /// 微信服务器IP地址列表
        /// </summary>
        public string[] ip_list;
#pragma warning restore
    }
}
