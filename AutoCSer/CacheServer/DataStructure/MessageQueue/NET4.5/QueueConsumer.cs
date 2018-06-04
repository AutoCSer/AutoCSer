using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.MessageQueue
{
    /// <summary>
    /// 队列消费节点（有序消费）
    /// </summary>
    public sealed partial class QueueConsumer<valueType>
    {
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="config">队列数据 读取配置</param>
        /// <returns></returns>
        public async Task<ReturnValue<ulong>> GetDequeueIdentityTask(Cache.MessageQueue.Config.QueueReader config)
        {
            return Client.GetULong(await ClientDataStructure.Client.OperationAsynchronousTask(GetDequeueIdentityNode(config)));
        }
    }
}
