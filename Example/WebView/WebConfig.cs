using System;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 网站生成配置
    /// </summary>
    [AutoCSer.WebView.Config]
    internal sealed class WebConfig : AutoCSer.WebView.Config
    {
        /// <summary>
        /// 默认主域名
        /// </summary>
        public override string MainDomain
        {
            get { return "127.0.0.1:14000"; }
        }

        /// <summary>
        /// HTTP URI 域名字符串前缀
        /// </summary>
        internal static readonly string HttpDomain = "http://" + new WebConfig().MainDomain + "/";
    }
}
