using System;

namespace AutoCSer.Email
{
    /// <summary>
    /// 发送邮件信息
    /// </summary>
    public sealed class Content
    {
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
        /// 邮件内容是否HTML代码
        /// </summary>
        public bool IsHtml;
        /// <summary>
        /// 附件文件名集合
        /// </summary>
        public string[] Attachments;
    }
}
