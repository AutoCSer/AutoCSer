using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数组节点
    /// </summary>
    public abstract partial class NodeArray<nodeType>
    {
        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<nodeType>> GetOrCreateTask(int index)
        {
            AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await ClientDataStructure.Client.OperationTask(GetOrCreateNode(index));
            return value.Value.GetBool(value.Type, value.Value.Parameter.Int64.Bool ? this[index] : null);
        }
        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> IsNodeTask(int index)
        {
            return Client.GetBool(await ClientDataStructure.Client.QueryTask(GetIsNode(index)));
        }
    }
}
