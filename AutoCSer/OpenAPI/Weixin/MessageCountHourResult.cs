using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 消息分送分时数据
    /// </summary>
    internal sealed class MessageCountHourResult : Return
    {
#pragma warning disable
        /// <summary>
        /// 消息分送分时数据
        /// </summary>
        public MessageCountHour[] list;
#pragma warning restore
    }
}
