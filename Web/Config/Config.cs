using System;

namespace AutoCSer.Web.Config
{
    /// <summary>
    /// 网站全部公共配置
    /// </summary>
    //[AutoCSer.Config.Type]
    public class Config
    {
        /// <summary>
        /// TCP 内部注册服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.Web.Config.Pub.TcpRegister)]
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
        [AutoCSer.Config.Member(Name = AutoCSer.Diagnostics.ProcessCopyServer.ServerName)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute ProcessCopyServerAttribute
        {
            get
            {
                return Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Diagnostics.ProcessCopyServer));
            }
        }
    }
}
