using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 退款信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Refund
    {
        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string out_refund_no;
        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string refund_id;
        /// <summary>
        /// 退款渠道
        /// </summary>
        public RefundChannel? refund_channel;
        /// <summary>
        /// 退款总金额,单位为分,可以做部分退款
        /// </summary>
        public uint refund_fee;
        /// <summary>
        /// 代金券或立减优惠退款金额，退款金额-代金券或立减优惠退款金额为现金
        /// </summary>
        public uint? coupon_refund_fee;
        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        internal uint? coupon_refund_count;
        /// <summary>
        /// 代金券或立减优惠
        /// </summary>
        public RefundCoupon[] coupons;
        /// <summary>
        /// 退款状态
        /// </summary>
        public RefundStatus refund_status;
    }
}
