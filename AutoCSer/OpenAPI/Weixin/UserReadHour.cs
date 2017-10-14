using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文统计分时数据
    /// </summary>
    public sealed class UserReadHour : UserRead
    {
        /// <summary>
        /// 数据的小时，包括从000到2300，分别代表的是[000,100)到[2300,2400)，即每日的第1小时和最后1小时
        /// </summary>
        public short ref_hour;
        /// <summary>
        /// 在获取图文阅读分时数据时才有该字段，代表用户从哪里进入来阅读该图文。0:会话;1.好友;2.朋友圈;3.腾讯微博;4.历史消息页;5.其他
        /// </summary>
        public byte user_source;
    }
}
