using System;
using AutoCSer.Log;
using System.Net;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 组件基类
    /// </summary>
    public abstract class CommandBuffer : CommandBase
    {
        /// <summary>
        /// 接受数据缓存区池
        /// </summary>
        internal readonly SubBuffer.Pool ReceiveBufferPool;
        /// <summary>
        /// 发送数据缓存区池
        /// </summary>
        internal readonly SubBuffer.Pool SendBufferPool;

#if !NOJIT
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        internal CommandBuffer() : base() { }
#endif
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP 服务调用配置</param>
        /// <param name="receiveBufferSize">接受数据缓冲区字节大小</param>
        /// <param name="sendBufferSize">发送数据缓冲区字节大小</param>
        /// <param name="sendBufferMaxSize">发送数据缓存区最大字节大小</param>
        /// <param name="log">日志接口</param>
        internal CommandBuffer(ServerBaseAttribute attribute, SubBuffer.Size receiveBufferSize, SubBuffer.Size sendBufferSize, int sendBufferMaxSize, ILog log)
            :base(attribute, sendBufferMaxSize, log)
        {
            SendBufferPool = SubBuffer.Pool.GetPool(sendBufferSize);
            ReceiveBufferPool = SubBuffer.Pool.GetPool(receiveBufferSize);
        }
    }
}
