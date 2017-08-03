using System;

namespace AutoCSer.Web
{
    /// <summary>
    /// 静态文件服务
    /// </summary>
     sealed class FileServer : AutoCSer.Net.HttpDomainServer.StaticFileServer
    {
        /// <summary>
        /// 网站生成配置
        /// </summary>
        /// <returns>网站生成配置</returns>
        protected override AutoCSer.WebView.Config getWebConfig()
        {
            return WebServer.WebConfig;
        }
    }
}
