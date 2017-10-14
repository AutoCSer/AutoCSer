using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 统一下单参数
    /// </summary>
    public abstract class PrePayIdQueryBase : SignQuery
    {
        /// <summary>
        /// 终端设备号(门店号或收银设备ID)，注意：PC网页或公众号内支付请传"WEB"
        /// </summary>
        public string device_info = "WEB";
        /// <summary>
        /// 终端IP，APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP String(16)[必填]
        /// </summary>
        public string spbill_create_ip;
        /// <summary>
        /// 商户系统内部的订单号,32个字符内、可包含字母, 其他说明见商户订单号[必填]
        /// </summary>
        public string out_trade_no;
        /// <summary>
        /// 商品描述，商品或支付单简要描述 String(32)[必填]
        /// </summary>
        public string body;
        /// <summary>
        /// 商品名称明细列表 String(8192)
        /// </summary>
        public string detail;
        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据 String(127)
        /// </summary>
        public string attach;
        /// <summary>
        /// 商品标记，代金券或立减优惠功能的参数 String(32)
        /// </summary>
        public string goods_tag;
        /// <summary>
        /// 订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见
        /// </summary>
        public string time_start;
        /// <summary>
        /// 订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010。注意：最短失效时间间隔必须大于5分钟
        /// </summary>
        public string time_expire;
        /// <summary>
        /// 接收微信支付异步通知回调地址 String(256)[必填]
        /// </summary>
        public string notify_url;
        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，其他值列表详见货币类型
        /// </summary>
        internal readonly string fee_type = "CNY";
        /// <summary>
        /// 指定支付方式 no_credit指定不能使用信用卡支付
        /// </summary>
        public string limit_pay;
        /// <summary>
        /// 订单总金额，单位分[必填]
        /// </summary>
        public uint total_fee;
    }
}
