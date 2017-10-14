using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 交易类型
    /// </summary>
    public enum TradeType : byte
    {
        /// <summary>
        /// 原生扫码支付
        /// </summary>
        NATIVE,
        /// <summary>
        /// app支付
        /// </summary>
        APP,
        /// <summary>
        /// 公众号支付
        /// </summary>
        JSAPI,
        /// <summary>
        /// 手机浏览器H5支付，统一下单接口trade_type的传参可参考这里
        /// </summary>
        WAP,
        /// <summary>
        /// 刷卡支付，刷卡支付有单独的支付接口，不调用统一下单接口
        /// </summary>
        MICROPAY
    }
}
