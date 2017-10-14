using System;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 签名请求
    /// </summary>
    public abstract class SignQuery
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
        /// <summary>
        /// 设置应用配置
        /// </summary>
        /// <param name="config">应用配置</param>
        protected internal void setConfig(Config config)
        {
            appid = config.appid;
            mch_id = config.mch_id;
            nonce_str = AutoCSer.Random.Default.SecureNextULongNotZero().toHex();
        }
    }
}
