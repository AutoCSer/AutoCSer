using System;
using System.Threading;
using AutoCSer.Extension;
using AutoCSer.CacheServer;
using AutoCSer.CacheServer.DataStructure.Value;
using AutoCSer.CacheServer.DataStructure.MessageQueue;

namespace AutoCSer.TestCase.CacheServer
{
    /// <summary>
    /// 消息分发测试
    /// </summary>
    internal sealed class MesssageDistributor : IDisposable
    {
        /// <summary>
        /// 消息验证成功数据
        /// </summary>
        private const int successValue = (1 << 7) - (1 << 1); 

        /// <summary>
        /// 消费完毕等待事件
        /// </summary>
        private readonly AutoResetEvent wait = new AutoResetEvent(false);
        /// <summary>
        /// 消息分发 客户端消费者
        /// </summary>
        private readonly AutoCSer.CacheServer.MessageQueue.DistributionConsumer<Json<int>, int> consumer;
        /// <summary>
        /// 消息消费检测数据
        /// </summary>
        private int checkValue;
        /// <summary>
        /// 消息分发测试
        /// </summary>
        /// <param name="distributor"></param>
        private MesssageDistributor(Distributor<Json<int>> distributor)
        {
            consumer = distributor.CreateConsumer(check, new AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig { MemoryCacheNodeCount = 0 });
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            consumer.Dispose();
        }
        /// <summary>
        /// 消息队列测试
        /// </summary>
        /// <param name="value"></param>
        private void check(int value)
        {
            checkValue |= 1 << value;
            if(checkValue == successValue) wait.Set();
        }

        /// <summary>
        /// 消息分发测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase(Client masterClient, bool isStart)
        {
            string name = "messsageDistributor";
            Distributor<Json<int>> distributor = masterClient.GetOrCreateDataStructure<Distributor<Json<int>>>(name).Value;
            if (distributor == null)
            {
                return false;
            }
            if (isStart)
            {
                if (!start(distributor))
                {
                    return false;
                }
            }
            else
            {
                using (MesssageDistributor consumer = new MesssageDistributor(distributor))
                {
                    ReturnValue<bool> isEnqueue = distributor.Enqueue(5);
                    if (!isEnqueue.Value)
                    {
                        return false;
                    }
                    isEnqueue = distributor.Enqueue(3);
                    if (!isEnqueue.Value)
                    {
                        return false;
                    }
                    isEnqueue = distributor.Enqueue(1);
                    if (!isEnqueue.Value)
                    {
                        return false;
                    }
                    consumer.wait.WaitOne();
                    return consumer.checkValue == successValue;
                }
            }
            return true;
        }
        /// <summary>
        /// 消息队列测试
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static bool start(Distributor<Json<int>> queue)
        {
            ReturnValue<bool> isEnqueue = queue.Enqueue(2);
            if (!isEnqueue.Value)
            {
                return false;
            }
            isEnqueue = queue.Enqueue(4);
            if (!isEnqueue.Value)
            {
                return false;
            }
            isEnqueue = queue.Enqueue(6);
            if (!isEnqueue.Value)
            {
                return false;
            }
            return true;
        }
    }
}
