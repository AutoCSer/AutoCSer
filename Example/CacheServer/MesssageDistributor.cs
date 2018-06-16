using System;
using System.Threading;
using AutoCSer.Extension;
using AutoCSer.CacheServer;
using AutoCSer.CacheServer.DataStructure.MessageQueue;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 消息分发测试
    /// </summary>
    internal class MesssageDistributor
    {
        /// <summary>
        /// 消息分发测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            string name = "MesssageDistributor";

            #region 删除名称为 MesssageDistributor 的缓存数据，防止未处理数据对测试验证产生副作用
            client.RemoveDataStructure(name);
            #endregion

            #region 创建名称为 MesssageDistributor 的 JSON 消息分发
            Distributor<Json<int>> distributor = client.GetOrCreateDataStructure<Distributor<Json<int>>>(name).Value;
            if (distributor == null)
            {
                return false;
            }
            #endregion

            #region 添加消息 2
            ReturnValue<bool> isEnqueue = distributor.Enqueue(2);
            if (!isEnqueue.Value)
            {
                return false;
            }
            #endregion

            isEnqueue = distributor.Enqueue(6);
            if (!isEnqueue.Value)
            {
                return false;
            }
            isEnqueue = distributor.Enqueue(4);
            if (!isEnqueue.Value)
            {
                return false;
            }

            #region 初始化消息消费状态（仅用于当前测试验证）
            checkValue = 0;
            wait.Reset();
            #endregion

            #region 创建消息分发的消费者
            using (AutoCSer.CacheServer.MessageQueue.DistributionConsumer<Json<int>, int> consumer = distributor.CreateConsumer(onMessage, new AutoCSer.CacheServer.MessageQueue.DistributionConsumerConfig { MemoryCacheNodeCount = 0 }))
            #endregion
            {
                #region 等待消息处理完毕
                wait.WaitOne();
                #endregion
            }
            return checkValue == successValue;
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
        /// 消息验证成功数据
        /// </summary>
        private const int successValue = (1 << 2) | (1 << 4) | (1 << 6);
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="value"></param>
        private static void onMessage(int value)
        {
            checkValue |= 1 << value;
            if (checkValue == successValue)
            {
                #region 消息验证成功
                wait.Set();
                #endregion
            }
        }
    }
}
