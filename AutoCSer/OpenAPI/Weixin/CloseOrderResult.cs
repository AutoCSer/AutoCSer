using System;
using AutoCSer.Extension;

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
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, "签名验证错误 " + AutoCSer.Json.Serializer.Serialize(this), new System.Diagnostics.StackFrame(), false);
            }
            return false;
        }
    }
}
