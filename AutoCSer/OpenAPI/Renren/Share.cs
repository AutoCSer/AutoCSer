using System;

namespace AutoCSer.OpenAPI.Renren
{
    /// <summary>
    /// 分享
    /// </summary>
    public sealed class Share : IReturn
    {
        /// <summary>
        /// 分享的ID
        /// </summary>
        public long id;
        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsReturn
        {
            get { return id != 0; }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get { return string.Empty; }
        }
        /// <summary>
        /// 发起本次分享的用户ID
        /// </summary>
        public int user_id;
        /// <summary>
        /// 最初发起此分享的用户ID，若为0，表示user_id即为original_user_id
        /// </summary>
        public int original_user_id;
        /// <summary>
        /// 被分享资源的ID
        /// </summary>
        public long resource_id;
        /// <summary>
        /// 被分享资源所有者的用户ID
        /// </summary>
        public int resource_owner_id;
        /// <summary>
        /// 分享的标题
        /// </summary>
        public string title;
        /// <summary>
        /// 分享的链接地址
        /// </summary>
        public string url;
        /// <summary>
        /// 分享的缩略图
        /// </summary>
        public string thumbnail_url;
        /// <summary>
        /// 分享的内容的摘要
        /// </summary>
        public string summary;
        /// <summary>
        /// 分享被评论的次数
        /// </summary>
        public int comment_count;
        /// <summary>
        /// 分享产生的时间
        /// </summary>
        public string share_time;
        /// <summary>
        /// 当前access_token(或session_key)对应用户是否like（赞）
        /// </summary>
        public int my_like;
        /// <summary>
        /// 分享被like（赞）的次数
        /// </summary>
        public int like_count;
    }
}
