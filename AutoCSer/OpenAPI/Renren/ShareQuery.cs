using System;

namespace AutoCSer.OpenAPI.Renren
{
    /// <summary>
    /// 发表分享
    /// </summary>
    public partial class ShareQuery : Form
    {
        /// <summary>
        /// 分享的类型：日志为1、照片为2、链接为6、相册为8、视频为10、音频为11、分享为20。（必填项）
        /// 当分享日志、照片、相册等人人网站内内容时，ugc_id和user_id为必须参数。
        /// 当分享优酷视频、站外链接等人人网站外内容时，url为必须参数。此时type只能是：链接为6、视频为10、音频为11。
        /// 当基于现存分享再次进行分享时（可以获取到分享的ID），type只能是：分享20，ugc_id和user_id为必须参数。
        /// </summary>
        public int type = 6;
        /// <summary>
        /// 分享人人网站内内容的ID。如日志、照片、相册、分享的ID。
        /// </summary>
        public int ugc_id;
        /// <summary>
        /// 分享人人网站内内容所有者的ID。如日志、照片、相册、分享所有者的用户ID。
        /// </summary>
        public int user_id;
        /// <summary>
        /// 分享人人网站外内容的URL。
        /// </summary>
        public string url;
        /// <summary>
        /// 分享内容时，用户的评论内容。
        /// </summary>
        public string comment;
        /// <summary>
        /// 新鲜事中来源信息链接，连接的名字默认为App的名字。该参数为JSON格式，格式如下：{ "text": "App A", "href": "http://appa.com/path" }
        /// </summary>
        public string source_link;
    }
}
