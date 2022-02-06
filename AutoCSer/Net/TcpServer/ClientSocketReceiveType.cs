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
        /// 继续获取命令回调序号
        /// </summary>
        CommandIdentityAgain,
        /// <summary>
        /// 获取数据
        /// </summary>
        Data,
        /// <summary>
        /// 获取临时数据
        /// </summary>
        BigData,
    }
}
