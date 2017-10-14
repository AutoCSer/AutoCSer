using System;
#pragma warning disable

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// XML签名返回值
    /// </summary>
    public abstract class ReturnSign : ErrorCode
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        internal string appid;
        /// <summary>
        /// 商户号
        /// </summary>
        internal string mch_id;
        /// <summary>
        /// 随机字符串
        /// </summary>
        internal string nonce_str;
        /// <summary>
        /// 签名
        /// </summary>
        internal string sign;
    }
}
