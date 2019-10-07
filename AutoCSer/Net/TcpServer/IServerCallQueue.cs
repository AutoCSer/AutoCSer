using System;

namespace AutoCSer.Net.TcpServer
{
#if NOJIT
    /// <summary>
    /// TCP 服务器端同步调用队列接口
    /// </summary>
    public interface IServerCallQueue
    {
        /// <summary>
        /// 获取 TCP 服务器端同步调用队列
        /// </summary>
        /// <typeparam name="queueKeyType">关键字类型</typeparam>
        /// <param name="sender">TCP 服务套接字数据发送</param>
        /// <param name="queueKey">关键字</param>
        /// <returns>TCP 服务器端同步调用队列</returns>
        ServerCallQueue Get<queueKeyType>(ServerSocketSenderBase sender, ref queueKeyType queueKey);
    }
#else
    /// <summary>
    /// TCP 服务器端同步调用队列接口
    /// </summary>
    /// <typeparam name="queueKeyType">关键字类型</typeparam>
    public interface IServerCallQueue<queueKeyType>
    {
        /// <summary>
        /// 获取 TCP 服务器端同步调用队列
        /// </summary>
        /// <param name="sender">TCP 服务套接字数据发送</param>
        /// <param name="queueKey">关键字</param>
        /// <returns>TCP 服务器端同步调用队列</returns>
        ServerCallQueue Get(ServerSocketSenderBase sender, ref queueKeyType queueKey);
    }
#endif
}
