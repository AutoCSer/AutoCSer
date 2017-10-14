using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 消息发送分布数据
    /// </summary>
    internal sealed class MessageDistributedResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 消息发送分布数据
        /// </summary>
        public MessageDistributed[] list;
#pragma warning restore
    }
}
