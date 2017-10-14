using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 删除分组
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct DeleteGroupQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteGroupId group;
    }
}
