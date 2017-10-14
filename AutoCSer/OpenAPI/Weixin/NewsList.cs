using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文消息素材列表
    /// </summary>
    public sealed class NewsList : Return
    {
        /// <summary>
        /// 素材的总数
        /// </summary>
        public int total_count;
        /// <summary>
        /// 本次调用获取的素材的数量
        /// </summary>
        public int item_count;
        /// <summary>
        /// 图文消息素材列表
        /// </summary>
        public NewsItem[] item;
    }
}
