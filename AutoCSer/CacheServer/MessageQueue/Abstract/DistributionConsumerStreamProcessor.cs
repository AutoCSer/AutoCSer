using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.MessageQueue.Abstract
{
    /// <summary>
    /// 消息分发 客户端消费者 处理器
    /// </summary>
    internal abstract class DistributionConsumerStreamProcessor
    {
        /// <summary>
        /// 消息分发 客户端消费者
        /// </summary>
        protected readonly DistributionConsumer consumer;
        /// <summary>
        /// 获取数据保持回调
        /// </summary>
        protected AutoCSer.Net.TcpServer.KeepCallback getMessageKeepCallback;
        /// <summary>
        /// 消息分发 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息分发 客户端消费者</param>
        internal DistributionConsumerStreamProcessor(DistributionConsumer consumer)
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
