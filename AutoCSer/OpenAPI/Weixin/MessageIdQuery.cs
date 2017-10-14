using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 删除消息发送任务的ID
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct MessageIdQuery
    {
        /// <summary>
        /// 消息发送任务的ID
        /// </summary>
        public string msg_id;
    }
}
