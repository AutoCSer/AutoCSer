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
    }
}
