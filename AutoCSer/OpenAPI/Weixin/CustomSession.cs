using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 多客服的客服工号会话
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct CustomSession
    {
        /// <summary>
        /// 完整客服账号，格式为：账号前缀@公众号微信号
        /// </summary>
        public string kf_account;
        /// <summary>
        /// 客户openid
        /// </summary>
        public string openid;
        /// <summary>
        /// 附加信息，文本会展示在客服人员的多客服客户端
        /// </summary>
        public string text;
    }
}
