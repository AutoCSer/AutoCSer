using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数组节点
    /// </summary>
    public abstract partial class Array
    {
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetDefaultTask(int index)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationAwaiter(GetSetDefaultNode(index)));
        }
    }
    /// <summary>
    /// 数组节点
    /// </summary>
    public abstract partial class Array<nodeType>
    {
        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ReturnValue<nodeType>> GetOrCreateTask(int index)
        {
            AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await ClientDataStructure.Client.OperationAwaiter(GetOrCreateNode(index));
            return value.Value.Parameter.GetBool(value.Type, value.Value.Parameter.Int64.Bool ? this[index] : null);
        }
        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> IsNodeTask(int index)
        {
            return Client.GetBool(await ClientDataStructure.Client.QueryAwaiter(GetIsNode(index)));
        }
    }
}
