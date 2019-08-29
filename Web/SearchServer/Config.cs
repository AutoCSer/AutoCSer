using System;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索服务配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal class Config : AutoCSer.Web.Config.Config
    {
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
