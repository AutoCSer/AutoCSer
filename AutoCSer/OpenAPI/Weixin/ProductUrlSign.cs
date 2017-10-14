using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 获取商品二维码URL
    /// </summary>
    internal sealed class ProductUrlSign : SignQuery
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string product_id;
        /// <summary>
        /// 时间戳,1970/1/1经过的秒数
        /// </summary>
        public long time_stamp = (long)(Date.NowTime.SetUtc() - Config.MinTime).TotalSeconds;
    }
}
