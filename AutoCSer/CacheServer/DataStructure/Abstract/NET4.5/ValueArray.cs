using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数组节点
    /// </summary>
    public abstract partial class ValueArray<nodeType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ReturnValueNode<nodeType>> GetTask(int index)
        {
            return new ReturnValueNode<nodeType>(await ClientDataStructure.Client.QueryTask(GetNode(index)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> SetTask(int index, nodeType valueNode)
        {
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) return Client.GetBool(await ClientDataStructure.Client.OperationTask(node));
            return new AutoCSer.CacheServer.ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
    }
}
