using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 列表数据，OPENID的列表
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct OpenIdItem
    {
        /// <summary>
        /// 列表数据，OPENID的列表
        /// </summary>
        public string openid;
    }
}
