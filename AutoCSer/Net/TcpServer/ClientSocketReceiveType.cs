using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端套接字接收数据回调类型
    /// </summary>
    internal enum ClientSocketReceiveType : byte
    {
        /// <summary>
        /// 获取命令回调序号
        /// </summary>
        CommandIdentity,
        /// <summary>
        /// 获取数据
        /// </summary>
        Data,
        /// <summary>
        /// 获取临时数据
        /// </summary>
        BigData,
        /// <summary>
        /// 获取压缩数据
        /// </summary>
        CompressionData,
        /// <summary>
        /// 获取临时压缩数据
        /// </summary>
        CompressionBigData,
    }
}
