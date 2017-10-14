using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 对账单数据
    /// </summary>
    public sealed class Bill
    {
        /// <summary>
        /// 交易时间
        /// </summary>
        public string time;
        /// <summary>
        /// 公众账号ID
        /// </summary>
        internal string appid;
        /// <summary>
        /// 商户号
        /// </summary>
        internal string mch_id;
        /// <summary>
        /// 设备号
        /// </summary>
        internal string device_info;
        /// <summary>
        /// 微信订单号
        /// </summary>
        public string transaction_id;
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no;
        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid;
        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type;
        /// <summary>
        /// 交易状态
        /// </summary>
        public string trade_state;
        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type;
        /// <summary>
        /// 货币种类
        /// </summary>
        public string fee_type;
        /// <summary>
        /// 总金额
        /// </summary>
        public uint total_fee;
        /// <summary>
        /// 代金券或立减优惠金额
        /// </summary>
        public uint coupon_fee;
        /// <summary>
        /// 退款申请时间
        /// </summary>
        public string refund_apply_time;
        /// <summary>
        /// 退款成功时间
        /// </summary>
        public string refund_time;
        /// <summary>
        /// 微信退款单号
        /// </summary>
        internal string refund_id;
        /// <summary>
        /// 商户退款单号
        /// </summary>
        internal string out_refund_no;
        /// <summary>
        /// 退款金额
        /// </summary>
        public uint refund_fee;
        /// <summary>
        /// 代金券或立减优惠退款金额
        /// </summary>
        public uint coupon_refund_fee;
        /// <summary>
        /// 退款类型
        /// </summary>
        public string refund_type;
        /// <summary>
        /// 退款状态
        /// </summary>
        public string refund_status;
        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name;
        /// <summary>
        /// 商户数据包
        /// </summary>
        public string packet;
        /// <summary>
        /// 手续费
        /// </summary>
        public uint fee;
        /// <summary>
        /// 费率
        /// </summary>
        public string rates;
        /// <summary>
        /// 对账单数据
        /// </summary>
        /// <param name="values"></param>
        /// <param name="type"></param>
        internal Bill(LeftArray<SubString> values, BillType type)
        {
            switch (type)
            {
                case BillType.ALL:
                    if (values.Length != 24) throw new IndexOutOfRangeException(type.ToString());
                    break;
                case BillType.SUCCESS:
                    if (values.Length != 18) throw new IndexOutOfRangeException(type.ToString());
                    break;
                case BillType.REFUND:
                    if (values.Length != 26) throw new IndexOutOfRangeException(type.ToString());
                    break;
                default: throw new IndexOutOfRangeException(type.ToString());
            }
            SubString[] valueArray = values.Array;
            time = valueArray[0];
            appid = valueArray[1];
            mch_id = valueArray[2];
            //子商户号[3]
            device_info = valueArray[4];
            transaction_id = valueArray[5];
            out_trade_no = valueArray[6];
            openid = valueArray[7];
            trade_type = valueArray[8];
            trade_state = valueArray[9];
            bank_type = valueArray[10];
            fee_type = valueArray[11];
            total_fee = (uint)(decimal.Parse(valueArray[12]) * 100);
            coupon_fee = (uint)(decimal.Parse(valueArray[13]) * 100);
            int index = 14;
            if (type == BillType.REFUND)
            {
                refund_apply_time = valueArray[index++];
                refund_time = valueArray[index++];
            }
            if (type != BillType.SUCCESS)
            {
                refund_id = valueArray[index++];
                out_refund_no = valueArray[index++];
                refund_fee = (uint)(decimal.Parse(valueArray[index++]) * 100);
                coupon_refund_fee = (uint)(decimal.Parse(valueArray[index++]) * 100);
                refund_type = valueArray[index++];
                refund_status = valueArray[index++];
            }
            product_name = valueArray[index++];
            packet = valueArray[index++];
            fee = (uint)(decimal.Parse(valueArray[index++]) * 100);
            rates = valueArray[index++];
        }
        /// <summary>
        /// 对账单数据名称检测
        /// </summary>
        /// <param name="names"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool CheckName(SubString names, BillType type)
        {
            switch (type)
            {
                case BillType.ALL:
                    return names.Equals("交易时间,公众账号ID,商户号,子商户号,设备号,微信订单号,商户订单号,用户标识,交易类型,交易状态,付款银行,货币种类,总金额,代金券或立减优惠金额,微信退款单号,商户退款单号,退款金额,代金券或立减优惠退款金额,退款类型,退款状态,商品名称,商户数据包,手续费,费率");
                case BillType.SUCCESS:
                    return names.Equals("交易时间,公众账号ID,商户号,子商户号,设备号,微信订单号,商户订单号,用户标识,交易类型,交易状态,付款银行,货币种类,总金额,代金券或立减优惠金额,商品名称,商户数据包,手续费,费率");
                case BillType.REFUND:
                    return names.Equals("交易时间,公众账号ID,商户号,子商户号,设备号,微信订单号,商户订单号,用户标识,交易类型,交易状态,付款银行,货币种类,总金额,代金券或立减优惠金额,退款申请时间,退款成功时间,微信退款单号,商户退款单号,退款金额,代金券或立减优惠退款金额,退款类型,退款状态,商品名称,商户数据包,手续费,费率");
            }
            return false;
        }
    }
}
