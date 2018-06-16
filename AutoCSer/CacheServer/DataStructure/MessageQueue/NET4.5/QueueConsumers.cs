using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.MessageQueue
{
    /// <summary>
    /// 多消费者队列消费节点（有序消费）
    /// </summary>
    public sealed partial class QueueConsumers<valueType>
    {
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <returns></returns>
        public async Task<ReturnValue<ulong>> GetDequeueIdentityTask(int readerIndex, Cache.MessageQueue.ReaderConfig config)
        {
            if ((uint)readerIndex < Cache.MessageQueue.ReaderConfig.MaxReaderCount) return Client.GetULong(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(getDequeueIdentityNode(readerIndex, config)));
            return new ReturnValue<ulong> { Type = ReturnType.MessageQueueReaderIndexOutOfRange };
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="readerIndex">消费读取编号（0-65535）</param>
        /// <param name="config">队列数据 读取配置</param>
        /// <returns></returns>
        public async Task<ReturnValue<ulong>> GetDequeueIdentityTask(IConvertible readerIndex, Cache.MessageQueue.ReaderConfig config)
        {
            if ((uint)readerIndex < Cache.MessageQueue.ReaderConfig.MaxReaderCount) return Client.GetULong(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(getDequeueIdentityNode(readerIndex.ToInt32(null), config)));
            return new ReturnValue<ulong> { Type = ReturnType.MessageQueueReaderIndexOutOfRange };
        }
    }
}
