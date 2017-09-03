using System;

namespace AutoCSer.Net.TcpStreamServer.ServerOutput
{
    /// <summary>
    /// TCP 服务端套接字输出信息
    /// </summary>
    internal abstract class OutputLink : TcpServer.ServerOutput.OutputLink<OutputLink>
    {
        /// <summary>
        /// TCP 服务端输出数据长度
        /// </summary>
        internal const int OutputDataSize = 8;
    }
}
