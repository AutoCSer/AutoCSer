using System;

namespace AutoCSer.TestCase.ChatServer
{
    /// <summary>
    /// 服务端项目配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// TCP 任务处理配置
        /// </summary>
        [AutoCSer.Config.Member]
        internal static AutoCSer.Net.TcpServer.TaskConfig TcpTaskConfig
        {
            get { return new AutoCSer.Net.TcpServer.TaskConfig { ThreadCount = 1 }; }
        }
    }
}
