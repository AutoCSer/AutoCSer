using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 群发消息发送状态
    /// </summary>
    internal sealed class MessageStatus : Return
    {
#pragma warning disable
        /// <summary>
        /// 群发消息后返回的消息id
        /// </summary>
        public long msg_id;
        /// <summary>
        /// 消息发送后的状态，SEND_SUCCESS表示发送成功
        /// </summary>
        public string msg_status;
#pragma warning restore
    }
}
