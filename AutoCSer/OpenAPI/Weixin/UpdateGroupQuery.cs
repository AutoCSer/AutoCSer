using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 移动用户分组
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct UpdateGroupQuery
    {
        /// <summary>
        /// 用户唯一标识符
        /// </summary>
        public string openid;
        /// <summary>
        /// 分组id
        /// </summary>
        public int to_groupid;
    }
}
