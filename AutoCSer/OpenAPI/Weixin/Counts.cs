using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 统计数据
    /// </summary>
    public abstract class Counts
    {
        /// <summary>
        /// 图文页（点击群发图文卡片进入的页面）的阅读人数
        /// </summary>
        public int int_page_read_user;
        /// <summary>
        /// 图文页的阅读次数
        /// </summary>
        public int int_page_read_count;
        /// <summary>
        /// 原文页（点击图文页“阅读原文”进入的页面）的阅读人数，无原文页时此处数据为0
        /// </summary>
        public int ori_page_read_user;
        /// <summary>
        /// 原文页的阅读次数
        /// </summary>
        public int ori_page_read_count;
        /// <summary>
        /// 分享的人数
        /// </summary>
        public int share_user;
        /// <summary>
        /// 分享的次数
        /// </summary>
        public int share_count;
        /// <summary>
        /// 收藏的人数
        /// </summary>
        public int add_to_fav_user;
        /// <summary>
        /// 收藏的次数
        /// </summary>
        public int add_to_fav_count;
    }
}
