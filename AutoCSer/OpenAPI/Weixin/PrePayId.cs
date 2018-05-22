using System;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 交易会话标识
    /// </summary>
    public sealed class PrePayId : AppPrePayId
    {
        #region 以下字段在return_code 和result_code都为SUCCESS的时候有返回
        /// <summary>
        /// 二维码链接 trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付
        /// </summary>
        internal string code_url;
        #endregion
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal new bool Verify(Config config)
        {
            if (IsReturn)
            {
                if (appid == config.appid && mch_id == config.mch_id && Sign<PrePayId>.Check(this, config.key, sign)) return true;
                config.PayLog.Add(Log.LogType.Debug | Log.LogType.Info, "签名验证错误 " + AutoCSer.Json.Serializer.Serialize(this));
            }
            return false;
        }
    }
}
