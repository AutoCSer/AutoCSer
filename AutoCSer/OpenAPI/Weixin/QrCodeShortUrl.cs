﻿using System;
using AutoCSer.Extensions;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 二维码短链接
    /// </summary>
    internal sealed class QrCodeShortUrl : ReturnSign
    {
#pragma warning disable
        /// <summary>
        /// 短链接
        /// </summary>
        public string short_url;
#pragma warning restore
        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal bool Verify(Config config)
        {
            if (IsReturn)
            {
                if (appid == config.appid && mch_id == config.mch_id && Sign<QrCodeShortUrl>.Check(this, config.key, sign)) return true;
                AutoCSer.LogHelper.Debug("签名验证错误 " + AutoCSer.JsonSerializer.Serialize(this), LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
            }
            return false;
        }
    }
}
