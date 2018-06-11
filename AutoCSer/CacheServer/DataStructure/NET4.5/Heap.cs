using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 最小堆节点
    /// </summary>
    public sealed partial class Heap<keyType, valueType>
    {
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<ShortPath.Heap<keyType, valueType>>> CreateShortPathTask()
        {
            return await new ShortPath.Heap<keyType, valueType>(this).CreateTask();
        }
        /// <summary>
        /// 获取堆顶关键字（最小值）
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<keyType>> GetTopKeyTask()
        {
            return new ReturnValue<keyType>(await ClientDataStructure.Client.QueryAwaiter(GetTopKeyNode()));
        }
        /// <summary>
        /// 获取堆顶数据（最小关键字）
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTopValueTask()
        {
            return new ReturnValue<valueType>(await ClientDataStructure.Client.QueryAwaiter(GetTopValueNode()));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> PushTask(keyType key, valueType value)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationAwaiter(GetPushNode(key, value)));
        }
        /// <summary>
        /// 删除堆顶数据（最小关键字）
        /// </summary>
        public async Task<ReturnValue<bool>> PopTopTask()
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationAwaiter(GetPopTopNode()));
        }
        /// <summary>
        /// 删除堆顶数据并返回关键字（最小关键字）
        /// </summary>
        public async Task<ReturnValue<keyType>> GetPopTopKeyTask()
        {
            return new ReturnValue<keyType>(await ClientDataStructure.Client.OperationAwaiter(GetPopTopKeyNode()));
        }
        /// <summary>
        /// 删除堆顶数据并返回数据（最小关键字）
        /// </summary>
        public async Task<ReturnValue<valueType>> GetPopTopValueTask()
        {
            return new ReturnValue<valueType>(await ClientDataStructure.Client.OperationAwaiter(GetPopTopValueNode()));
        }
    }
}
