using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.MessageQueue
{
    /// <summary>
    /// 消息队列 客户端消费者 处理器
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class ConsumerProcessor<valueType> : Abstract.ConsumerProcessor<valueType>
    {
        /// <summary>
        /// 消息处理委托
        /// </summary>
        private readonly Action<valueType> onMessage;
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="onMessage">消息处理委托</param>
        internal ConsumerProcessor(Abstract.Consumer consumer, Action<valueType> onMessage) : base(consumer)
        {
            messageHandle = messageLoop;
            this.onMessage = onMessage;
        }
        /// <summary>
        /// 消息队列 客户端消费者 处理器
        /// </summary>
        /// <param name="consumer">消息队列 客户端消费者</param>
        /// <param name="getValue">获取参数数据委托</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="onMessage">消息处理委托</param>
        internal ConsumerProcessor(Abstract.Consumer consumer, ValueData.GetData<valueType> getValue, ValueData.DataType dataType, Action<valueType> onMessage) : base(consumer, getValue, dataType)
        {
            messageHandle = messageLoop;
            this.onMessage = onMessage;
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        private void messageLoop()
        {
            DateTime messageTime = Date.NowTime.Now;
            int isSetDequeueIdentity = 0;
            try
            {
                do
                {
                    START:
                    try
                    {
                        while ((uint)identity != writeIdentity)
                        {
                            if (consumer.IsProcessor(this))
                            {
                                onMessage(messages[messageIndex]);
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
                    if ((uint)identity != writeIdentity) goto START;
                    Interlocked.Exchange(ref isRead, 0);
                }
                while ((uint)identity != writeIdentity && Interlocked.CompareExchange(ref isRead, 1, 0) == 0);
            }
            finally
            {
                if (isSetDequeueIdentity != 0) consumer.SetDequeueIdentity(identity);
            }
        }
    }
}
