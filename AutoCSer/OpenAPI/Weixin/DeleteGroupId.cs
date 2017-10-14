using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 分组的id
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct DeleteGroupId
    {
        /// <summary>
        /// 分组的id
        /// </summary>
        public int id;
    }
}
