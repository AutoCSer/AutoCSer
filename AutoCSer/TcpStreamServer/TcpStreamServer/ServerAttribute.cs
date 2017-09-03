using System;

namespace AutoCSer.Net.TcpStreamServer
{
    /// <summary>
    /// TCP 服务配置
    /// </summary>
    public abstract class ServerAttribute : TcpServer.ServerAttribute
    {
        /// <summary>
        /// 客户端最大自定义数据包字节大小，0 表示不限
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetMaxCustomDataSize { get { return 0; } }
        /// <summary>
        /// 命令池初始化二进制大小 2^n
        /// </summary>
        internal override byte GetCommandPoolBitSize { get { return 0; } }
        /// <summary>
        /// 服务端任务类型，默认为 Queue
        /// </summary>
        public ServerTaskType ServerTaskType = ServerTaskType.Queue;
    }
}
