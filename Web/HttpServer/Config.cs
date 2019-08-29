using System;

namespace AutoCSer.Web.HttpServer
{
    /// <summary>
    /// HTTP 服务配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal class Config : AutoCSer.Web.Config.Config
    {
        /// <summary>
        /// 公用全局配置
        /// </summary>
        [AutoCSer.Config.Member]
        internal static AutoCSer.Config.Pub Pub
        {
            get
            {
                AutoCSer.Config.Pub pub = new AutoCSer.Config.Pub();
                pub.CachePath = AutoCSer.Web.Config.Http.CachePath;
                return pub;
            }
        }
        /// <summary>
        /// 搜索服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.Web.SearchServer.Server.ServerName)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute SearchServerAttribute
        {
            get
            {
                return AutoCSer.Web.Config.Pub.GetTcpStaticRegisterAttribute(typeof(AutoCSer.Web.SearchServer.Server));
            }
        }
    }
}
