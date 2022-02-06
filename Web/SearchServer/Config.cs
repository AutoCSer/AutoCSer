using System;
using System.Collections.Generic;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索服务配置
    /// </summary>
    internal sealed class Config : AutoCSer.Web.Config.Public
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(Config); } }

        /// <summary>
        /// 搜索服务配置
        /// </summary>
        [AutoCSer.Configuration.Member(AutoCSer.Web.SearchServer.Server.ServerName)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute SearchServerAttribute
        {
            get
            {
                return AutoCSer.Web.Config.Pub.GetTcpStaticRegisterAttribute(typeof(AutoCSer.Web.SearchServer.Server), false);
            }
        }
    }
}
