using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文群发总数据，某天群发的文章，从群发日起到接口调用日（但最多统计发表日后7天数据），每天的到当天的总等数据。例如某篇文章是12月1日发出的，发出后在1日、2日、3日的阅读次数分别为1万，则getarticletotal获取到的数据为，距发出到12月1日24时的总阅读量为1万，距发出到12月2日24时的总阅读量为2万，距发出到12月1日24时的总阅读量为3万。
    /// </summary>
    public sealed class ArticleTotal
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public string ref_date;
        /// <summary>
        /// 这里的msgid实际上是由msgid（图文消息id，这也就是群发接口调用后返回的msg_data_id）和index（消息次序索引）组成， 例如12003_3， 其中12003是msgid，即一次群发的消息的id； 3为index，假设该次群发的图文消息共5个文章（因为可能为多图文），3表示5个中的第3个
        /// </summary>
        public string msgid;
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 图文群发总数据
        /// </summary>
        public ArticleTotalDetail[] details;
    }
}
