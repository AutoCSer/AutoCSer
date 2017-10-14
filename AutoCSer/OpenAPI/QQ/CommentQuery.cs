using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 第三方分享的评论列表 请求
    /// </summary>
    public partial class CommentQuery : AppId
    {
        /// <summary>
        /// 网页的URL，查询评论时的起始位置，一般情况下可以不传值或传入0，表示从第一条开始读取评论列表
        /// </summary>
        public string url;
        /// <summary>
        /// start参数是为一种特殊情况准备的，即需要分页展示评论时，则start可设置为该页显示的条数。例如如果start为10，则会跳过第10条评论，从第11条评论开始读取。如果传入的start比实际总的评论数还要大，则读取到0条评论
        /// </summary>
        public int start;
        /// <summary>
        /// 表示查询评论的返回限制数（即最多期望返回几条评论）。num不传则默认返回200条评论，所有评论不足200条则返回所有评论
        /// </summary>
        public int num;
    }
}
