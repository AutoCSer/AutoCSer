using System;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 订单查询结果
    /// </summary>
    public sealed class OrderResult : PayNotify
    {
        /// <summary>
        /// 交易状态
        /// </summary>
        public OrderResultTradeState trade_state;
        /// <summary>
        /// `对当前查询订单状态的描述和下一步操作的指引
        /// </summary>
        public string trade_state_desc;
        /// <summary>
        /// 代金券或立减优惠解析
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.Xml.UnknownName]
        private unsafe static bool parseCoupon(AutoCSer.Xml.Parser parser, ref OrderResult value, ref Pointer.Size name)
        {
            return value.parseCoupon(parser, name.Char);
        }
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public new bool Verify(Config config = null)
        {
            if (config == null) config = Config.Default;
            if (appid == config.appid && mch_id == config.mch_id && Sign<PayNotify>.Check(this, config.key, sign)) return true;
            config.PayLog.Add(Log.LogType.Debug | Log.LogType.Info, "签名验证错误 " + AutoCSer.Json.Serializer.Serialize(this));
            return false;
        }
    }
}
