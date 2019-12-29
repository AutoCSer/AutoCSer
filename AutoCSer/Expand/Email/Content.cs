using System;

namespace AutoCSer.Email
{
    /// <summary>
    /// 发送邮件信息
    /// </summary>
    public sealed class Content
    {
        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public string From;
        /// <summary>
        /// 收件人邮箱
        /// </summary>
        public string SendTo;
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject;
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body;
        /// <summary>
        /// HTML 邮件的文本内容，IsHtml 为 true 时有效
        /// </summary>
        public string TextBody;
        /// <summary>
        /// 邮件内容是否HTML代码
        /// </summary>
        public bool IsHtml;
        /// <summary>
        /// HTML 是否增加视图附件
        /// </summary>
        public bool IsHtmlView;
        /// <summary>
        /// 附件文件名集合
        /// </summary>
        public string[] Attachments;
    }
}
