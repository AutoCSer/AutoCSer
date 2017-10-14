using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 累计用户数据
    /// </summary>
    internal sealed class UserCumulateResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 累计用户数据
        /// </summary>
        public UserCumulate[] list;
#pragma warning restore
    }
}
