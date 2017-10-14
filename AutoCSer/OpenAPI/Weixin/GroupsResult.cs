using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 所有分组
    /// </summary>
    internal sealed class GroupsResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 公众平台分组信息列表
        /// </summary>
        public GroupCount[] groups;
#pragma warning restore
    }
}
