using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    internal sealed class ClientInfo
    {
        /// <summary>
        /// TCP 内部注册服务更新日志回调
        /// </summary>
        internal AutoCSer.Net.TcpServer.ServerCallback<ServerLog> OnLog;
        /// <summary>
        /// 是否需要移除
        /// </summary>
        internal bool IsRemove;
    }
}
