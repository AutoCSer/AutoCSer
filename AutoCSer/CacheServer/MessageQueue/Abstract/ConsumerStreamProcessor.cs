using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue.Abstract
{
    /// <summary>
    /// 消息队列 客户端消费者 处理器
    /// </summary>
    internal abstract class ConsumerStreamProcessor
    {
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        protected readonly Consumer consumer;
        /// <summary>
        /// 获取数据保持回调
        /// </summary>
        protected AutoCSer.Net.TcpServer.KeepCallback getMessageKeepCallback;
        /// <summary>
        /// 读取消息起始标识
        /// </summary>
        protected ulong identity;
        /// <summary>
        /// 可发送的客户端未处理消息数量
        /// </summary>
        protected uint sendClientCount;
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        internal ConsumerStreamProcessor(Consumer consumer)
        {
            this.consumer = consumer;
        }
        /// <summary>
        /// 释放获取数据保持回调
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeMessageKeepCallback()
        {
            if (getMessageKeepCallback != null) getMessageKeepCallback.Dispose();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal virtual void Free()
        {
            FreeMessageKeepCallback();
        }
    }
}
