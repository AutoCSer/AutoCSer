using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文群发总数据
    /// </summary>
    public sealed class ArticleTotalDetail : Counts
    {
        /// <summary>
        /// 统计的日期，在getarticletotal接口中，ref_date指的是文章群发出日期， 而stat_date是数据统计日期
        /// </summary>
        public string stat_date;
        /// <summary>
        /// 送达人数，一般约等于总粉丝数（需排除黑名单或其他异常情况下无法收到消息的粉丝）
        /// </summary>
        public int target_user;
    }
}
