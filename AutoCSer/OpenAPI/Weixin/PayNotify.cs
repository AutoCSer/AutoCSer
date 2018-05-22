using System;
using AutoCSer.Extension;
#pragma warning disable

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 扫码支付完成回调通知请求参数
    /// </summary>
    public class PayNotify : ReturnSign
    {
        /// <summary>
        /// 调用接口提交的终端设备号
        /// </summary>
        internal string device_info;
        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid;
        /// <summary>
        /// 是否关注公众账号
        /// </summary>
        public string is_subscribe;
        /// <summary>
        /// 交易类型[必填]
        /// </summary>
        public TradeType trade_type;
        /// <summary>
        /// 银行类型
        /// </summary>
        public string bank_type;
        /// <summary>
        /// 订单总金额，单位分
        /// </summary>
        public uint total_fee;
        /// <summary>
        /// 现金支付金额，单位分
        /// </summary>
        public uint cash_fee;
        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，其他值列表详见货币类型
        /// </summary>
        public string fee_type;
        /// <summary>
        /// 现金支付货币类型
        /// </summary>
        public string cash_fee_type;
        /// <summary>
        /// 代金券或立减优惠金额 小于等于 订单总金额，订单总金额-代金券或立减优惠金额=现金支付金额，详见支付金额
        /// </summary>
        public uint? coupon_fee;
        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        private uint? coupon_count;
        /// <summary>
        /// 代金券或立减优惠(存在sign验证问题)
        /// </summary>
        public PayNotifyCoupon[] coupons;
        /// <summary>
        /// 代金券或立减优惠解析
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.Xml.UnknownName]
        private unsafe static bool parseCoupon(AutoCSer.Xml.Parser parser, ref PayNotify value, ref Pointer.Size name)
        {
            return value.parseCoupon(parser, name.Char);
        }
        /// <summary>
        /// 代金券或立减优惠解析
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected unsafe bool parseCoupon(AutoCSer.Xml.Parser parser, char* name)
        {
            int index;
            char code = *(name + 7);
            if (code == 'i')
            {
                if ((index = getCouponIndex("coupon_id_", name)) >= 0)
                {
                    return parser.CustomParse(ref coupons[index].coupon_id);
                }
            }
            else if (code == 'f')
            {
                if ((index = getCouponIndex("coupon_fee_", name)) >= 0)
                {
                    return parser.CustomParse(ref coupons[index].coupon_fee);
                }
            }
            return parser.CustomIgnoreValue();
        }
        /// <summary>
        /// 获取下标
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nameStart"></param>
        /// <returns></returns>
        private unsafe int getCouponIndex(string name, char* nameStart)
        {
            if (AutoCSer.Extension.String_Weixin.SimpleEqual(name, nameStart))
            {
                int index = *(nameStart += name.Length) - '0';
                do
                {
                    uint number = (uint)(*++nameStart - '0');
                    if (number > 9) break;
                    index = index * 10 + (int)number;
                }
                while (true);
                if (coupon_count != null && index < (int)coupon_count)
                {
                    if (coupons == null) coupons = new PayNotifyCoupon[(int)coupon_count];
                    return index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id;
        /// <summary>
        /// 商户系统的订单号，与请求一致。
        /// </summary>
        public string out_trade_no;
        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        public string attach;
        /// <summary>
        /// 支付完成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见
        /// </summary>
        public string time_end;
        /// <summary>
        /// 微信支付完成回调验证
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool Verify(Config config = null)
        {
            if (IsReturn)
            {
                if (config == null) config = Config.Default;
                if (appid == config.appid && mch_id == config.mch_id && Sign<PayNotify>.Check(this, config.key, sign)) return true;
                config.PayLog.Add(Log.LogType.Debug | Log.LogType.Info, "微信支付回调验证错误 " + AutoCSer.Json.Serializer.Serialize(this));
            }
            return false;
        }
        /// <summary>
        /// 微信支付完成回调验证
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public bool Verify(API api)
        {
            return Verify(api.config);
        }
        /// <summary>
        /// 获取扫码支付回调返回值
        /// </summary>
        /// <param name="return_msg">返回信息 返回信息，如非空，为错误原因 签名失败 参数格式校验错误</param>
        /// <param name="config"></param>
        /// <returns></returns>
        public ReturnCode GetErrorResult(string return_msg, Config config = null)
        {
            (config ?? Config.Default).PayLog.Add(Log.LogType.Debug | Log.LogType.Info, return_msg);
            return new ReturnCode { return_msg = return_msg };
        }
    }
}
