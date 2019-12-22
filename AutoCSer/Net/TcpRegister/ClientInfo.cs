using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// TCP 内部注册服务更新日志回调
        /// </summary>
        public AutoCSer.Net.TcpServer.ServerCallback<ServerLog> OnLog { get; internal set; }
        /// <summary>
        /// 是否需要移除
        /// </summary>
        internal bool IsRemove;
    }
}
