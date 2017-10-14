using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文群发每日数据，某天所有被阅读过的文章（仅包括群发的文章）在当天的阅读次数等数据
    /// </summary>
    public sealed class ArticleSummary : UserRead
    {
        /// <summary>
        /// 这里的msgid实际上是由msgid（图文消息id，这也就是群发接口调用后返回的msg_data_id）和index（消息次序索引）组成， 例如12003_3， 其中12003是msgid，即一次群发的消息的id； 3为index，假设该次群发的图文消息共5个文章（因为可能为多图文），3表示5个中的第3个
        /// </summary>
        public string msgid;
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
    }
}
