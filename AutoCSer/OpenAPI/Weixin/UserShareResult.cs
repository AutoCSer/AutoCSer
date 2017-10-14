using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文分享转发数据
    /// </summary>
    internal sealed class UserShareResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 图文分享转发数据
        /// </summary>
        public UserShare[] list;
#pragma warning restore
    }
}
