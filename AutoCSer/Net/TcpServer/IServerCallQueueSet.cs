using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务器端同步调用队列接口集合
    /// </summary>
    public interface IServerCallQueueSet : IDisposable
    {
#if NOJIT
        /// <summary>
        /// 获取 TCP 服务器端同步调用队列接口
        /// </summary>
        /// <typeparam name="queueKeyType">关键字类型</typeparam>
        /// <returns>TCP 服务器端同步调用队列接口</returns>
        IServerCallQueue Get<queueKeyType>();
#else
        /// <summary>
        /// 获取 TCP 服务器端同步调用队列接口
        /// </summary>
        /// <typeparam name="queueKeyType">关键字类型</typeparam>
        /// <returns>TCP 服务器端同步调用队列接口</returns>
        IServerCallQueue<queueKeyType> Get<queueKeyType>();
#endif
    }
}
