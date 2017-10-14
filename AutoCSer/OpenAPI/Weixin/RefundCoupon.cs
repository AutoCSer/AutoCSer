using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 代金券或立减优惠
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct RefundCoupon
    {
        /// <summary>
        /// 批次ID ,$n为下标，$m为下标，从0开始编号
        /// </summary>
        public string coupon_refund_batch_id;
        /// <summary>
        /// 代金券或立减优惠ID, $n为下标，$m为下标，从0开始编号
        /// </summary>
        public string coupon_refund_id;
        /// <summary>
        /// 单个代金券或立减优惠支付金额, $n为下标，$m为下标，从0开始编号
        /// </summary>
        public uint coupon_refund_fee;
    }
}
