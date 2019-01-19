using System;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务客户端 绑定 TCP 服务客户端
    /// </summary>
    internal interface IClient
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        string ServerName { get; }
        /// <summary>
        /// 服务更新
        /// </summary>
        /// <param name="serverSet">TCP 服务信息集合</param>
        void OnServerChange(ServerSet serverSet);
    }
}
