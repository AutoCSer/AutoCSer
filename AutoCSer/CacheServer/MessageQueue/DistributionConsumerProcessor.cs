using System;
using AutoCSer.Extension;
using System.Threading;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息分发 客户端消费者 处理器
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class DistributionConsumerProcessor<valueType> : Abstract.DistributionConsumerProcessor<valueType>
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType> onMessage;
        /// <summary>
        /// 消息分发 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息分发 客户端消费者</param>
        /// <param name="onMessage">消息处理委托</param>
        internal DistributionConsumerProcessor(Abstract.DistributionConsumer consumer, Action<valueType> onMessage) : base(consumer)
        {
            messageHandle = messageLoop;
            this.onMessage = onMessage;
        }
        /// <summary>
        /// 消息分发 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息分发 客户端消费者</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="onMessage">消息处理委托</param>
        internal DistributionConsumerProcessor(Abstract.DistributionConsumer consumer, ValueData.GetData<valueType> getValue, ValueData.DataType dataType, Action<valueType> onMessage) : base(consumer, getValue, dataType)
        {
            messageHandle = messageLoop;
            this.onMessage = onMessage;
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        private void messageLoop()
        {
            do
            {
                START:
                try
                {
                    while (messageIndex != messageWriteIndex)
                    {
                        if (consumer.IsProcessor(this))
                        {
                            onMessage(messages[messageIndex].Value);
                            consumer.SetDequeueIdentity(messages[messageIndex].Key);
                            if (++messageIndex == messages.Length) messageIndex = 0;
                        }
                        else
                        {
                            freeKeepCallback();
                            return;
                        }
                    }
                }
                catch (Exception error)
                {
                    consumer.Log.Add(Log.LogType.Error, error);
                    Thread.Sleep(1);
                    goto START;
                }
                Thread.Sleep(0);
                if (messageIndex != messageWriteIndex) goto START;
                Interlocked.Exchange(ref isRead, 0);
            }
            while (messageIndex != messageWriteIndex && Interlocked.CompareExchange(ref isRead, 1, 0) == 0);
        }
    }
}
