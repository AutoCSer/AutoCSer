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
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTask(keyType key)
        {
            return new ReturnValue<valueType>(await ClientDataStructure.Client.QueryTask(GetNode(key)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetTask(keyType key, valueType valueNode)
        {
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) return Client.GetBool(await ClientDataStructure.Client.OperationTask(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
    }
}
