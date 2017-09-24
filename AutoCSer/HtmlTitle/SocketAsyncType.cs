using System;

namespace AutoCSer.Net.HtmlTitle
{
    /// <summary>
    /// 套接字异步操作类型
    /// </summary>
    internal enum SocketAsyncType : byte
    {
        /// <summary>
        /// 连接处理
        /// </summary>
        Connect,
        /// <summary>
        /// 发送数据处理
        /// </summary>
        Send,
        /// <summary>
        /// 接收数据处理
        /// </summary>
        Recieve
    }
}
