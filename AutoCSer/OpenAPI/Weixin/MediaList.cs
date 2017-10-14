using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 永久素材列表
    /// </summary>
    public sealed class MediaList : Return
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
        /// 永久素材列表
        /// </summary>
        public MediaItem[] item;
    }
}
