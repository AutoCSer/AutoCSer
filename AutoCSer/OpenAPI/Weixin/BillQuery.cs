using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 下载对账单
    /// </summary>
    [AutoCSer.Xml.Serialize(Filter = Metadata.MemberFilters.InstanceField)]//, IsAllMember = true
    internal sealed class BillQuery : SignQuery
    {
        /// <summary>
        /// 终端设备号(门店号或收银设备ID)，注意：PC网页或公众号内支付请传"WEB"
        /// </summary>
        public string device_info;
        /// <summary>
        /// 下载对账单的日期，格式：20140603
        /// </summary>
        public string bill_date;
        /// <summary>
        /// 账单类型
        /// </summary>
        public BillType bill_type;
        /// <summary>
        /// 设置应用配置
        /// </summary>
        /// <param name="config">应用配置</param>
        internal void SetConfig(Config config)
        {
            setConfig(config);
            Sign<BillQuery>.Set(this, config.key);
        }
    }
}
