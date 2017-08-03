using System;

namespace AutoCSer.Web.DeployServer
{
    /// <summary>
    /// 部署服务配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// 进程复制重启服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.Diagnostics.ProcessCopyServer.ServerName)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute ProcessCopyServerAttribute
        {
            get
            {
                return AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Diagnostics.ProcessCopyServer));
            }
        }
    }
}
