using System;
using AutoCSer.Extensions;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 关闭订单结果
    /// </summary>
    internal sealed class CloseOrderResult : ReturnSign
    {
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal bool Verify(Config config)
        {
            if (IsReturn)
            {
                if (appid == config.appid && mch_id == config.mch_id && Sign<CloseOrderResult>.Check(this, config.key, sign)) return true;
                AutoCSer.LogHelper.Debug("签名验证错误 " + AutoCSer.JsonSerializer.Serialize(this), LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
            }
            return false;
        }
    }
}
