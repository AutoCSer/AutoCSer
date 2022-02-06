using System;
using AutoCSer.Extensions;
using System.Threading;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息分发 客户端消费者 处理器
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class DistributionConsumerProcessorAsynchronous<valueType> : Abstract.DistributionConsumerProcessor<valueType>
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType, Action> onMessage;
        /// <summary>
        /// 消息处理完毕
        /// </summary>
        private readonly Action onMessageCompletedHandle;
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="onMessage">消息处理委托</param>
        internal DistributionConsumerProcessorAsynchronous(Abstract.DistributionConsumer consumer, Action<valueType, Action> onMessage) : base(consumer)
        {
            messageHandle = nextMessage;
            onMessageCompletedHandle = onMessageCompleted;
            this.onMessage = onMessage;
        }
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="onMessage">消息处理委托</param>
        internal DistributionConsumerProcessorAsynchronous(Abstract.DistributionConsumer consumer, ValueData.GetData<valueType> getValue, ValueData.DataType dataType, Action<valueType, Action> onMessage) : base(consumer, getValue, dataType)
        {
            messageHandle = nextMessage;
            onMessageCompletedHandle = onMessageCompleted;
            this.onMessage = onMessage;
        }
        /// <summary>
        /// 处理下一个消息
        /// </summary>
        private void nextMessage()
        {
            KeyValue<ulong, valueType>[] messages = this.messages;
            do
            {
                START:
                try
                {
                    if (messageIndex != messageWriteIndex)
                    {
                        if (consumer.IsProcessor(this)) onMessage(messages[messageIndex].Value, onMessageCompletedHandle);
                        else freeKeepCallback();
                        return;
                    }
                }
                catch (Exception error)
                {
                    consumer.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    Thread.Sleep(1);
                    goto START;
                }
                Thread.Sleep(0);
                if (messageIndex != messageWriteIndex) goto START;
                Interlocked.Exchange(ref isRead, 0);
            }
            while (messageIndex != messageWriteIndex && Interlocked.CompareExchange(ref isRead, 1, 0) == 0);
        }
        /// <summary>
        /// 消息处理完毕
        /// </summary>
        private void onMessageCompleted()
        {
            KeyValue<ulong, valueType>[] messages = this.messages;
            consumer.SetDequeueIdentity(messages[messageIndex].Key);
            if (++messageIndex == messages.Length) messageIndex = 0;
            nextMessage();
        }
    }
}
