using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 统一下单参数
    /// </summary>
    [AutoCSer.Xml.Serialize(Filter = Metadata.MemberFilters.InstanceField)]//, IsAllMember = true
    public sealed class PrePayIdQuery : PrePayIdQueryBase
    {
        /// <summary>
        /// 用户标识 trade_type=JSAPI，此参数必传
        /// </summary>
        public string openid;
        /// <summary>
        /// 商品ID trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        public string product_id;
        /// <summary>
        /// 交易类型[必填]
        /// </summary>
        public TradeType trade_type;
        /// <summary>
        /// 设置应用配置
        /// </summary>
        /// <param name="config">应用配置</param>
        internal void SetConfig(Config config)
        {
            setConfig(config);
            Sign<PrePayIdQuery>.Set(this, config.key);
        }
    }
}
