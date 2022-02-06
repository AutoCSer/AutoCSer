using System;

namespace AutoCSer.Web
{
    /// <summary>
    /// 网站生成配置
    /// </summary>
    [AutoCSer.WebView.Config]
    internal sealed class WebViewConfig : AutoCSer.WebView.Config
    {
        /// <summary>
        /// 默认主域名
        /// </summary>
        public override string MainDomain
        {
            get { return AutoCSer.Web.Config.Web.MainDomain; }
        }
        /// <summary>
        /// 静态文件域名
        /// </summary>
        public override string StaticFileDomain
        {
            get { return AutoCSer.Web.Config.Web.StaticFileDomain; }
        }
        /// <summary>
        /// 本地 HTML 文件链接是否添加版本号
        /// </summary>
        public override bool IsHtmlLinkVersion { get { return true; } }
    }
}
