using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 消息发送概况数据
    /// </summary>
    public class MessageCount
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public string ref_date;
        /// <summary>
        /// 上行发送了（向公众号发送了）消息的用户数
        /// </summary>
        public int msg_user;
        /// <summary>
        /// 上行发送了消息的消息总数
        /// </summary>
        public int msg_count;
        /// <summary>
        /// 消息类型，1代表文字 2代表图片 3代表语音 4代表视频 6代表第三方应用消息（链接消息）
        /// </summary>
        public int msg_type;
    }
}
