using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 字典节点
    /// </summary>
    public abstract partial class ValueDictionary<keyType, valueType>
    {
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<ShortPath.Dictionary<keyType, valueType>>> CreateShortPathTask()
        {
            return await new ShortPath.Dictionary<keyType, valueType>(this).CreateTask();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTask(keyType key)
        {
            return new ReturnValue<valueType>(await ClientDataStructure.Client.QueryAwaiter(GetNode(key)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetTask(keyType key, valueType valueNode)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationAwaiter(GetSetNode(key, valueNode)));
        }
    }
}
