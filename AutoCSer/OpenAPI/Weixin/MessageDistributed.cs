using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 消息发送分布数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct MessageDistributed
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
        /// 当日发送消息量分布的区间，0代表 “0”，1代表“1-5”，2代表“6-10”，3代表“10次以上”
        /// </summary>
        public byte count_interval;
    }
}
