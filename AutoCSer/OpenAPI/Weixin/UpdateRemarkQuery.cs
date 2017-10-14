using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 设置备注名
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct UpdateRemarkQuery
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid;
        /// <summary>
        /// 新的备注名，长度必须小于30字符
        /// </summary>
        public string remark;
    }
}
