using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 未接入会话
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct WaitSession
    {
        /// <summary>
        /// 用户来访时间，UNIX时间戳
        /// </summary>
        public long createtime;
        /// <summary>
        /// 指定接待的客服，为空表示未指定客服
        /// </summary>
        public string kf_account;
        /// <summary>
        /// 客户openid
        /// </summary>
        public string openid;
    }
}
