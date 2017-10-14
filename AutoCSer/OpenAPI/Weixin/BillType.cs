using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 账单类型
    /// </summary>
    public enum BillType : byte
    {
        /// <summary>
        /// 返回当日所有订单信息，默认值
        /// </summary>
        ALL,
        /// <summary>
        /// 返回当日成功支付的订单
        /// </summary>
        SUCCESS,
        /// <summary>
        /// 返回当日退款订单
        /// </summary>
        REFUND,
        ///// <summary>
        ///// 已撤销的订单
        ///// </summary>
        //REVOKED
    }
}
