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
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> OnLog;
        /// <summary>
        /// 自定义推送
        /// </summary>
        public Func<AutoCSer.Net.TcpServer.ReturnValue<byte[]>, bool> OnCustomPush { get; internal set; }
    }
}
