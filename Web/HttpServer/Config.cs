using System;
using System.Collections.Generic;

namespace AutoCSer.Web.HttpServer
{
    /// <summary>
    /// HTTP 服务配置
    /// </summary>
    internal class Config : AutoCSer.Web.Config.Public
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// 公用全局配置
        /// </summary>
        [AutoCSer.Configuration.Member]
        internal static AutoCSer.Config Pub
        {
            get
            {
                AutoCSer.Config pub = new AutoCSer.Config();
                pub.CachePath = AutoCSer.Web.Config.Http.CachePath;
                return pub;
            }
        }
        /// <summary>
        /// 搜索服务配置
        /// </summary>
        [AutoCSer.Configuration.Member(AutoCSer.Web.SearchServer.Server.ServerName)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute SearchServerAttribute
        {
            get
            {
                return AutoCSer.Web.Config.Pub.GetTcpStaticRegisterAttribute(typeof(AutoCSer.Web.SearchServer.Server), true);
            }
        }
    }
}
