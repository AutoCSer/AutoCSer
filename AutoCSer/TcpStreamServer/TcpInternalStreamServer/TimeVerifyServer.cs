using System;

namespace AutoCSer.Net.TcpInternalStreamServer
{
    /// <summary>
    /// 时间验证服务
    /// </summary>
    public abstract class TimeVerifyServer : TcpStreamServer.TimeVerifyServer<Server, ServerSocket, ServerSocketSender>
    {
    }
}
