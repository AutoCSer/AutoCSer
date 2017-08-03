using System;

namespace AutoCSer.Web
{
    /// <summary>
    /// 域名重定向服务
    /// </summary>
    sealed class LocationServer : AutoCSer.Net.HttpDomainServer.LocationServer
    {
        /// <summary>
        /// 网站生成配置
        /// </summary>
        /// <returns>网站生成配置</returns>
        protected override AutoCSer.WebView.Config getWebConfig()
        {
            return WebServer.WebConfig;
        }
        /// <summary>
        /// 获取包含协议的重定向域名,比如 http://www.AutoCSer.com
        /// </summary>
        /// <returns>获取包含协议的重定向域名</returns>
        protected override string getLocationDomain()
        {
            return "http://" + AutoCSer.Web.Config.Web.MainDomain + "/";
        }
    }
}
