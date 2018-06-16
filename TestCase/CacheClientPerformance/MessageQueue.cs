using System;
using AutoCSer.Extension;
using AutoCSer.CacheServer.DataStructure.Value;
using AutoCSer.CacheServer.DataStructure.MessageQueue;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// 消息队列 测试
    /// </summary>
    sealed class MessageQueue : Test
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        private const string cacheName = "MessageQueue";
        /// <summary>
        /// 添加消息测试回调
        /// </summary>
        private readonly Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> enqueueCallbackReturnParameter;
        /// <summary>
        /// 添加消息测试回调
        /// </summary>
        private readonly Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> enqueueCallbackInterlockedReturnParameter;
        /// <summary>
        /// 测试消息队列
        /// </summary>
        private QueueConsumer<Binary<Message>> queue;
        /// <summary>
        /// 消息队列测试
        /// </summary>
        /// <param name="client">测试客户端</param>
        internal MessageQueue(AutoCSer.CacheServer.Client client) : base(client, true)
        {
            enqueueCallbackReturnParameter = AutoCSer.CacheServer.ReturnParameter.GetCallback(setCallback);
            enqueueCallbackInterlockedReturnParameter = AutoCSer.CacheServer.ReturnParameter.GetCallback(setCallbackInterlocked);
        }
        /// <summary>
        /// 测试
        /// </summary>
        internal void Test()
        {
            threadPool();
            synchronousStream();

            threadPoolMixing();
            synchronousStreamMixing();
        }
        /// <summary>
        /// 添加数据到
        /// </summary>
        private void enqueue()
        {
            client.RemoveDataStructure(cacheName);
            queue = client.GetOrCreateDataStructure<QueueConsumer<Binary<Message>>>(cacheName);

            start(CallbackType.Asynchronous, TestType.MessageQueueEnqueue);
            for (int index = 0; index != count; ++index) queue.EnqueueStream(new Message { Value = index }, enqueueCallbackReturnParameter);
            wait();

            messageIndex = 0;
        }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        private CacheServer.MessageQueue.ConsumerConfig getConfig()
        {
            return new CacheServer.MessageQueue.ConsumerConfig();
        }
        /// <summary>
        /// 消费者 IO 线程同步实时处理数据
        /// </summary>
        private void synchronousStream()
        {
            enqueue();

            start(CallbackType.SynchronousStream, TestType.MessageQueueDequeue);
            using (AutoCSer.CacheServer.MessageQueue.Abstract.Consumer consumer = queue.CreateConsumerStream(onMessage, getConfig()))
            {
                wait();
            }
            client.RemoveDataStructure(cacheName);
        }
        /// <summary>
        /// 消费者 线程池轮询缓冲区
        /// </summary>
        private void threadPool()
        {
            enqueue();

            start(CallbackType.ThreadPool, TestType.MessageQueueDequeue);
            using (AutoCSer.CacheServer.MessageQueue.Abstract.Consumer consumer = queue.CreateConsumer(onMessage, getConfig()))
            {
                wait();
            }
            client.RemoveDataStructure(cacheName);
        }
        /// <summary>
        /// 消费者 IO 线程同步实时处理数据（生产消费实时并行测试）
        /// </summary>
        private void synchronousStreamMixing()
        {
            client.RemoveDataStructure(cacheName);
            queue = client.GetOrCreateDataStructure<QueueConsumer<Binary<Message>>>(cacheName);
            messageIndex = errorCount = 0;
            start(CallbackType.SynchronousStream, TestType.MessageQueueMixing, 2);
            using (AutoCSer.CacheServer.MessageQueue.Abstract.Consumer consumer = queue.CreateConsumerStream(onMessageInterlocked, getConfig()))
            {
                for (int index = 0; index != count; ++index) queue.EnqueueStream(new Message { Value = index }, enqueueCallbackInterlockedReturnParameter);
                wait();
            }
            client.RemoveDataStructure(cacheName);
        }
        /// <summary>
        /// 消费者 线程池轮询缓冲区（生产消费实时并行测试）
        /// </summary>
        private void threadPoolMixing()
        {
            client.RemoveDataStructure(cacheName);
            queue = client.GetOrCreateDataStructure<QueueConsumer<Binary<Message>>>(cacheName);
            messageIndex = errorCount = 0;
            start(CallbackType.ThreadPool, TestType.MessageQueueMixing, 2);
            using (AutoCSer.CacheServer.MessageQueue.Abstract.Consumer consumer = queue.CreateConsumer(onMessageInterlocked, getConfig()))
            {
                for (int index = 0; index != count; ++index) queue.EnqueueStream(new Message { Value = index }, enqueueCallbackInterlockedReturnParameter);
                wait();
            }
            client.RemoveDataStructure(cacheName);
        }
        /// <summary>
        /// 消息验证数据
        /// </summary>
        private int messageIndex;
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        private void onMessage(Message message)
        {
            setCallback(message != null && message.Value == messageIndex);
            ++messageIndex;
        }
        /// <summary>
        /// 错误次数
        /// </summary>
        private new int errorCount;
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        private void onMessageInterlocked(Message message)
        {
            if (message == null || message.Value != messageIndex) ++errorCount;
            if (++messageIndex == count) setCallbackInterlocked(errorCount);
        }
    }
}
