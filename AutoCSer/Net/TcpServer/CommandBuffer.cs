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
        /// <param name="serviceName">服务名称</param>
        /// <param name="receiveBufferPool">接受数据缓存区池</param>
        /// <param name="sendBufferPool">发送接受数据缓存区池</param>
        /// <param name="sendBufferMaxSize">发送数据缓存区最大字节大小</param>
        /// <param name="minCompressSize">压缩启用最低字节数量</param>
        /// <param name="log">日志接口</param>
        internal CommandBuffer(string serviceName, SubBuffer.Pool receiveBufferPool, SubBuffer.Pool sendBufferPool, int sendBufferMaxSize, int minCompressSize, ILog log)
            :base(serviceName, sendBufferMaxSize, minCompressSize, log)
        {
            SendBufferPool = sendBufferPool;
            ReceiveBufferPool = receiveBufferPool;
        }
    }
    /// <summary>
    /// TCP 组件基类
    /// </summary>
    /// <typeparam name="attributeType"></typeparam>
    public abstract class CommandBuffer<attributeType> : CommandBuffer
        where attributeType : ServerBaseAttribute
    {
        /// <summary>
        /// TCP 服务调用配置
        /// </summary>
        internal readonly attributeType Attribute;
#if !NOJIT
        /// <summary>
        /// TCP 组件基类
        /// </summary>
        internal CommandBuffer() : base() { }
#endif
        /// <summary>
        /// TCP 组件基类
        /// </summary>
        /// <param name="attribute">TCP 服务调用配置</param>
        /// <param name="receiveBufferSize">接受数据缓冲区字节大小</param>
        /// <param name="sendBufferSize">发送数据缓冲区字节大小</param>
        /// <param name="sendBufferMaxSize">发送数据缓存区最大字节大小</param>
        /// <param name="log">日志接口</param>
        protected CommandBuffer(attributeType attribute, SubBuffer.Size receiveBufferSize, SubBuffer.Size sendBufferSize, int sendBufferMaxSize, ILog log)
            : base(attribute.ServerName, SubBuffer.Pool.GetPool(receiveBufferSize), SubBuffer.Pool.GetPool(sendBufferSize), sendBufferMaxSize, attribute.GetMinCompressSize, log)
        {
            Attribute = attribute;
        }
    }
}
