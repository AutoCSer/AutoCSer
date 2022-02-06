using System;
using System.Collections.Generic;

namespace AutoCSer.Web
{
    /// <summary>
    /// 网站配置
    /// </summary>
    internal sealed class WebConfig : AutoCSer.Web.Config.Public
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public override IEnumerable<Type> MainTypes { get { yield return typeof(WebConfig); } }


        /// <summary>
        /// TCP 内部注册服务配置
        /// </summary>
        [AutoCSer.Configuration.Member(AutoCSer.Web.Config.Pub.TcpRegister)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute TcpRegisterServerAttribute
        {
            get
            {
                return AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Net.TcpRegister.Server));
            }
        }
    }
}
