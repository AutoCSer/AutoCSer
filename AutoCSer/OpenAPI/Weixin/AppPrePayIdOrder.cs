using System;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 交易会话信息
    /// </summary>
    public sealed class AppPrePayIdOrder
    {
        /// <summary>
        /// 交易会话信息
        /// </summary>
        /// <param name="config"></param>
        /// <param name="prePayId"></param>
        /// <param name="noncestr"></param>
        internal AppPrePayIdOrder(Config config, string prePayId, string noncestr)
        {
            appid = config.appid;
            partnerid = config.partnerid;
            prepayid = prePayId;
            this.noncestr = noncestr;
            timestamp = (((Date.NowTime.Set().Ticks) - AutoCSer.Json.Parser.JavascriptLocalMinTimeTicks) / TimeSpan.TicksPerSecond).toString();
            Sign<AppPrePayIdOrder>.Set(this, config.key);
        }
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid;
        /// <summary>
        /// 
        /// </summary>
        public string partnerid;
        /// <summary>
        /// 交易会话标识
        /// </summary>
        public string prepayid;
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string noncestr;
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp;
        /// <summary>
        /// 
        /// </summary>
        public readonly string package = "Sign=WXPay";
        /// <summary>
        /// 数据签名
        /// </summary>
        public string sign;
    }
}
