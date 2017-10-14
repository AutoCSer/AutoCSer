using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文统计数据
    /// </summary>
    internal sealed class UserReadResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 图文统计数据
        /// </summary>
        public UserRead[] list;
#pragma warning restore
    }
}
