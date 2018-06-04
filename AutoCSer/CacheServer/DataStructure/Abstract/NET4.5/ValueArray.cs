using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数组节点
    /// </summary>
    public abstract partial class ValueArray<valueType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTask(int index)
        {
            return new ReturnValue<valueType>(await ClientDataStructure.Client.QueryTask(GetNode(index)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetTask(int index, valueType value)
        {
            Parameter.OperationBool node = GetSetNode(index, value);
            if (node != null) return Client.GetBool(await ClientDataStructure.Client.OperationTask(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
    }
}
