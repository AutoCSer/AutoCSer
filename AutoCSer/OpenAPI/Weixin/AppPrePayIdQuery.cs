using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 统一下单参数 https://pay.weixin.qq.com/wiki/doc/api/app/app.php?chapter=9_1
    /// </summary>
    [AutoCSer.Xml.Serialize(Filter = Metadata.MemberFilters.InstanceField)]//, IsAllMember = true
    public sealed class AppPrePayIdQuery : PrePayIdQueryBase
    {
        /// <summary>
        /// 签名类型，目前支持HMAC-SHA256和MD5，默认为MD5
        /// </summary>
        internal readonly string sign_type = "MD5";
        /// <summary>
        /// 交易类型[必填]
        /// </summary>
        internal readonly TradeType trade_type = TradeType.APP;

        /// <summary>
        /// 设置应用配置
        /// </summary>
        /// <param name="config">应用配置</param>
        internal void SetConfig(Config config)
        {
            setConfig(config);
            Sign<AppPrePayIdQuery>.Set(this, config.key);
        }
    }
}
