using System;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// TCP 服务配置
    /// </summary>
    public abstract class ServerAttribute : TcpServer.ServerBaseAttribute
    {
        /// <summary>
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override SubBuffer.Size GetReceiveBufferSize { get { return GetSendBufferSize; } }
    }
}
