using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 对账单统计数据
    /// </summary>
    public sealed class BillTotal
    {
        /// <summary>
        /// 总交易单数
        /// </summary>
        public uint total_count;
        /// <summary>
        /// 总交易额
        /// </summary>
        public ulong total_fee;
        /// <summary>
        /// 总退款金额
        /// </summary>
        public ulong refund_fee;
        /// <summary>
        /// 总代金券或立减优惠退款金额
        /// </summary>
        public ulong coupon_refund_fee;
        /// <summary>
        /// 手续费总金额
        /// </summary>
        public ulong fee;
        /// <summary>
        /// 对账单数据集合
        /// </summary>
        public Bill[] bills;
        /// <summary>
        /// 对账单统计数据
        /// </summary>
        /// <param name="values"></param>
        /// <param name="bills"></param>
        internal BillTotal(LeftArray<SubString> values, Bill[] bills)
        {
            if (values.Length != 5) throw new IndexOutOfRangeException();
            SubString[] valueArray = values.Array;
            total_count = uint.Parse(valueArray[0]);
            total_fee = (ulong)(decimal.Parse(valueArray[1]) * 100);
            refund_fee = (ulong)(decimal.Parse(valueArray[2]) * 100);
            coupon_refund_fee = (ulong)(decimal.Parse(valueArray[3]) * 100);
            fee = (ulong)(decimal.Parse(valueArray[4]) * 100);
            this.bills = bills;
        }
        /// <summary>
        /// 对账单统计数据
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        internal static bool CheckName(SubString names)
        {
            return names.Equals("总交易单数,总交易额,总退款金额,总代金券或立减优惠退款金额,手续费总金额");
        }
    }
}
