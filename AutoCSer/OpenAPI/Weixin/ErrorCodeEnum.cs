using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public enum ErrorCodeEnum : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 商户无此接口权限，商户未开通此接口权限，请商户前往申请此接口权限
        /// </summary>
        NOAUTH,
        /// <summary>
        /// 用户帐号余额不足，请用户充值或更换支付卡后再支付
        /// </summary>
        NOTENOUGH,
        /// <summary>
        /// 商户订单已支付，无需更多操作
        /// </summary>
        ORDERPAID,
        /// <summary>
        /// 当前订单已关闭，无法支付，请重新下单
        /// </summary>
        ORDERCLOSED,
        /// <summary>
        /// 系统异常，请用相同参数重新调用
        /// </summary>
        SYSTEMERROR,
        /// <summary>
        /// APPID不存在
        /// </summary>
        APPID_NOT_EXIST,
        /// <summary>
        /// MCHID不存在
        /// </summary>
        MCHID_NOT_EXIST,
        /// <summary>
        /// appid和mch_id不匹配
        /// </summary>
        APPID_MCHID_NOT_MATCH,
        /// <summary>
        /// 缺少必要的请求参数
        /// </summary>
        LACK_PARAMS,
        /// <summary>
        /// 商户订单号重复
        /// </summary>
        OUT_TRADE_NO_USED,
        /// <summary>
        /// 签名错误
        /// </summary>
        SIGNERROR,
        /// <summary>
        /// XML格式错误
        /// </summary>
        XML_FORMAT_ERROR,
        /// <summary>
        /// 未使用post传递参数
        /// </summary>
        REQUIRE_POST_METHOD,
        /// <summary>
        /// post数据为空
        /// </summary>
        POST_DATA_EMPTY,
        /// <summary>
        /// 编码格式错误
        /// </summary>
        NOT_UTF8,
        /// <summary>
        /// 此交易订单号不存在，该API只能查提交支付交易返回成功的订单，请商户检查需要查询的订单号是否正确
        /// </summary>
        ORDERNOTEXIST,
        /// <summary>
        /// 无效transaction_id
        /// </summary>
        INVALID_TRANSACTIONID,
        /// <summary>
        /// 请求参数错误，请重新检查再调用退款申请
        /// </summary>
        PARAM_ERROR,
    }
}
