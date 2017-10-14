using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文分享转发每日数据
    /// </summary>
    public sealed class UserShareHour : UserShare
    {
        /// <summary>
        /// 数据的小时，包括从000到2300，分别代表的是[000,100)到[2300,2400)，即每日的第1小时和最后1小时
        /// </summary>
        public short ref_hour;
    }
}
