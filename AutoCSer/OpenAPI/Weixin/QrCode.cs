using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 二维码
    /// </summary>
    public sealed class QrCode : Return
    {
        /// <summary>
        /// 凭借此ticket可以在有效时间内换取二维码
        /// </summary>
        public string ticket;
        /// <summary>
        /// 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        /// </summary>
        public string url;
        /// <summary>
        /// 二维码的有效时间，以秒为单位。最大不超过1800
        /// </summary>
        public int expire_seconds;
    }
}
