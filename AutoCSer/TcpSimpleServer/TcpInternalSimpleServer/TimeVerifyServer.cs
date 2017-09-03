using System;

namespace AutoCSer.Net.TcpInternalSimpleServer
{
    /// <summary>
    /// 时间验证服务
    /// </summary>
    public abstract class TimeVerifyServer : TcpSimpleServer.TimeVerifyServer<ServerAttribute, Server, ServerSocket>
    {
    }
}
