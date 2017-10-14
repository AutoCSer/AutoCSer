using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 未接入会话列表
    /// </summary>
    public sealed class WaitCustomSession : Return
    {
        /// <summary>
        /// 未接入会话数量
        /// </summary>
        public int count;
        /// <summary>
        /// 未接入会话列表
        /// </summary>
        public WaitSession[] waitcaselist;
    }
}
