using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 客服聊天记录
    /// </summary>
    internal sealed class CustomRecordList : Return
    {
#pragma warning disable
        /// <summary>
        /// 客服聊天记录
        /// </summary>
        public CustomRecord[] recordlist;
#pragma warning restore
    }
}
