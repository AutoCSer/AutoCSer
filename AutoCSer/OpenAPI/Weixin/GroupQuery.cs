using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 分组查询
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct GroupQuery
    {
        /// <summary>
        /// 分组查询
        /// </summary>
        public GroupName group;
    }
}
