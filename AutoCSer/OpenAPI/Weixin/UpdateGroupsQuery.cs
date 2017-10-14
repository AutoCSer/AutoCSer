using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 批量移动用户分组
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct UpdateGroupsQuery
    {
        /// <summary>
        /// 用户唯一标识符openid的列表
        /// </summary>
        public string[] openid_list;
        /// <summary>
        /// 分组id
        /// </summary>
        public int to_groupid;
    }
}
