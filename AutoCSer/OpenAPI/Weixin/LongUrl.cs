using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 长链接
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct LongUrl
    {
        /// <summary>
        /// 此处填long2short，代表长链接转短链接
        /// </summary>
        public string action;
        /// <summary>
        /// 需要转换的长链接，支持http://、https://、weixin://wxpay 格式的url
        /// </summary>
        public string long_url;
    }
}
