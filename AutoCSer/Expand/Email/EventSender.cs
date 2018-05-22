using System;
using System.ComponentModel;
using System.Net.Mail;
using AutoCSer.Extension;

namespace AutoCSer.Email
{
    /// <summary>
    /// 电子邮件发送器
    /// </summary>
    internal sealed class EventSender
    {
        /// <summary>
        /// 邮件发送回调
        /// </summary>
        private readonly SendCompletedEventHandler onSend;
        /// <summary>
        /// 邮件信息
        /// </summary>
        private MailMessage message;
        /// <summary>
        /// 日志处理
        /// </summary>
        private AutoCSer.Log.ILog log;
        /// <summary>
        /// 回调委托
        /// </summary>
        private Action<Exception> callback;
        /// <summary>
        /// 电子邮件发送器
        /// </summary>
        internal EventSender()
        {
            onSend = send;
        }
        /// <summary>
        /// 邮件发送回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void send(object sender, AsyncCompletedEventArgs e)
        {
            Action<Exception> callback = this.callback;
            AutoCSer.Log.ILog log = this.log;
            message.Dispose();
            this.callback = null;
            this.log = null;
            message = null;
            AutoCSer.Threading.RingPool<EventSender>.Default.PushNotNull(this);
            if (callback != null)
            {
                try
                {
                    callback(e.Error);
                }
                catch (Exception error)
                {
                    log.Add(Log.LogType.Debug | Log.LogType.Info, error);
                }
            }
        }
        /// <summary>
        /// 获取电子邮件发送器
        /// </summary>
        /// <param name="message">邮件信息</param>
        /// <param name="smtpClient">STMP客户端</param>
        /// <param name="onSend">邮件发送回调</param>
        /// <param name="log">日志处理</param>
        /// <returns>电子邮件发送器</returns>
        internal void Send(MailMessage message, SmtpClient smtpClient, Action<Exception> onSend, AutoCSer.Log.ILog log)
        {
            this.message = message;
            this.log = log;
            callback = onSend;
            smtpClient.SendCompleted += this.onSend;
            smtpClient.SendAsync(message, this);
        }
    }
}
