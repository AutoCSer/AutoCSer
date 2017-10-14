using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 接口分析数据
    /// </summary>
    public class InterfaceSummary
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public string ref_date;
        /// <summary>
        /// 总耗时，除以callback_count即为平均耗时
        /// </summary>
        public long total_time_cost;
        /// <summary>
        /// 最大耗时
        /// </summary>
        public long max_time_cost;
        /// <summary>
        /// 通过服务器配置地址获得消息后，被动回复用户消息的次数
        /// </summary>
        public int callback_count;
        /// <summary>
        /// 上述动作的失败次数
        /// </summary>
        public int fail_count;
    }
}
