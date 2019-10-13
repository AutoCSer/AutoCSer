using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientObject
    {
        /// <summary>
        /// 部署任务状态更新回调
        /// </summary>
        internal AutoCSer.Net.TcpServer.ServerCallback<Log> OnLog;
        /// <summary>
        /// 自定义推送
        /// </summary>
        public AutoCSer.Net.TcpServer.ServerCallback<byte[]> OnCustomPush { get; internal set; }
    }
}
