using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 消息队列节点 短路径
    /// </summary>
    public sealed partial class MessageQueue<valueType>
    {
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> EnqueueTask(valueType value)
        {
            return Client.GetBool(await Client.MasterQueryAsynchronousAwaiter(GetEnqueueNode(value)));
        }
    }
}
