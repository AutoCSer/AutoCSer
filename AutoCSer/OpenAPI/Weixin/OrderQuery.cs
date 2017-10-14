using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 订单查询信息
    /// </summary>
    [AutoCSer.Xml.Serialize(Filter = Metadata.MemberFilters.InstanceField)]//, IsAllMember = true
    public sealed class OrderQuery : SignQuery
    {
        /// <summary>
        /// 微信的订单号，优先使用
        /// </summary>
        public string transaction_id;
        /// <summary>
        /// 商户系统内部的订单号，当没提供transaction_id时需要传这个
        /// </summary>
        public string out_trade_no;
        /// <summary>
        /// 设置应用配置
        /// </summary>
        /// <param name="config">应用配置</param>
        internal void SetConfig(Config config)
        {
            setConfig(config);
            Sign<OrderQuery>.Set(this, config.key);
        }
    }
}
