using System;

namespace AutoCSer.Web.Config
{
    /// <summary>
    /// AutoCSer.Web.Web 项目配置
    /// </summary>
    public static class Web
    {
        /// <summary>
        /// 默认主域名
        /// </summary>
        public const string MainDomain = "www.autocser.com";
        /// <summary>
        /// 静态文件域名
        /// </summary>
        public const string StaticFileDomain = "f.autocser.com";
        /// <summary>
        /// WEB 客户端视图绑定类型前缀
        /// </summary>
        public const string WebViewClientTypePrefix = "AutoCSerWeb.";
        /// <summary>
        /// 重定向域名集合
        /// </summary>
        public static readonly string[] LocationDomains = new string[] { "autocser.com", "www.autocser.cn" };
    }
}
