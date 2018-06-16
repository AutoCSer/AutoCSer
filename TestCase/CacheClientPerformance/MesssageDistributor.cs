using System;
using AutoCSer.Extension;
using AutoCSer.CacheServer.DataStructure.Value;
using AutoCSer.CacheServer.DataStructure.MessageQueue;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// 消息分发 测试
    /// </summary>
    sealed class MesssageDistributor : Test
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        private const string cacheName = "MesssageDistributor";
        /// <summary>
        /// 添加消息测试回调
        /// </summary>
        private readonly Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> enqueueCallbackReturnParameter;
        /// <summary>
        /// 添加消息测试回调
        /// </summary>
        private readonly Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> enqueueCallbackInterlockedReturnParameter;
        /// <summary>
        /// 测试消息分发
        /// </summary>
        private Distributor<Binary<Message>> distributor;
        /// <summary>
        /// 消息分发测试
        /// </summary>
        /// <param name="client">测试客户端</param>
        internal MesssageDistributor(AutoCSer.CacheServer.Client client) : base(client, true)
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
            distributor = client.GetOrCreateDataStructure<Distributor<Binary<Message>>>(cacheName);

            start(CallbackType.Asynchronous, TestType.MessageDistributionEnqueue);
            for (int index = 0; index != count; ++index) distributor.EnqueueStream(new Message { Value = index }, enqueueCallbackReturnParameter);
            wait();

            bitArray.SetAll(false);
        }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        private CacheServer.MessageQueue.DistributionConsumerConfig getConfig()
        {
            return new CacheServer.MessageQueue.DistributionConsumerConfig();
        }
        /// <summary>
        /// 消费者 IO 线程同步实时处理数据
        /// </summary>
        private void synchronousStream()
        {
            enqueue();

            start(CallbackType.SynchronousStream, TestType.MessageDistributionDequeue);
            using (AutoCSer.CacheServer.MessageQueue.Abstract.DistributionConsumer consumer1 = distributor.CreateConsumerStream(onMessage, getConfig()))
            using (AutoCSer.CacheServer.MessageQueue.Abstract.DistributionConsumer consumer2 = distributor.CreateConsumerStream(onMessage, getConfig()))
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

            start(CallbackType.ThreadPool, TestType.MessageDistributionDequeue);
            using (AutoCSer.CacheServer.MessageQueue.Abstract.DistributionConsumer consumer1 = distributor.CreateConsumer(onMessage, getConfig()))
            using (AutoCSer.CacheServer.MessageQueue.Abstract.DistributionConsumer consumer2 = distributor.CreateConsumer(onMessage, getConfig()))
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
            distributor = client.GetOrCreateDataStructure<Distributor<Binary<Message>>>(cacheName);
            bitArray.SetAll(false);
            errorCount = messageIndex = 0;
            start(CallbackType.SynchronousStream, TestType.MessageDistributionMixing, 2);
            using (AutoCSer.CacheServer.MessageQueue.Abstract.DistributionConsumer consumer1 = distributor.CreateConsumerStream(onMessageInterlocked, getConfig()))
            using (AutoCSer.CacheServer.MessageQueue.Abstract.DistributionConsumer consumer2 = distributor.CreateConsumerStream(onMessageInterlocked, getConfig()))
            {
                for (int index = 0; index != count; ++index) distributor.EnqueueStream(new Message { Value = index }, enqueueCallbackInterlockedReturnParameter);
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
            distributor = client.GetOrCreateDataStructure<Distributor<Binary<Message>>>(cacheName);
            bitArray.SetAll(false);
            errorCount = messageIndex = 0;
            start(CallbackType.ThreadPool, TestType.MessageDistributionMixing, 2);
            using (AutoCSer.CacheServer.MessageQueue.Abstract.DistributionConsumer consumer1 = distributor.CreateConsumer(onMessageInterlocked, getConfig()))
            using (AutoCSer.CacheServer.MessageQueue.Abstract.DistributionConsumer consumer2 = distributor.CreateConsumer(onMessageInterlocked, getConfig()))
            {
                for (int index = 0; index != count; ++index) distributor.EnqueueStream(new Message { Value = index }, enqueueCallbackInterlockedReturnParameter);
                wait();
            }
            client.RemoveDataStructure(cacheName);
        }
        /// <summary>
        /// 位图
        /// </summary>
        private readonly System.Collections.BitArray bitArray = new System.Collections.BitArray(loopCount);
        /// <summary>
        /// 消息处理访问锁
        /// </summary>
        private readonly object onMessageLock = new object();
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        private void onMessage(Message message)
        {
            System.Threading.Monitor.Enter(onMessageLock);
            if (message != null && !bitArray.Get(message.Value))
            {
                bitArray.Set(message.Value, true);
                setCallback(true);
            }
            else setCallback(false);
            System.Threading.Monitor.Exit(onMessageLock);
        }
        /// <summary>
        /// 消息编号
        /// </summary>
        private int messageIndex;
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
            System.Threading.Monitor.Enter(onMessageLock);
            if (message != null && !bitArray.Get(message.Value)) bitArray.Set(message.Value, true);
            else ++errorCount;
            if (++messageIndex == count) setCallbackInterlocked(errorCount);
            System.Threading.Monitor.Exit(onMessageLock);
        }
    }
}
