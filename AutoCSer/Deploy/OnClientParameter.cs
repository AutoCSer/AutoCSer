using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// TCP 客户端套接字事件参数
    /// </summary>
    public sealed class OnClientParameter
    {
        /// <summary>
        /// 服务命名
        /// </summary>
        public string ServerName;
        /// <summary>
        /// 事件类型
        /// </summary>
        public AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType Type;
    }
}
