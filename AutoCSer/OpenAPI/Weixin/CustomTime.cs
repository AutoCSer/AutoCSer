using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 客服的会话
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct CustomTime
    {
        /// <summary>
        /// 客户openid
        /// </summary>
        public string openid;
        /// <summary>
        /// 会话创建时间，UNIX时间戳
        /// </summary>
        public long createtime;
    }
}
