using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 消息发送任务的ID
    /// </summary>
    public sealed class MessageId : Return
    {
        /// <summary>
        /// 消息发送任务的ID
        /// </summary>
        public long msg_id;
        /// <summary>
        /// 消息的数据ID，该字段只有在群发图文消息时，才会出现。可以用于在图文分析数据接口中，获取到对应的图文消息的数据，是图文分析数据接口中的msgid字段中的前半部分，详见图文分析数据接口中的msgid字段的介绍。
        /// </summary>
        public long msg_data_id;
    }
}
