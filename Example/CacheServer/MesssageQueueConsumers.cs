using System;
using System.Threading;
using AutoCSer.Extension;
using AutoCSer.CacheServer;
using AutoCSer.CacheServer.DataStructure.MessageQueue;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 消息队列测试（支持多消费者）
    /// </summary>
    internal class MesssageQueueConsumers
    {
        /// <summary>
        /// 消息队列测试（支持多消费者）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            string name = "MesssageQueueConsumers";

            #region 删除名称为 MesssageQueueConsumers 的缓存数据，防止未处理数据对测试验证产生副作用
            client.RemoveDataStructure(name);
            #endregion

            #region 创建名称为 MesssageQueueConsumers 的消息队列
            QueueConsumers<int> queue = client.GetOrCreateDataStructure<QueueConsumers<int>>(name).Value;
            if (queue == null)
            {
                return false;
            }
            #endregion

            #region 消息队列添加消息 2
            ReturnValue<bool> isEnqueue = queue.Enqueue(2);
            if (!isEnqueue.Value)
            {
                return false;
            }
            #endregion

            isEnqueue = queue.Enqueue(6);
            if (!isEnqueue.Value)
            {
                return false;
            }
            isEnqueue = queue.Enqueue(4);
            if (!isEnqueue.Value)
            {
                return false;
            }

            #region 初始化消息消费状态（仅用于当前测试验证）
            checkValue = 0;
            isSuccess = false;
            wait.Reset();
            #endregion

            #region 创建消息队列的消费者，readerIndex 参数最好使用枚举而不是魔鬼数字
            using (AutoCSer.CacheServer.MessageQueue.Consumer<int> consumer = queue.CreateConsumer(0, onMessage, new AutoCSer.CacheServer.MessageQueue.ConsumerConfig { MemoryCacheNodeCount = 0 }))
            #endregion
            {
                #region 等待消息处理完毕
                wait.WaitOne();
                #endregion
            }
            return isSuccess;
        }
        /// <summary>
        /// 消息状态验证等待事件
        /// </summary>
        private static readonly AutoResetEvent wait = new AutoResetEvent(false);
        /// <summary>
        /// 消息消费检测数据
        /// </summary>
        private static int checkValue;
        /// <summary>
        /// 消息验证是否成功
        /// </summary>
        private static bool isSuccess;
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="value"></param>
        private static void onMessage(int value)
        {
            switch (checkValue)
            {
                #region 第 1 个消息状态
                case 0: checkValue = 2; break;
                #endregion
                #region 第 2 个消息状态
                case 2: checkValue = 6; break;
                #endregion
                #region 第 3 个消息状态
                case 6: checkValue = 4; break;
                    #endregion
            }
            if (checkValue == value)
            {
                if (value == 4)
                {
                    #region 消息验证成功
                    isSuccess = true;
                    wait.Set();
                    #endregion
                }
                return;
            }
            #region 消息验证错误
            wait.Set();
            #endregion
        }
    }
}
