using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 代金券或立减优惠
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct PayNotifyCoupon
    {
        /// <summary>
        /// 下标
        /// </summary>
        public int index;
        /// <summary>
        /// 代金券或立减优惠ID
        /// </summary>
        public string coupon_id;
        /// <summary>
        /// 单个代金券或立减优惠支付金额
        /// </summary>
        public uint coupon_fee;
    }
}
