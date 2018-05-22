using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 哈希表节点
    /// </summary>
    public abstract partial class HashSet<valueType>
    {
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> ContainsTask(valueType value)
        {
            return Client.GetBool(await ClientDataStructure.Client.QueryTask(GetContainsKeyNode(value)));
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> RemoveTask(valueType value)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationTask(GetRemoveNode(value)));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> AddTask(valueType value)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationTask(GetAddNode(value)));
        }
    }
}
