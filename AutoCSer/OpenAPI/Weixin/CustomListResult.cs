using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 客服的会话列表
    /// </summary>
    internal sealed class CustomListResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 会话列表
        /// </summary>
        public CustomTime[] sessionlist;
#pragma warning restore
    }
}
