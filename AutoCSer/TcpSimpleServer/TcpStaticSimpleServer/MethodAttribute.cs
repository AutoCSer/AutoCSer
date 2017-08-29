using System;

namespace AutoCSer.Net.TcpStaticSimpleServer
{
    /// <summary>
    /// TCP 调用函数配置
    /// </summary>
    public class MethodAttribute : TcpSimpleServer.MethodAttribute
    {
        /// <summary>
        /// 服务名称。如果不指定 Service，则默认绑定到该 class 申明配置的 Service；一个 class 中的不同函数可以绑定到不同服务名称。
        /// </summary>
        public string ServerName;
        /// <summary>
        /// 服务名称
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal virtual string GetServerName
        {
            get { return ServerName; }
        }
    }
}
