using System;
using System.Threading;
using AutoCSer.Extension;
using AutoCSer.CacheServer;
using AutoCSer.CacheServer.DataStructure.MessageQueue;

namespace AutoCSer.TestCase.CacheServer
{
    /// <summary>
    /// 消息队列测试
    /// </summary>
    internal sealed class QueueConsumers : IDisposable
    {
        /// <summary>
        /// 消费完毕等待事件
        /// </summary>
        private readonly AutoResetEvent wait = new AutoResetEvent(false);
        /// <summary>
        /// 消息队列 客户端消费者
        /// </summary>
        private readonly AutoCSer.CacheServer.MessageQueue.Consumer<int> consumer;
        /// <summary>
        /// 当前测试阶段
        /// </summary>
        private int step;
        /// <summary>
        /// 是否测试成功
        /// </summary>
        private bool isSuccess;
        /// <summary>
        /// 消息队列测试
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="readerIndex"></param>
        /// <param name="isFirst"></param>
        private QueueConsumers(QueueConsumers<int> queue, int readerIndex, bool isFirst)
        {
            consumer = queue.CreateConsumer(readerIndex, isFirst ? (Action<int>)first : check, new AutoCSer.CacheServer.MessageQueue.ConsumerConfig { MemoryCacheNodeCount = 0 });
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            consumer.Dispose();
        }
        /// <summary>
        /// 第一次消息队列测试
        /// </summary>
        /// <param name="value"></param>
        private void first(int value)
        {
            switch (value)
            {
                case 1: case 2: case 3: case 4: case 5: case 6: return;
                case 7:
                    isSuccess = true;
                    wait.Set();
                    return;
            }
            wait.Set();
        }
        /// <summary>
        /// 消息队列测试
        /// </summary>
        /// <param name="value"></param>
        private void check(int value)
        {
            switch (step)
            {
                case 0: step = 2; break;
                case 2: step = 4; break;
                case 4: step = 6; break;
                case 6: step = 5; break;
                case 5: step = 3; break;
                case 3: step = 1; break;
            }
            if (value == step)
            {
                if (value == 1)
                {
                    isSuccess = true;
                    wait.Set();
                }
                return;
            }
            else if (value == 7)
            {
                step = 0;
                return;
            }
            wait.Set();
        }

        /// <summary>
        /// 消息队列测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase(Client masterClient, bool isStart)
        {
            string name = "messageQueueConsumers";
            QueueConsumers<int> queue = masterClient.GetOrCreateDataStructure<QueueConsumers<int>>(name).Value;
            if (queue == null)
            {
                return false;
            }
            if (!checkFirst(queue, isStart))
            {
                return false;
            }
            if (isStart)
            {
                if (!start(queue))
                {
                    return false;
                }
            }
            else
            {
                using (QueueConsumers consumer1 = new QueueConsumers(queue, 1, false))
                using (QueueConsumers consumer2 = new QueueConsumers(queue, 2, false))
                {
                    ReturnValue<bool> isEnqueue = queue.Enqueue(5);
                    if (!isEnqueue.Value)
                    {
                        return false;
                    }
                    isEnqueue = queue.Enqueue(3);
                    if (!isEnqueue.Value)
                    {
                        return false;
                    }
                    isEnqueue = queue.Enqueue(1);
                    if (!isEnqueue.Value)
                    {
                        return false;
                    }
                    consumer1.wait.WaitOne();
                    consumer2.wait.WaitOne();
                    return consumer1.isSuccess && consumer2.isSuccess;
                }
            }
            return true;
        }
        /// <summary>
        /// 是否第一次测试
        /// </summary>
        private static bool isFirst = true;
        /// <summary>
        /// 第一次消息队列测试清除历史数据
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool checkFirst(QueueConsumers<int> queue, bool isStart)
        {
            if (isFirst)
            {
                isFirst = false;
                using (QueueConsumers consumer1 = new QueueConsumers(queue, 1, true))
                using (QueueConsumers consumer2 = new QueueConsumers(queue, 2, true))
                {
                    ReturnValue<bool> isEnqueue = queue.Enqueue(7);
                    if (!isEnqueue.Value)
                    {
                        return false;
                    }
                    consumer1.wait.WaitOne();
                    consumer2.wait.WaitOne();
                    if (!consumer1.isSuccess || !consumer2.isSuccess) return false;
                }
                if (!isStart && !start(queue))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 消息队列测试
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static bool start(QueueConsumers<int> queue)
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
