using System;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// 时间验证服务
    /// </summary>
    public abstract class TimeVerifyServer : TcpServer.TimeVerifyServer<ServerAttribute, Server, ServerSocket, ServerSocketSender>
    {
    }
}
