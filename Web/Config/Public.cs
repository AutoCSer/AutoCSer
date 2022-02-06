using System;
using System.Collections.Generic;

namespace AutoCSer.Web.Config
{
    /// <summary>
    /// 网站全部公共配置
    /// </summary>
    public class Public : AutoCSer.Configuration.Root
    {
        /// <summary>
        /// 公共配置类型集合
        /// </summary>
        public override IEnumerable<Type> PublicTypes { get { yield return typeof(Public); } }

        /// <summary>
        /// TCP 内部注册服务配置
        /// </summary>
        [AutoCSer.Configuration.Member(AutoCSer.Web.Config.Pub.TcpRegister)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute TcpRegisterServerAttribute
        {
            get
            {
                return Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Net.TcpRegister.Server));
            }
        }
        /// <summary>
        /// 进程复制重启服务配置
        /// </summary>
        [AutoCSer.Configuration.Member(AutoCSer.Diagnostics.ProcessCopyServer.ServerName)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute ProcessCopyServerAttribute
        {
            get
            {
                return Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Diagnostics.ProcessCopyServer));
            }
        }
    }
}
