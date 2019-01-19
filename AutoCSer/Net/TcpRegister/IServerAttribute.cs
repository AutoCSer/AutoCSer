using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 服务配置接口
    /// </summary>
    internal interface IServerAttribute
    {
        /// <summary>
        /// 客户端访问的主机名称或者 IP 地址，用于需要使用端口映射服务。
        /// </summary>
        string ClientRegisterHost { get; set; }
        /// <summary>
        /// 客户端访问的监听端口，用于需要使用端口映射服务。
        /// </summary>
        int ClientRegisterPort { get; set; }
    }
}
