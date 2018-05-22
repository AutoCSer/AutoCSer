using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 搜索树字典节点
    /// </summary>
    public sealed partial class ValueSearchTreeDictionary<keyType, nodeType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ReturnValueNode<nodeType>> GetTask(keyType key)
        {
            return new ReturnValueNode<nodeType>(await ClientDataStructure.Client.QueryTask(GetNode(key)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> SetTask(keyType key, nodeType valueNode)
        {
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) return Client.GetBool(await ClientDataStructure.Client.OperationTask(node));
            return new AutoCSer.CacheServer.ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
    }
}
