using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 第三方分享的评论http://wiki.opensns.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91get_comment
    /// </summary>
    public partial class Comment
    {
        /// <summary>
        /// 发表评论的用户头像url
        /// </summary>
        public string pic;
        /// <summary>
        /// 发表评论的用户昵称
        /// </summary>
        public string nick;
        /// <summary>
        /// 评论内容
        /// </summary>
        public string content;
    }
}
