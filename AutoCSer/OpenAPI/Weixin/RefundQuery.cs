using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 查询退款，请求需要双向证书 https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=9_4
    /// </summary>
    [AutoCSer.Xml.Serialize(Filter = Metadata.MemberFilters.InstanceField)]//, IsAllMember = true
    internal sealed class RefundQuery : SignQuery
    {
        /// <summary>
        /// 调用接口提交的终端设备号
        /// </summary>
        internal string device_info;
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        internal string transaction_id;
        /// <summary>
        /// 商户系统的订单号，与请求一致。
        /// </summary>
        internal string out_trade_no;
        /// <summary>
        /// 商户退款单号
        /// </summary>
        internal string out_refund_no;
        /// <summary>
        /// 微信退款单号
        /// </summary>
        internal string refund_id;
        /// <summary>
        /// 设置应用配置
        /// </summary>
        /// <param name="config">应用配置</param>
        internal void SetConfig(Config config)
        {
            setConfig(config);
            Sign<RefundQuery>.Set(this, config.key);
        }
    }
}
