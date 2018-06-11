using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 消息队列节点
    /// </summary>
    public abstract partial class MessageQueue<valueType>
    {
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<ShortPath.MessageQueue<valueType>>> CreateShortPathTask()
        {
            return await new ShortPath.MessageQueue<valueType>(this).CreateTask();
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> EnqueueTask(valueType value)
        {
            return Client.GetBool(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(getEnqueueNode(value)));
        }
    }
}
