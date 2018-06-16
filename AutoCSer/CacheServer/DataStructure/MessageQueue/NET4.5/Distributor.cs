using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.MessageQueue
{
    /// <summary>
    /// 消费分发节点（无序消费）
    /// </summary>
    public sealed partial class Distributor<valueType>
    {
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="config">消息分发 读取配置</param>
        /// <returns></returns>
        public async Task<ReturnValue<uint>> GetSendCountTask(Cache.MessageQueue.DistributionConfig config)
        {
            return Client.GetUInt(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(GetSendCountNode(config)));
        }
    }
}
