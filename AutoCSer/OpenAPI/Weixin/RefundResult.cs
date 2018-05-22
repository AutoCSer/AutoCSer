using System;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 退款查询结果
    /// </summary>
    public sealed class RefundResult : ReturnSign
    {
        /// <summary>
        /// 调用接口提交的终端设备号
        /// </summary>
        internal string device_info;
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id;
        /// <summary>
        /// 商户系统的订单号，与请求一致。
        /// </summary>
        public string out_trade_no;
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
#pragma warning disable
        /// <summary>
        /// 退款笔数
        /// </summary>
        private uint refund_count;
#pragma warning restore
        /// <summary>
        /// 退款信息集合
        /// </summary>
        public Refund[] refunds;
        /// <summary>
        /// 退款信息
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.Xml.UnknownName]
        private unsafe static bool parseRefund(AutoCSer.Xml.Parser parser, ref RefundResult value, ref Pointer.Size name)
        {
            return value.parseRefund(parser, name.Char);
        }
        /// <summary>
        /// 退款信息
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private unsafe bool parseRefund(AutoCSer.Xml.Parser parser, char* name)
        {
            int index, couponIndex = 0;
            switch (*name - 'c')
            {
                case 'c' - 'c':
                    switch (*(name + 14) - 'b')
                    {
                        case 'f' - 'b':
                            if ((index = getCouponIndex("coupon_refund_fee_", name, ref couponIndex)) >= 0)
                            {
                                if (couponIndex >= 0) return parser.CustomParse(ref refunds[index].coupons[couponIndex].coupon_refund_fee);
                                else return parser.CustomParse(ref refunds[index].coupon_refund_fee);
                            }
                            break;
                        case 'c' - 'b':
                            if ((index = getRefundIndex("coupon_refund_count_", name)) >= 0)
                            {
                                return parser.CustomParse(ref refunds[index].coupon_refund_count);
                            }
                            break;
                        case 'b' - 'b':
                            if ((index = getCouponIndex("coupon_refund_batch_id_", name, ref couponIndex)) >= 0 && couponIndex >= 0)
                            {
                                return parser.CustomParse(ref refunds[index].coupons[couponIndex].coupon_refund_batch_id);
                            }
                            break;
                        case 'i' - 'b':
                            if ((index = getCouponIndex("coupon_refund_id_", name, ref couponIndex)) >= 0 && couponIndex >= 0)
                            {
                                return parser.CustomParse(ref refunds[index].coupons[couponIndex].coupon_refund_id);
                            }
                            break;
                    }
                    break;
                case 'r' - 'c':
                    switch (*(name + 7) - 'c')
                    {
                        case 'i' - 'c':
                            if ((index = getRefundIndex("refund_id_", name)) >= 0)
                            {
                                return parser.CustomParse(ref refunds[index].refund_id);
                            }
                            break;
                        case 'c' - 'c':
                            if ((index = getRefundIndex("refund_channel_", name)) >= 0)
                            {
                                return parser.CustomEnumByte(ref refunds[index].refund_channel);
                            }
                            break;
                        case 'f' - 'c':
                            if ((index = getRefundIndex("refund_fee_", name)) >= 0)
                            {
                                return parser.CustomParse(ref refunds[index].refund_fee);
                            }
                            break;
                        case 's' - 'c':
                            if ((index = getRefundIndex("refund_status_", name)) >= 0)
                            {
                                return parser.CustomEnumByte(ref refunds[index].refund_status);
                            }
                            break;
                    }
                    break;
                case 'o' - 'c':
                    if ((index = getRefundIndex("out_refund_no_", name)) >= 0)
                    {
                        return parser.CustomParse(ref refunds[index].out_refund_no);
                    }
                    break;
            }
            return parser.CustomIgnoreValue();
        }
        /// <summary>
        /// 获取下标
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nameStart"></param>
        /// <returns></returns>
        private unsafe int getRefundIndex(string name, char* nameStart)
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
                if (index < refund_count)
                {
                    if (refunds == null) refunds = new Refund[refund_count];
                    return index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取下标
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nameStart"></param>
        /// <param name="couponIndex"></param>
        /// <returns></returns>
        private unsafe int getCouponIndex(string name, char* nameStart, ref int couponIndex)
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
                if (index < refund_count && index >= 0)
                {
                    if (refunds == null) refunds = new Refund[refund_count];
                    if (*nameStart == '_')
                    {
                        couponIndex = *++nameStart - '0';
                        do
                        {
                            uint number = (uint)(*++nameStart - '0');
                            if (number > 9) break;
                            couponIndex = couponIndex * 10 + (int)number;
                        }
                        while (true);
                        if (refunds[index].coupon_refund_count != null && couponIndex < (uint)refunds[index].coupon_refund_count && couponIndex >= 0)
                        {
                            if (refunds[index].coupons == null) refunds[index].coupons = new RefundCoupon[(uint)refunds[index].coupon_refund_count];
                            return -1;
                        }
                        return -1;
                    }
                    return index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal bool Verify(Config config)
        {
            if (IsReturn)
            {
                if (appid == config.appid && mch_id == config.mch_id && Sign<RefundResult>.Check(this, config.key, sign)) return true;
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, "签名验证错误 " + AutoCSer.Json.Serializer.Serialize(this));
            }
            return false;
        }
    }
}
