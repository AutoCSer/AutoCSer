using System;

namespace AutoCSer.OpenAPI.Weixin
{
#pragma warning disable
    /// <summary>
    /// 用户增减数据
    /// </summary>
    internal sealed class UserSummaryResult : Return
    {
        /// <summary>
        /// 用户增减数据
        /// </summary>
        public UserSummary[] list;
    }
#pragma warning restore
}
