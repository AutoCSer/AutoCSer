using System;

namespace AutoCSer.Web.HttpServer
{
    /// <summary>
    /// HTTP 服务配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
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
        /// TCP 内部注册读取服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.Web.Config.Pub.TcpRegisterReader)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute TcpRegisterReaderServerAttribute
        {
            get
            {
                return AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Net.TcpRegister.ReaderServer));
            }
        }
        /// <summary>
        /// TCP 内部注册写服务配置
        /// </summary>
        [AutoCSer.Config.Member(Name = AutoCSer.Web.Config.Pub.TcpRegister)]
        internal static AutoCSer.Net.TcpInternalServer.ServerAttribute TcpRegisterServerAttribute
        {
            get
            {
                return AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Net.TcpRegister.Server));
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
                return AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Diagnostics.ProcessCopyServer));
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
