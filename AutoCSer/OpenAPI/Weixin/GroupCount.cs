using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 分组用户数量
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct GroupCount
    {
        /// <summary>
        /// 分组名字，UTF8编码
        /// </summary>
        public string name;
        /// <summary>
        /// 分组id，由微信分配
        /// </summary>
        public int id;
        /// <summary>
        /// 分组内用户数量
        /// </summary>
        public int count;
    }
}
