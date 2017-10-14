using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 二维码长链接
    /// </summary>
    [AutoCSer.Xml.Serialize(Filter = Metadata.MemberFilters.InstanceField)]//, IsAllMember = true
    internal sealed class QrCodeLongUrl : SignQuery
    {
        /// <summary>
        /// 需要转换的URL，签名用原串，传输需URLencode
        /// </summary>
        public string long_url;
        /// <summary>
        /// 设置应用配置
        /// </summary>
        /// <param name="config">应用配置</param>
        internal void SetConfig(Config config)
        {
            setConfig(config);
            Sign<QrCodeLongUrl>.Set(this, config.key);
        }
    }
}
