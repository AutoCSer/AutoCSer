using System;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using AutoCSer.Extension;

namespace AutoCSer.Email
{
    /// <summary>
    /// SMTP与用户信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Sender
    {
        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public string From;
        /// <summary>
        /// 发件人密码
        /// </summary>
        public string Password;
        /// <summary>
        /// SMTP信息
        /// </summary>
        public Smtp Smtp;
        /// <summary>
        /// 简单检测邮件信息
        /// </summary>
        /// <param name="content">邮件信息</param>
        /// <returns>是否合法</returns>
        private bool check(Content content)
        {
            return content != null && content.Subject != null && content.Body != null && content.SendTo != null
                && From != null && Password != null && Smtp.Server != null
                && content.Subject.Length != 0 && content.Body.Length != 0
                && Password.Length != 0 && Smtp.Server.Length != 0
                && content.SendTo.IndexOf('@') > 0 && From.IndexOf('@') > 0;
        }
        /// <summary>
        /// 获取STMP客户端
        /// </summary>
        /// <param name="message">邮件信息</param>
        /// <param name="content">邮件信息</param>
        /// <returns>STMP客户端</returns>
        private SmtpClient getSmtp(MailMessage message, Content content)
        {
            message.IsBodyHtml = content.IsHtml;
            //send.ReplyTo = new MailAddress(from, "我的接收邮箱", Encoding.GetEncoding(936));//对方回复邮件时默认的接收地址
            //send.CC.Add("a@163.com,b@163.com,c@163.com");//抄送者
            //send.Bcc.Add("a@163.com,b@163.com,c@163.com");//密送者
            //send.SubjectEncoding = Encoding.GetEncoding(936);
            //send.BodyEncoding = Encoding.GetEncoding(936);
            //send.Headers.Add("Disposition-Notification-To", from);//要求回执的标志
            //send.Headers.Add("X-Website", "http://www.showjim.com/");//自定义邮件头
            //send.Headers.Add("ReturnReceipt", "1");//针对 LOTUS DOMINO SERVER，插入回执头
            //send.Priority = MailPriority.Normal;//优先级
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(content.Body, null, "text/plain"));//普通文本邮件内容，如果对方的收件客户端不支持HTML，这是必需的
            #region 嵌入图片资源
            //AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(@"<img src=""cid:weblogo"">", null, "text/html");
            //LinkedResource lrImage = new LinkedResource(@"d:\logo.gif", "image/gif");
            //lrImage.ContentId = "weblogo";
            //htmlBody.LinkedResources.Add(lrImage);
            //send.AlternateViews.Add(htmlBody);
            #endregion
            if (content.Attachments != null)
            {
                foreach (string fileName in content.Attachments) message.Attachments.Add(new Attachment(fileName));
            }
            SmtpClient smtpClient = new SmtpClient(Smtp.Server, Smtp.Port);
            if (Smtp.IsSsl) smtpClient.EnableSsl = true;
            //smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(From, Password);
            return smtpClient;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="content">电子邮件内容</param>
        /// <param name="log">日志处理</param>
        /// <returns>邮件是否发送成功</returns>
        public bool Send(Content content, AutoCSer.Log.ILog log = null)
        {
            bool isSend = false;
            if (check(content))
            {
                using (MailMessage message = new MailMessage(From, content.SendTo, content.Subject, content.Body))
                {
                    try
                    {
                        getSmtp(message, content).Send(message);
                        isSend = true;
                    }
                    catch (Exception error)
                    {
                        log.Add(Log.LogType.Debug | Log.LogType.Info, error, "邮件发送失败 : " + content.SendTo);
                    }
                }
            }
            return isSend;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="content">电子邮件内容</param>
        /// <param name="onSend">邮件发送回调</param>
        /// <param name="log">日志处理</param>
        /// <returns>是否异步完成</returns>
        public bool Send(Content content, Action<Exception> onSend, AutoCSer.Log.ILog log = null)
        {
            if (check(content))
            {
                if (log == null) log = AutoCSer.Log.Pub.Log;
                MailMessage message = null;
                try
                {
                    message = new MailMessage(From, content.SendTo, content.Subject, content.Body);
                    message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;//如果发送失败，SMTP 服务器将发送 失败邮件告诉我
                    (AutoCSer.Threading.RingPool<EventSender>.Default.Pop() ?? new EventSender()).Send(message, getSmtp(message, content), onSend, log);
                    return true;
                }
                catch (Exception error)
                {
                    log.Add(Log.LogType.Debug | Log.LogType.Info, error, "邮件发送失败 : " + content.SendTo);
                }
                if (message != null) message.Dispose();
            }
            return false;
        }
    }
}
