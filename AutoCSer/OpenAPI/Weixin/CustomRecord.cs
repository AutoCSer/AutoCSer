using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 客服聊天记录
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct CustomRecord
    {
        /// <summary>
        /// 操作时间
        /// </summary>
        public long time;
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid;
        /// <summary>
        /// 客服账号
        /// </summary>
        public string worker;
        /// <summary>
        /// 聊天记录
        /// </summary>
        public string text;
        /// <summary>
        /// 会话状态
        /// 1000	创建未接入会话
        /// 1001	接入会话
        /// 1002	主动发起会话
        /// 1003	转接会话
        /// 1004	关闭会话
        /// 1005	抢接会话
        /// 2001	公众号收到消息
        /// 2002	客服发送消息
        /// 2003	客服收到消息
        /// </summary>
        public short opercode;
    }
}
