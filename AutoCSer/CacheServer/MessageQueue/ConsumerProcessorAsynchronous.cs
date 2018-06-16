using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息队列 客户端消费者 处理器
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class ConsumerProcessorAsynchronous<valueType> : Abstract.ConsumerProcessor<valueType>
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
        /// 消息处理时间
        /// </summary>
        private DateTime messageTime;
        /// <summary>
        /// 是否需要设置确认已完成消息标识
        /// </summary>
        private int isSetDequeueIdentity;
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="onMessage">消息处理委托</param>
        internal ConsumerProcessorAsynchronous(Abstract.Consumer consumer, Action<valueType, Action> onMessage) : base(consumer)
        {
            messageHandle = messageStart;
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
        internal ConsumerProcessorAsynchronous(Abstract.Consumer consumer, ValueData.GetData<valueType> getValue, ValueData.DataType dataType, Action<valueType, Action> onMessage) : base(consumer, getValue, dataType)
        {
            messageHandle = messageStart;
            onMessageCompletedHandle = onMessageCompleted;
            this.onMessage = onMessage;
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        private void messageStart()
        {
            messageTime = Date.NowTime.Now;
            isSetDequeueIdentity = 0;
            nextMessage();
        }
        /// <summary>
        /// 处理下一个消息
        /// </summary>
        private void nextMessage()
        {
            ulong identity = this.identity;
            int isSetDequeueIdentity = 1;
            try
            {
                do
                {
                    START:
                    try
                    {
                        if ((uint)identity != writeIdentity)
                        {
                            if (consumer.IsProcessor(this))
                            {
                                onMessage(messages[messageIndex], onMessageCompletedHandle);
                                isSetDequeueIdentity = 0;
                            }
                            else freeKeepCallback();
                            return;
                        }
                    }
                    catch (Exception error)
                    {
                        consumer.Log.Add(Log.LogType.Error, error);
                        if (identity != this.identity) return;
                        Thread.Sleep(1);
                        goto START;
                    }
                    Thread.Sleep(0);
                    if ((uint)identity != writeIdentity) goto START;
                    Interlocked.Exchange(ref isRead, 0);
                }
                while ((uint)identity != writeIdentity && Interlocked.CompareExchange(ref isRead, 1, 0) == 0);
            }
            finally
            {
                if ((isSetDequeueIdentity & this.isSetDequeueIdentity) != 0) consumer.SetDequeueIdentity(identity);
            }
        }
        /// <summary>
        /// 消息处理完毕
        /// </summary>
        private void onMessageCompleted()
        {
            if (++messageIndex == messages.Length) messageIndex = 0;
            ++identity;
            if (messageTime == Date.NowTime.Now && --sendClientCount != 0) isSetDequeueIdentity = 1;
            else
            {
                isSetDequeueIdentity = 0;
                consumer.SetDequeueIdentity(identity);
                messageTime = Date.NowTime.Now;
                sendClientCount = ((uint)messages.Length >> 1) | 1U;
            }
            nextMessage();
        }
    }
}

